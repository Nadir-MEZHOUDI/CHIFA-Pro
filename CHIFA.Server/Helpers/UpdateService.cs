using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Velopack;

namespace CHIFA.Server.Helpers;

public partial class UpdateService : ObservableObject
{
    [ObservableProperty] private int _progress;

    [RelayCommand(AllowConcurrentExecutions = false)]
    private Task UpdateApp()
    {
        return Task.Run(async () =>
        {
            try
            {
              //  using var op = TimedOperation.Create();
                var mgr = new UpdateManager("https://nadirsmartapp.blob.core.windows.net/smart-pharm");

                if (!mgr.IsInstalled) return;
                // check for new version
                var newVersion = await mgr.CheckForUpdatesAsync();
                if (newVersion != null)
                {
                    // download new version
                    await mgr.DownloadUpdatesAsync(newVersion, i => Progress = i);
                    // install new version and restart app
                    mgr.ApplyUpdatesAndRestart(newVersion);
                }
            }
            catch (Exception ex)
            {
                ex.Log();
            }
        });
    }
}