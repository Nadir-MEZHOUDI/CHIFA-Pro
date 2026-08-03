using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Serilog;
using System.Net.Http;
using Velopack;

namespace CHIFA.Server.Helpers;

public partial class UpdateService : ObservableObject
{
    private readonly ILogger _logger = Log.ForContext<UpdateService>();
    private readonly string _updateUrl = "https://nadirsmartapp.blob.core.windows.net/chifa-server";
    private readonly HttpClient _httpClient;
    private readonly UpdateManager _updateManager;
    private bool _isChecking;
    private bool _isUpdating;

    [ObservableProperty] private int _progress;
    [ObservableProperty] private string _status = "جاهز";
    [ObservableProperty] private bool _updateAvailable;
    [ObservableProperty] private Version _currentVersion = typeof(UpdateService).Assembly.GetName().Version!;
    [ObservableProperty] private SemanticVersion _newVersion = null!;

    public UpdateService()
    {
        _httpClient = new HttpClient { Timeout = TimeSpan.FromMinutes(5) };
        _updateManager = new UpdateManager(_updateUrl);
    }

    [RelayCommand(AllowConcurrentExecutions = false)]
    private async Task CheckUpdate(bool silent = false)
    {
        if (_isChecking) return;
        _isChecking = true;

        try
        {
            Status = "جاري التحقق من التحديثات...";
            _logger.Information("Checking for updates...");

            if (!_updateManager.IsInstalled)
            {
                Status = "التطبيق غير مثبت بشكل صحيح";
                _logger.Warning("Application is not installed properly");
                return;
            }

            // التحقق من الاتصال بالإنترنت
            if (!await CheckInternetConnection())
            {
                Status = "لا يوجد اتصال بالإنترنت";
                _logger.Warning("No internet connection");
                return;
            }

            var result = await _updateManager.CheckForUpdatesAsync();

            if (result == null)
            {
                Status = "لا يوجد تحديثات جديدة";
                UpdateAvailable = false;
                if (!silent) 
                    _logger.Information("No updates available");
            }
            else
            {
                NewVersion = result.TargetFullRelease.Version;
                Status = $"يوجد تحديث جديد: {NewVersion}";
                UpdateAvailable = true;
                _logger.Information("Update available: {Version}", NewVersion);
            }
        }
        catch (Exception ex)
        {
            Status = "حدث خطأ أثناء التحقق من التحديثات";
            _logger.Error(ex, "Error checking for updates");
        }
        finally
        {
            _isChecking = false;
        }
    }

    [RelayCommand(AllowConcurrentExecutions = false)]
    private async Task UpdateApp()
    {
        if (_isUpdating) return;
        _isUpdating = true;

        try
        {
            Status = "جاري تحميل التحديث...";
            _logger.Information("Starting update process");

            if (!await CheckInternetConnection())
            {
                Status = "لا يوجد اتصال بالإنترنت";
                return;
            }

            if (!_updateManager.IsInstalled)
            {
                Status = "التطبيق غير مثبت بشكل صحيح";
                _logger.Warning("Application is not installed properly");
                return;
            }

            var newVersion = await _updateManager.CheckForUpdatesAsync();
            if (newVersion == null)
            {
                Status = "لا يوجد تحديثات جديدة";
                return;
            }

            // تحميل التحديث مع إظهار التقدم
            await _updateManager.DownloadUpdatesAsync(newVersion, progress =>
            {
                Progress = progress;
                Status = $"جاري التحميل... {progress}%";
            });

            Status = "جاري تثبيت التحديث...";
            _logger.Information("Installing update {Version}", newVersion.TargetFullRelease.Version);

            // تثبيت التحديث وإعادة تشغيل التطبيق
            _updateManager.ApplyUpdatesAndRestart(newVersion);
        }
        catch (Exception ex)
        {
            Status = "حدث خطأ أثناء التحديث";
            _logger.Error(ex, "Error updating application");
            Progress = 0;
        }
        finally
        {
            _isUpdating = false;
        }
    }

    private async Task<bool> CheckInternetConnection()
    {
        try
        {
            using var response = await _httpClient.GetAsync("https://8.8.8.8", HttpCompletionOption.ResponseHeadersRead);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}