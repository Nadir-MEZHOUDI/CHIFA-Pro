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
        var pharmacyId = PharmacyId(_storageDirectory);

        try
        {
            _client = CreateClient(settings, pharmacyId);
        }
        catch (Exception ex)
        {
            ex.Log();
            XtraMessageBox.Show("La vérification de la licence est impossible sur ce poste.\n" + ex.Message,
                "Licence SmartCHIFA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        var state = _client.Check();

        // A license activated before the serial followed the code PS is bound to this Windows: move it to the pharmacy.
        if (state.Status == LicenseStatus.WrongMachine && pharmacyId is not null)
            state = MoveToPharmacy(settings);

        // Expired or offline for too long: maybe it was renewed meanwhile, ask the server once.
        if (!state.IsValid && state.License is not null)
            state = RefreshNow();

        // A license request was sent from this computer: if SmartApp approved it, the license installs now
        // (it also replaces a running trial).
        if ((!state.IsValid || state.IsTrial) && _client.PendingRequest is not null)
            state = CheckRequestNow();

        // The same, for a license requested from a linked phone.
        if (!state.IsValid || state.IsTrial)
            state = InstallPhoneRequestNow() ?? state;

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

    /// <summary>
    /// Keeps what the pharmacist typed for the trial or a license request: the phone app sends the license request
    /// with these details in one tap.
    /// </summary>
    public static void SaveContact(LicenseRequestInfo info)
    {
        if (_storageDirectory is null) return;
        var contact = new CHIFA.Contract.PharmacistContact(info.FullName, info.Phone, info.BusinessName, info.Email, info.Wilaya, info.Address,
            info.CodePs, DateTime.UtcNow);
        try
        {
            File.WriteAllText(ContactFile, System.Text.Json.JsonSerializer.Serialize(contact));
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            ex.Log(showMessage: false);
        }
    }

    /// <summary>The details saved by <see cref="SaveContact"/>, or <c>null</c>.</summary>
    public static CHIFA.Contract.PharmacistContact? SavedContact()
    {
        if (_storageDirectory is null || !File.Exists(ContactFile)) return null;
        try
        {
            return System.Text.Json.JsonSerializer.Deserialize<CHIFA.Contract.PharmacistContact>(File.ReadAllText(ContactFile));
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or System.Text.Json.JsonException)
        {
            return null;
        }
    }

    private static string ContactFile => Path.Combine(_storageDirectory!, "contact.json");

    /// <summary>
    /// The pharmacy's details in the CHIFA database (Paramètres de l'officine), or <c>null</c> when it cannot be read.
    /// The pharmacist's name, address and code PS are taken from here and cannot be changed in a license request,
    /// on the computer or on a phone; the phone number, e-mail and pharmacy name only start from here.
    /// </summary>
    public static async Task<LicenseRequestInfo?> OfficineAsync()
    {
        try
        {
            var officine = await ChifaService.Instance.GetFirstOfficineAsync().AsTask().WaitAsync(TimeSpan.FromSeconds(8));
            if (officine is null) return null;
            var name = string.Join(" ", new[] { officine.Nom, officine.Prenom }.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x!.Trim()));
            return new LicenseRequestInfo(
                FullName: name,
                Phone: officine.NumTel?.Trim() ?? string.Empty,
                BusinessName: Clean(officine.NomPharmacie),
                Email: Clean(officine.Email),
                Address: Clean(officine.Adresse),
                CodePs: Clean(officine.CodePs));
        }
        catch (Exception ex)
        {
            // The database may not be reachable yet: the pharmacist types the details.
            ex.Log(showMessage: false);
            return null;
        }

        static string? Clean(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
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

    private static string AppVersion => Application.ProductVersion.Split("+")[0];

    /// <param name="pharmacyId"><c>null</c>: the license follows this Windows, as before the code PS.</param>
    private static LicenseClient CreateClient(Settings settings, string? pharmacyId) =>
        LicenseClientFactory.Create(options =>
        {
            options.Endpoint = settings.Endpoint;
            options.Product = Product;
            options.PublicKeyPem = settings.PublicKeyPem;
            options.AppVersion = AppVersion;
            options.StorageDirectory = settings.StorageDirectory;
            if (pharmacyId is not null)
                options.MachineIdProvider = () => pharmacyId;
        });

    /// <summary>
    /// The id the license is bound to, from the code PS of the CHIFA database: all the computers of the pharmacy share it
    /// and it survives a Windows reinstall. Kept in <c>pharmacy.id</c> for a start where the database does not answer.
    /// <c>null</c> when the database has no code PS.
    /// </summary>
    private static string? PharmacyId(string storageDirectory)
    {
        var cache = Path.Combine(storageDirectory, "pharmacy.id");
        try
        {
            var officine = Task.Run(OfficineAsync).GetAwaiter().GetResult();
            if (officine is null)
                return File.Exists(cache) && File.ReadAllText(cache).Trim() is { } cached && PharmacyLicenseId.IsPharmacy(cached) ? cached : null;

            var id = PharmacyLicenseId.FromCodePs(officine.CodePs);
            if (id is not null)
            {
                Directory.CreateDirectory(storageDirectory);
                File.WriteAllText(cache, id);
            }
            return id;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            ex.Log(showMessage: false);
            return null;
        }
    }

    /// <summary>
    /// Asks the licensing server, once, to move this Windows' license (or trial) to the pharmacy. Offline, the
    /// Windows license keeps working for this session and the move is tried again at the next start.
    /// </summary>
    private static LicenseState MoveToPharmacy(Settings settings)
    {
        var path = Path.Combine(_storageDirectory!, "license.lic");
        string? moved = null;
        try
        {
            var token = File.ReadAllText(path);
            moved = Task.Run(() => RebindAsync(settings.Endpoint, token, _client!.MachineId))
                .WaitAsync(TimeSpan.FromSeconds(20))
                .GetAwaiter()
                .GetResult();
            if (moved is not null)
            {
                File.WriteAllText(path + ".tmp", moved);
                File.Move(path + ".tmp", path, overwrite: true);
            }
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or HttpRequestException or TimeoutException or TaskCanceledException)
        {
            Log.Warning(ex, "The license could not move to the pharmacy's code PS.");
            moved = null;
        }

        if (moved is not null)
        {
            var state = _client!.Check();
            if (state.Status != LicenseStatus.WrongMachine)
            {
                Log.Information("The license now follows the pharmacy's code PS.");
                Program.Telemetry?.TrackFeatureUsed("license.moved_to_pharmacy");
                return state;
            }
        }

        var windows = CreateClient(settings, pharmacyId: null);
        var legacy = windows.Check();
        if (legacy.Status == LicenseStatus.WrongMachine)
        {
            // Neither this pharmacy's nor this Windows' license (copied from elsewhere): activate this pharmacy.
            windows.Dispose();
            return _client!.Current;
        }

        _client!.Dispose();
        _client = windows;
        return legacy;
    }

    private static async Task<string?> RebindAsync(string endpoint, string token, string pharmacyId)
    {
        using var http = new HttpClient { BaseAddress = new Uri(endpoint.TrimEnd('/') + "/"), Timeout = TimeSpan.FromSeconds(15) };
        using var response = await http.PostAsJsonAsync("api/v1/licenses/rebind",
            new RebindRequest(token, pharmacyId, Environment.MachineName, AppVersion)).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            Log.Warning("The licensing server refused to move the license to the pharmacy: {Status} {Body}",
                (int)response.StatusCode, await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            return null;
        }

        return (await response.Content.ReadFromJsonAsync<RebindResponse>().ConfigureAwait(false))?.License;
    }

    private sealed record RebindRequest(string License, string MachineId, string? MachineName, string? AppVersion);

    private sealed record RebindResponse(string? License);

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

    /// <summary>Activates the serial of a license requested from a phone and approved by SmartApp; <c>null</c> when there is none.</summary>
    private static LicenseState? InstallPhoneRequestNow()
    {
        try
        {
            return Task.Run(() => InstallPhoneRequestAsync(_client!))
                .WaitAsync(TimeSpan.FromSeconds(25))
                .GetAwaiter()
                .GetResult();
        }
        catch (Exception ex) when (ex is TimeoutException or TaskCanceledException)
        {
            return null;
        }
    }

    /// <summary>Also used by « Vérifier en ligne » in the license dialog.</summary>
    internal static async Task<LicenseState?> InstallPhoneRequestAsync(LicenseClient client)
    {
        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(20));
        if (await Mobile.MobileLink.ApprovedPhoneRequestSerialAsync(timeout.Token) is not { } serial)
            return null;

        LicenseState state;
        try
        {
            state = await client.ActivateAsync(serial, timeout.Token);
        }
        catch (Exception ex) when (ex is OperationCanceledException or HttpRequestException)
        {
            return null;
        }

        if (!state.IsValid)
        {
            Log.Warning("The license requested from a phone could not be activated: {Status} {Error}", state.Status, state.ErrorCode);
            return null;
        }

        Program.Telemetry?.TrackFeatureUsed("license.phone_request.installed");
        return state;
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
            LicenseStatus.WrongMachine => "Cette licence appartient à une autre pharmacie (autre code PS). Saisissez le numéro de série de cette pharmacie.",
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
            "activation_limit_reached" => "Ce numéro de série est déjà activé pour une autre pharmacie. " +
                                          "Chaque pharmacie (code PS) a son propre numéro de série : contactez SmartApp.",
            "revoked" => "Ce numéro de série a été révoqué. Contactez SmartApp.",
            "expired" => "Votre abonnement est terminé. Renouvelez-le auprès de SmartApp puis cliquez sur « Vérifier en ligne ».",
            "not_started" => "Votre abonnement n'a pas encore commencé.",
            "not_activated" => "Ce poste n'est plus activé. Saisissez à nouveau votre numéro de série.",
            "offline" => "Impossible de joindre le serveur de licences. Vérifiez la connexion Internet puis réessayez.",
            "unknown_product" => "Cette application n'est pas disponible à l'activation. Contactez SmartApp.",
            "invalid_request" => "Demande incomplète : le nom et le téléphone sont obligatoires.",
            "request_not_found" => "Votre demande n'existe plus. Envoyez une nouvelle demande.",
            "trial_used" => "L'essai gratuit a déjà été utilisé pour cette pharmacie. Demandez une licence pour continuer.",
            "trial_unavailable" => "L'essai gratuit n'est pas disponible pour le moment. Demandez une licence.",
            null => null,
            _ => "Le serveur de licences a répondu par une erreur. Réessayez plus tard."
        };
}
