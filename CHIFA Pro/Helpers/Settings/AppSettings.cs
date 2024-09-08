using SmartPharm.Helpers.Settings;

namespace Helpers.Settings;

public class AppSettings : SettingsToRegistry<AppSettings>
{
    public string ChifaPath { get; set; } = "D:\\CHIFA_OFFICINE";
    public bool? ServerAutoStart { get; internal set; } = null;
 

    #region ChifaFiles  
    public string ChifaBackup => Path.Combine(ChifaPath, "Backup");
    public string ChifaPostgres => $"{ChifaPath}\\CHIFA_OFFICINE_DB\\bin\\postgres.exe";
    public string ChifaLancer_Serveur => $"{ChifaPath}\\CHIFA_OFFICINE_DB\\Lancer_Serveur.bat";
    public string ChifaBin => $"{ChifaPath}\\CHIFA_OFFICINE_DB\\bin\\";
    public string ChifaDump => $"{ChifaPath}\\CHIFA_OFFICINE_DB\\bin\\pg_dump.exe";
    public string ChifaRestor => $"{ChifaPath}\\CHIFA_OFFICINE_DB\\bin\\pg_restore.exe";

    internal readonly string DbName = "CHIFA_OFFICINE";
    internal readonly string DbPassword = "REDACTED";
    internal readonly string DbUser = "pharm";

    #endregion
}
