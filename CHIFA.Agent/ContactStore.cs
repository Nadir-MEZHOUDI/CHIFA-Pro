using System.Text.Json;
using CHIFA.Contract;

namespace CHIFA.Agent;

/// <summary>A contact the agent refuses to save; the message is in French and safe to show on the phone.</summary>
public class ContactRejectedException(string message) : Exception(message);

/// <summary>
/// The pharmacist's patient phone book, in contacts.json next to watchlist.json: on the pharmacy computer,
/// shared by the Windows service and SmartCHIFA, never written to the CHIFA database.
/// </summary>
public class ContactStore(ILogger logger, string? path = null)
{
    private readonly SemaphoreSlim _lock = new(1, 1);

    public static string DefaultPath => Path.Combine(SharedFile.Folder, "contacts.json");

    public string FilePath { get; } = path ?? DefaultPath;

    public async Task<List<PatientContact>> LoadAllAsync()
    {
        await _lock.WaitAsync();
        try
        {
            return await ReadAsync();
        }
        finally
        {
            _lock.Release();
        }
    }

    /// <returns>The saved contact, or an empty one for this patient.</returns>
    public async Task<PatientContact> GetAsync(PatientKey key)
    {
        var numAssure = key.NumAssure.Trim();
        var rang = key.Rang.Trim();
        return (await LoadAllAsync()).FirstOrDefault(c => c.NumAssure == numAssure && c.Rang == rang)
               ?? new PatientContact(numAssure, rang);
    }

    /// <returns>The contact as stored (normalised phones). An empty contact is removed from the file.</returns>
    public async Task<PatientContact> SaveAsync(PatientContact contact)
    {
        if (contact.Problem() is { } problem)
            throw new ContactRejectedException(problem);

        var clean = Normalize(contact);
        await _lock.WaitAsync();
        try
        {
            var all = await ReadAsync();
            var existing = all.RemoveAll(c => c.NumAssure == clean.NumAssure && c.Rang == clean.Rang) > 0;
            if (!clean.IsEmpty)
            {
                if (!existing && all.Count >= PatientContact.MaxContacts)
                    throw new ContactRejectedException($"Carnet plein : au plus {PatientContact.MaxContacts} patients.");
                all.Add(clean);
            }

            var sorted = all.OrderBy(c => c.Nom, StringComparer.OrdinalIgnoreCase).ThenBy(c => c.NumAssure).ThenBy(c => c.Rang);
            await SharedFile.WriteAsync(FilePath, JsonSerializer.Serialize(sorted), logger);
            return clean;
        }
        finally
        {
            _lock.Release();
        }
    }

    /// <summary>Trims everything, keeps national digits, and moves a lone second phone to the first.</summary>
    public static PatientContact Normalize(PatientContact c)
    {
        var phones = new[] { AlgerianPhone.Normalize(c.Telephone1), AlgerianPhone.Normalize(c.Telephone2) }
            .OfType<string>()
            .Distinct()
            .ToList();
        var note = c.Note?.Trim();
        return new PatientContact(
            c.NumAssure.Trim(),
            c.Rang.Trim(),
            string.IsNullOrWhiteSpace(c.Nom) ? null : c.Nom.Trim(),
            phones.ElementAtOrDefault(0),
            phones.ElementAtOrDefault(1),
            string.IsNullOrEmpty(note) ? null : note[..Math.Min(note.Length, PatientContact.MaxNoteLength)],
            c.Favori);
    }

    private async Task<List<PatientContact>> ReadAsync()
    {
        try
        {
            await using var stream = File.OpenRead(FilePath);
            return await JsonSerializer.DeserializeAsync<List<PatientContact>>(stream) ?? [];
        }
        catch (Exception ex) when (ex is FileNotFoundException or DirectoryNotFoundException)
        {
            return [];
        }
        catch (JsonException ex)
        {
            // Never overwrite a file we could not read: the pharmacist would lose every number in it.
            logger.LogError(ex, "Unreadable {Path}", FilePath);
            throw new ContactRejectedException("Le carnet de contacts de la pharmacie est illisible.");
        }
    }
}
