using System.IO;

using CommunityToolkit.Mvvm.ComponentModel;

namespace CHIFA.Server.Helpers.Settings;

public partial class AppSettings : SettingsToRegistry<AppSettings>
{
    public bool IsServer { get; internal set; }

    #region ChifaFiles  
    public string ChifaPath { get; set; } = "D:\\CHIFA_OFFICINE";
    public string ChifaBackup => Path.Combine(ChifaPath, "Backup");
    public string ChifaPostgres => $"{ChifaPath}\\CHIFA_OFFICINE_DB\\bin\\postgres.exe";
    public string ChifaLancerServeur => $"{ChifaPath}\\CHIFA_OFFICINE_DB\\Lancer_Serveur.bat";
    public string ChifaBin => $"{ChifaPath}\\CHIFA_OFFICINE_DB\\bin\\";
    public string ChifaDump => $"{ChifaPath}\\CHIFA_OFFICINE_DB\\bin\\pg_dump.exe";
    public string ChifaRestore => $"{ChifaPath}\\CHIFA_OFFICINE_DB\\bin\\pg_restore.exe";
    #endregion

    #region Notifications

    [ObservableProperty] private bool _useChifaMobile;
    [ObservableProperty] private string? _chifaMobilEmail;
    [ObservableProperty] private string? _chifaMobilPassword;
    [ObservableProperty] private bool _startWithWin;

    #endregion

    #region CHIFA_PASS

    internal readonly string DbName = "CHIFA_OFFICINE";
    internal readonly string DbPassword = Environment.GetEnvironmentVariable("CHIFA_DB_PASSWORD") ?? "";
    internal readonly string DbUser = "pharm";

    #endregion

}
