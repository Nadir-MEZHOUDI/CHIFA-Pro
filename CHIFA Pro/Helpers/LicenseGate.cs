namespace CHIFA.Pro.Helpers;

/// <summary>
/// Serial-number licensing through SmartApp.Licensing (https://license.smartappdz.org).
/// The license is checked offline at every start (signed file + public key); the app checks in
/// online in the background to pick up renewals and revocations.
/// <para>
/// DEBUG builds skip licensing unless <c>CHIFA_LICENSING=1</c>. Then <c>CHIFA_LICENSE_ENDPOINT</c> and
/// <c>CHIFA_LICENSE_PUBLIC_KEY_FILE</c> can point to a local licensing server for testing.
/// </para>
/// </summary>
internal static class LicenseGate
{
    public const string Product = "chifa-pro";
    public const int ExpiryWarningDays = 15;

    /// <summary>The trial lasts 7 days and its end date is in the window title, so it only warns at the very end.</summary>
    public const int TrialWarningDays = 2;

    private const string ProductionEndpoint = "https://license.smartappdz.org";

    // Public key of the SmartApp licensing server. It only verifies licenses; it cannot create them.
    private const string ProductionPublicKey = """
        -----BEGIN PUBLIC KEY-----
        MFkwEwYHKoZIzj0CAQYIKoZIzj0DAQcDQgAEPckUvTCNoSzE+EQrTlAb9qaxnTiA
        l9GJ/e8oSV/rMmErXqmPAiyeKemgKYDL7+BV6o3umXv6GJJxn0yyGPyVQg==
        -----END PUBLIC KEY-----
        """;

    private static LicenseClient? _client;
    private static string? _storageDirectory;

    /// <summary>False in DEBUG builds unless licensing was turned on.</summary>
    public static bool IsEnabled => _client is not null;

    public static LicenseState? State => _client?.Current;

    /// <summary>Raised (on a background thread) after an online check-in changed the state.</summary>
    public static event Action? Changed;

