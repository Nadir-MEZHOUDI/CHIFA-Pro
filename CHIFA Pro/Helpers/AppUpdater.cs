using System.ComponentModel;
using System.Net.Sockets;
using DevExpress.XtraBars.Alerter;
using Velopack;
using Velopack.Exceptions;

namespace CHIFA.Pro.Helpers;

/// <summary>Velopack updates of SmartCHIFA: at startup, on demand, and every hour in the background.</summary>
internal sealed class AppUpdater(
    Form owner,
    IContainer components,
    ToolStripProgressBar progressBar,
    StatusStrip statusStrip,
    Func<string, string, ToolTipIcon, Task> notify)
{
    private const string UpdateFeedUrl = "https://nadirsmartapp.blob.core.windows.net/chifa-pro";
    private const string Title = "Mise à jour";
    private static readonly TimeSpan UpdateCheckInterval = TimeSpan.FromMinutes(60);
    private static readonly SemaphoreSlim UpdateGate = new(1, 1);

    private System.Windows.Forms.Timer? _updateTimer;
    private VelopackAsset? _readyUpdate;
    private ToolStripStatusLabel? _restartLabel;
    private AlertControl? _updateAlert;

    /// <summary>Downloads a new version with the progress bar shown and restarts into it at once.</summary>
    public Task UpdateNowAsync(bool showMessage = false) => CheckAndDownloadAsync(applyNow: true, showMessage);

    /// <summary>Pharmacies leave SmartCHIFA open all day: look for a new version every hour, not only at startup.</summary>
    public void StartHourlyCheck()
    {
        if (_updateTimer is not null) return;
        _updateTimer = new System.Windows.Forms.Timer { Interval = (int)UpdateCheckInterval.TotalMilliseconds };
        // In the background the update is downloaded quietly and only the restart is offered, so nobody is interrupted mid-work.
        _updateTimer.Tick += async (_, _) => await CheckAndDownloadAsync(applyNow: false, showMessage: false);
        _updateTimer.Start();
        owner.FormClosed += (_, _) => _updateTimer.Dispose();
    }

    private async Task CheckAndDownloadAsync(bool applyNow, bool showMessage)
    {
        if (!await UpdateGate.WaitAsync(0))
        {
            if (showMessage)
                await notify("Une mise à jour est déjà en cours", Title, ToolTipIcon.Info);
            return;
        }

        try
        {
            if (applyNow)
                progressBar.Visible = true;
            var mgr = new UpdateManager(UpdateFeedUrl);

            if (!mgr.IsInstalled)
            {
                if (applyNow)
                {
                    progressBar.Value = 50;
                    await Task.Delay(2000);
                }
                return;
            }

            var update = await mgr.CheckForUpdatesAsync();
            if (update is null)
            {
                if (showMessage)
                    await notify("Vous disposez de la dernière version", Title, ToolTipIcon.Info);
                return;
            }

            if (applyNow)
            {
                await mgr.DownloadUpdatesAsync(update, i => owner.Invoke(() => progressBar.Value = i));
                ApplyUpdateAndRestart(mgr, update.TargetFullRelease);
                return;
            }

            // Already downloaded and offered (or declined) in this run.
            if (Equals(update.TargetFullRelease.Version, _readyUpdate?.Version)) return;

            await mgr.DownloadUpdatesAsync(update);
            _readyUpdate = update.TargetFullRelease;
            OfferRestart(_readyUpdate.Version.ToString());
        }
        catch (Exception ex) when (!applyNow && (FindInChain<AcquireLockFailedException>(ex) is not null || IsUpdateServerUnreachable(ex)))
        {
            Serilog.Log.Warning(ex, "Hourly update check skipped, retrying in {Interval}", UpdateCheckInterval);
        }
        catch (Exception ex) when (FindInChain<AcquireLockFailedException>(ex) is not null)
        {
            Serilog.Log.Information(ex, "Update skipped: another update operation holds the Velopack lock");
            if (showMessage)
                await notify("Une mise à jour est déjà en cours", Title, ToolTipIcon.Info);
        }
        catch (Exception ex) when (IsUpdateServerUnreachable(ex))
        {
            // The next startup or manual check retries; reporting these would only measure customers' internet.
            Serilog.Log.Warning(ex, "Update check skipped: update server unreachable");
            if (showMessage)
                await notify("Serveur de mise à jour injoignable. Vérifiez votre connexion Internet et réessayez plus tard.",
                    Title, ToolTipIcon.Warning);
        }
        catch (Exception ex)
        {
            ex.Log(showMessage);
        }
        finally
        {
            if (applyNow)
                progressBar.Visible = false;
            UpdateGate.Release();
        }
    }

    private void OfferRestart(string version)
    {
        if (_restartLabel is null)
        {
            _restartLabel = new ToolStripStatusLabel { IsLink = true, ForeColor = Color.DarkGreen };
            _restartLabel.Click += (_, _) => RestartToUpdate();
            statusStrip.Items.Insert(0, _restartLabel);
        }
        _restartLabel.Text = $"Redémarrer pour mettre à jour (V {version})";

        if (!owner.Visible)
        {
            // In the tray, a downloaded update is applied anyway at the next start.
            _ = notify($"La version {version} est prête. Elle sera installée au prochain démarrage.", Title, ToolTipIcon.Info);
            return;
        }

        if (_updateAlert is null)
        {
            _updateAlert = new AlertControl(components) { AutoFormDelay = 15_000 };
            _updateAlert.AlertClick += (_, _) => RestartToUpdate();
        }
        _updateAlert.Show(owner, "Mise à jour disponible",
            $"SmartCHIFA {version} est prêt. Cliquez ici pour redémarrer et mettre à jour.");
    }

    private void RestartToUpdate()
    {
        if (_readyUpdate is null) return;
        if (!UpdateGate.Wait(0))
        {
            _ = notify("Une mise à jour est déjà en cours", Title, ToolTipIcon.Info);
            return;
        }

        try
        {
            ApplyUpdateAndRestart(new UpdateManager(UpdateFeedUrl), _readyUpdate);
        }
        catch (Exception ex)
        {
            ex.Log();
        }
        finally
        {
            UpdateGate.Release();
        }
    }

    private void ApplyUpdateAndRestart(UpdateManager mgr, VelopackAsset update)
    {
        // install-agent.ps1 copies the service from our agent\ folder, which applying the update replaces.
        if (Mobile.AgentService.IsInstalling)
        {
            Serilog.Log.Information("Update {Version} postponed to the next start: the agent service is being installed", update.Version);
            _ = notify("La mise à jour sera installée au prochain démarrage.", Title, ToolTipIcon.Info);
            return;
        }

        mgr.ApplyUpdatesAndRestart(update);
    }

    private static bool IsUpdateServerUnreachable(Exception ex) =>
        FindInChain<HttpRequestException>(ex) is not null ||
        FindInChain<SocketException>(ex) is not null ||
        FindInChain<TimeoutException>(ex) is not null ||
        ex is OperationCanceledException;

    private static T? FindInChain<T>(Exception? ex) where T : Exception
    {
        for (; ex is not null; ex = ex.InnerException)
        {
            if (ex is T match)
                return match;
            if (ex is AggregateException agg)
                foreach (var inner in agg.InnerExceptions)
                    if (FindInChain<T>(inner) is { } found)
                        return found;
        }
        return null;
    }
}
