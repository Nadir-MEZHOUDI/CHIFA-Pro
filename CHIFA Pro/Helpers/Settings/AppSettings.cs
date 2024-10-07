namespace CHIFA.Pro.Helpers.Settings;

public class AppSettings : SettingsToRegistry<AppSettings>
{
    public string ChifaPath { get; set; } = "D:\\CHIFA_OFFICINE";
    public bool? ServerAutoStart { get; internal set; }


    #region ChifaFiles  
    public string ChifaBackup => Path.Combine(ChifaPath, "Backup");
    public string ChifaPostgres => $"{ChifaPath}\\CHIFA_OFFICINE_DB\\bin\\postgres.exe";
    public string ChifaLancerServeur => $"{ChifaPath}\\CHIFA_OFFICINE_DB\\Lancer_Serveur.bat";
    public string ChifaBin => $"{ChifaPath}\\CHIFA_OFFICINE_DB\\bin\\";
    public string ChifaDump => $"{ChifaPath}\\CHIFA_OFFICINE_DB\\bin\\pg_dump.exe";
    public string ChifaRestore => $"{ChifaPath}\\CHIFA_OFFICINE_DB\\bin\\pg_restore.exe";

    public bool IsServer { get; internal set; }
    public bool NotificationOnDays { get; set; }
    public bool NotificationOnMontant { get; set; } 
    public bool NotificationOnNmbr { get; set; }

    public int MaxDays { get; set; }  = 7;
    public int MaxMontant { get; set; }= 500_000;
    public int MaxNmbr { get; set; } = 100;


    internal readonly string DbName = "CHIFA_OFFICINE";
    internal readonly string DbPassword = "REDACTED";
    internal readonly string DbUser = "pharm";

    #endregion
}