    /// <summary>
    /// Verifies the license before the main window opens. Shows the activation dialog when needed.
    /// Returns <c>false</c> when the application must exit.
    /// </summary>
    public static bool EnsureLicensed()
    {
        var settings = ResolveSettings();
        if (settings is null)
        {
            Log.Information("Licensing disabled for this DEBUG build (set CHIFA_LICENSING=1 to enable).");
            return true;
        }

        _storageDirectory = settings.StorageDirectory ?? Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SmartApp", "Licensing", Product);

        try
        {
            _client = LicenseClientFactory.Create(options =>
            {
                options.Endpoint = settings.Endpoint;
                options.Product = Product;
                options.PublicKeyPem = settings.PublicKeyPem;
                options.AppVersion = Application.ProductVersion.Split("+")[0];
                options.StorageDirectory = settings.StorageDirectory;
            });
        }
        catch (Exception ex)
        {
            ex.Log();
            XtraMessageBox.Show("La vérification de la licence est impossible sur ce poste.\n" + ex.Message,
                "Licence SmartCHIFA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        var state = _client.Check();

        // Expired or offline for too long: maybe it was renewed meanwhile, ask the server once.
        if (!state.IsValid && state.License is not null)
            state = RefreshNow();

        // A license request was sent from this computer: if SmartApp approved it, the license installs now
        // (it also replaces a running trial).
        if ((!state.IsValid || state.IsTrial) && _client.PendingRequest is not null)
            state = CheckRequestNow();

        if (!state.IsValid)
        {
            using var form = new LicenseForm(_client, startup: true);
            if (form.ShowDialog() != DialogResult.OK)
                return false;
            state = _client.Current;
        }
        else
        {
            _ = RefreshInBackgroundAsync();
        }

        WarnIfExpiringSoon(state);
        return true;
    }

    /// <summary>
    /// The signed license file of this computer, or <c>null</c> when it is not licensed. The SmartCHIFA
    /// server checks its signature to know which pharmacy links phones.
    /// </summary>
    public static string? LicenseFile()
    {
        if (_storageDirectory is null || State is not { IsValid: true }) return null;
        try
        {
            var path = Path.Combine(_storageDirectory, "license.lic");
            return File.Exists(path) ? File.ReadAllText(path) : null;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            return null;
        }
    }

    /// <summary>Opens the license dialog from the main window (details and online check).</summary>
    public static void ShowDialog(IWin32Window owner)
    {
        if (_client is null) return;
        using var form = new LicenseForm(_client, startup: false);
        form.ShowDialog(owner);
    }

    /// <summary>Called on the UI thread after a background check-in. Blocks the app if the license was lost.</summary>
    public static void HandleBackgroundResult(Form mainForm)
    {
        if (_client is null || _client.Current.IsValid) return;

        using var form = new LicenseForm(_client, startup: true);
        if (form.ShowDialog(mainForm) != DialogResult.OK)
            mainForm.Close();
    }

    /// <summary>Short text for the window title, e.g. "Pharmacie El Amel · jusqu'au 22/11/2027".</summary>
    public static string? TitleText()
    {
        var license = State is { IsValid: true } s ? s.License : null;
        if (license is null) return null;
        var until = license.ExpiresAt is { } end ? $" · jusqu'au {end.LocalDateTime:dd/MM/yyyy}" : string.Empty;
        return State!.IsTrial ? $"Version d'essai{until}" : $"{license.Customer}{until}";
    }

    public static void Dispose()
    {
        _client?.Dispose();
        _client = null;
    }

    private static LicenseState RefreshNow()
    {
        try
        {
            return Task.Run(() => _client!.RefreshAsync(force: true))
                .WaitAsync(TimeSpan.FromSeconds(20))
                .GetAwaiter()
                .GetResult();
        }
        catch (Exception ex) when (ex is TimeoutException or TaskCanceledException)
        {
            return _client!.Current;
        }
    }

    private static LicenseState CheckRequestNow()
    {
        try
        {
            var result = Task.Run(() => _client!.CheckRequestAsync())
                .WaitAsync(TimeSpan.FromSeconds(20))
                .GetAwaiter()
                .GetResult();
            if (result.Outcome == LicenseRequestOutcome.Approved)
                Program.Telemetry?.TrackFeatureUsed("license.request.approved");
            return result.State;
        }
        catch (Exception ex) when (ex is TimeoutException or TaskCanceledException)
        {
            return _client!.Current;
        }
    }

    private static async Task RefreshInBackgroundAsync()
    {
        try
        {
            var before = _client!.Current;
            var after = await Task.Run(() => _client.RefreshAsync()).ConfigureAwait(false);
            if (after.Status != before.Status || after.License?.ExpiresAt != before.License?.ExpiresAt)
                Changed?.Invoke();
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    private static void WarnIfExpiringSoon(LicenseState state)
    {
        var days = state.DaysRemaining(DateTimeOffset.UtcNow);
        if (!state.IsValid || days is null || days > (state.IsTrial ? TrialWarningDays : ExpiryWarningDays)) return;

        if (state.IsTrial)
        {
            XtraMessageBox.Show(
                $"Version d'essai gratuite : il reste {Math.Max(0, days.Value)} jour(s), jusqu'au " +
                $"{state.License!.ExpiresAt!.Value.LocalDateTime:dd/MM/yyyy à HH:mm}.\n\n" +
                "Pour continuer ensuite, ouvrez le menu « Licence » et demandez une licence : " +
                "SmartApp vous appelle pour le paiement.",
                "Essai gratuit SmartCHIFA", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        XtraMessageBox.Show(
            $"Votre abonnement SmartCHIFA se termine le {state.License!.ExpiresAt!.Value.LocalDateTime:dd/MM/yyyy} " +
            $"(dans {Math.Max(0, days.Value)} jour(s)).\n\nContactez SmartApp pour le renouveler : " +
            "votre numéro de série reste le même.",
            "Renouvellement de l'abonnement", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private sealed record Settings(string Endpoint, string PublicKeyPem, string? StorageDirectory);

    private static Settings? ResolveSettings()
    {
#if DEBUG
        if (Environment.GetEnvironmentVariable("CHIFA_LICENSING") != "1")
            return null;

        var endpoint = Environment.GetEnvironmentVariable("CHIFA_LICENSE_ENDPOINT");
        var keyFile = Environment.GetEnvironmentVariable("CHIFA_LICENSE_PUBLIC_KEY_FILE");
        if (string.IsNullOrWhiteSpace(endpoint))
            return new Settings(ProductionEndpoint, ProductionPublicKey, null);

        // A test server: keep its license apart from the real one.
        var key = !string.IsNullOrWhiteSpace(keyFile) && File.Exists(keyFile) ? File.ReadAllText(keyFile) : ProductionPublicKey;
        var storage = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "SmartApp", "Licensing", Product + "-test");
        return new Settings(endpoint, key, storage);
#else
        return new Settings(ProductionEndpoint, ProductionPublicKey, null);
#endif
    }
}

/// <summary>French messages for the license states and server errors.</summary>
internal static class LicenseText
{
    public static string Describe(LicenseState state)
    {
        ArgumentNullException.ThrowIfNull(state);
        if (state.IsTrial && state.License?.ExpiresAt is { } trialEnd)
        {
            if (state.Status == LicenseStatus.Valid)
                return $"Version d'essai gratuite jusqu'au {trialEnd.LocalDateTime:dd/MM/yyyy à HH:mm}.";
            if (state.Status == LicenseStatus.Expired)
                return $"Votre essai gratuit s'est terminé le {trialEnd.LocalDateTime:dd/MM/yyyy}. " +
                       "Demandez une licence pour continuer à utiliser SmartCHIFA.";
        }

        var fromError = DescribeError(state.ErrorCode);
        if (fromError is not null) return fromError;

        var end = state.License?.ExpiresAt is { } e ? e.LocalDateTime.ToString("dd/MM/yyyy", CultureInfo.CurrentCulture) : null;
        return state.Status switch
        {
            LicenseStatus.Valid => end is null ? "Licence valide." : $"Licence valide jusqu'au {end}.",
            LicenseStatus.NotActivated => "Ce poste n'est pas encore activé. Commencez l'essai gratuit de 7 jours, demandez une licence à SmartApp, ou saisissez le numéro de série fourni.",
            LicenseStatus.Expired => $"Votre abonnement a pris fin le {end}. Après renouvellement, cliquez sur « Vérifier en ligne ».",
            LicenseStatus.RefreshRequired => "La licence doit être vérifiée en ligne. Connectez ce poste à Internet puis cliquez sur « Vérifier en ligne ».",
            LicenseStatus.Revoked => "Ce numéro de série a été révoqué. Contactez SmartApp.",
            LicenseStatus.ClockTampered => "La date de cet ordinateur est incorrecte. Corrigez la date et l'heure puis relancez SmartCHIFA.",
            LicenseStatus.WrongMachine => "Cette licence appartient à un autre ordinateur. Ce poste nécessite son propre numéro de série.",
            LicenseStatus.WrongProduct or LicenseStatus.Invalid => "Le fichier de licence est invalide. Activez ce poste avec votre numéro de série.",
            _ => state.Message
        };
    }

    /// <summary>French text for a licensing server error code, or <c>null</c> when there is none.</summary>
    public static string? DescribeError(string? code) =>
        code switch
        {
            "invalid_serial" => "Numéro de série invalide. Vérifiez la saisie (5 groupes de 5 caractères).",
            "not_found" => "Ce numéro de série n'existe pas. Vérifiez la saisie.",
            "activation_limit_reached" => "Ce numéro de série est déjà activé sur un autre ordinateur. " +
                                          "Chaque poste nécessite son propre numéro de série : contactez SmartApp.",
            "revoked" => "Ce numéro de série a été révoqué. Contactez SmartApp.",
            "expired" => "Votre abonnement est terminé. Renouvelez-le auprès de SmartApp puis cliquez sur « Vérifier en ligne ».",
            "not_started" => "Votre abonnement n'a pas encore commencé.",
            "not_activated" => "Ce poste n'est plus activé. Saisissez à nouveau votre numéro de série.",
            "offline" => "Impossible de joindre le serveur de licences. Vérifiez la connexion Internet puis réessayez.",
            "unknown_product" => "Cette application n'est pas disponible à l'activation. Contactez SmartApp.",
            "invalid_request" => "Demande incomplète : le nom et le téléphone sont obligatoires.",
            "request_not_found" => "Votre demande n'existe plus. Envoyez une nouvelle demande.",
            "trial_used" => "L'essai gratuit a déjà été utilisé sur cet ordinateur. Demandez une licence pour continuer.",
            "trial_unavailable" => "L'essai gratuit n'est pas disponible pour le moment. Demandez une licence.",
            null => null,
            _ => "Le serveur de licences a répondu par une erreur. Réessayez plus tard."
        };
}
