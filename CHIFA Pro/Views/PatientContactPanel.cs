using CHIFA.Agent;
using CHIFA.Contract;
using Microsoft.Extensions.Logging;
using Serilog.Extensions.Logging;

namespace CHIFA.Pro.Views;

/// <summary>
/// The patient's phones, note and favourite star: the same contacts.json the phones edit through the agent,
/// kept on this computer and never in the CHIFA database.
/// </summary>
public class PatientContactPanel : GroupControl
{
    private static readonly ContactStore Store = new(new SerilogLoggerFactory(Serilog.Log.Logger).CreateLogger<PatientContactPanel>());

    private readonly TextEdit _phone1 = PhoneEdit();
    private readonly TextEdit _phone2 = PhoneEdit();
    private readonly MemoEdit _note = new() { Dock = DockStyle.Fill };
    private readonly CheckEdit _favori = new() { Text = "★ Favori", Width = 110 };
    private readonly SimpleButton _save = new() { Text = "Enregistrer", Width = 110, Enabled = false };
    private readonly LabelControl _status = new() { AutoSizeMode = LabelAutoSizeMode.Horizontal };

    private PatientContact? _contact;
    private bool _loading;

    public PatientContactPanel()
    {
        Text = "Contact du patient (enregistré sur cet ordinateur, visible sur les téléphones liés)";
        Dock = DockStyle.Bottom;
        Height = LogicalToDeviceUnits(150);
        Enabled = false;

        _note.Properties.MaxLength = PatientContact.MaxNoteLength;
        _note.Properties.NullValuePrompt = "Allergies, horaires, personne à contacter…";
        _favori.Properties.Appearance.ForeColor = Color.FromArgb(0xC0, 0x7A, 0x10);

        var table = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 8, RowCount = 2, Padding = new Padding(4, 2, 4, 0) };
        table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, LogicalToDeviceUnits(170)));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, LogicalToDeviceUnits(170)));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        table.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        table.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        table.Controls.Add(Caption("Téléphone 1"), 0, 0);
        table.Controls.Add(_phone1, 1, 0);
        table.Controls.Add(Caption("Téléphone 2"), 2, 0);
        table.Controls.Add(_phone2, 3, 0);
        table.Controls.Add(Caption("Note"), 4, 0);
        table.Controls.Add(_note, 5, 0);
        table.Controls.Add(_favori, 6, 0);
        table.Controls.Add(_save, 7, 0);
        table.Controls.Add(_status, 0, 1);
        table.SetColumnSpan(_status, 8);
        Controls.Add(table);

        _phone1.EditValueChanged += (_, _) => OnEdited(_phone1);
        _phone2.EditValueChanged += (_, _) => OnEdited(_phone2);
        _note.EditValueChanged += (_, _) => OnEdited(null);
        _favori.CheckedChanged += async (_, _) => await SaveFavoriAsync();
        _save.Click += async (_, _) => await SaveAsync();
    }

    /// <summary>The beneficiary's name, kept with the contact so the phones can list favourites.</summary>
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public string? Nom { get; set; }

    public async Task LoadAsync(string numAssure, string rang)
    {
        try
        {
            Display(await Store.GetAsync(new PatientKey(numAssure, rang)));
            Enabled = true;
        }
        catch (ContactRejectedException ex)
        {
            ShowStatus(ex.Message, error: true);
        }
        catch (Exception ex)
        {
            ex.Log();
            ShowStatus("Impossible de lire les contacts de cet ordinateur.", error: true);
        }
    }

    private static TextEdit PhoneEdit()
    {
        var edit = new TextEdit { Dock = DockStyle.Fill };
        edit.Properties.NullValuePrompt = "06 61 23 45 67";
        return edit;
    }

    private static LabelControl Caption(string text) => new() { Text = text, Padding = new Padding(6, 5, 2, 0) };

    private void OnEdited(TextEdit? phone)
    {
        if (_loading)
            return;
        if (phone is not null)
            phone.ErrorText = string.IsNullOrWhiteSpace(phone.Text) || AlgerianPhone.IsValid(phone.Text)
                ? ""
                : "Numéro algérien attendu : 05, 06, 07 ou un fixe 0xx.";
        _save.Enabled = true;
        ShowStatus("");
    }

    private Task SaveAsync() =>
        StoreAsync(_contact! with { Nom = Nom, Telephone1 = _phone1.Text, Telephone2 = _phone2.Text, Note = _note.Text });

    // Only the star: phones being typed are saved with « Enregistrer ».
    private Task SaveFavoriAsync() =>
        _loading || _contact is null ? Task.CompletedTask : StoreAsync(_contact with { Nom = Nom, Favori = _favori.Checked }, keepEdits: true);

    private async Task StoreAsync(PatientContact contact, bool keepEdits = false)
    {
        if (contact.Problem() is { } problem)
        {
            ShowStatus(problem, error: true);
            return;
        }

        try
        {
            var saved = await Store.SaveAsync(contact);
            if (keepEdits)
                _contact = saved;
            else
                Display(saved);
            ShowStatus($"Enregistré à {DateTime.Now:HH:mm}.");
        }
        catch (ContactRejectedException ex)
        {
            ShowStatus(ex.Message, error: true);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            ex.Log();
            ShowStatus("Impossible d'enregistrer le contact sur cet ordinateur.", error: true);
        }
    }

    private void Display(PatientContact contact)
    {
        _loading = true;
        _contact = contact;
        _phone1.Text = contact.Telephone1 is { } t1 ? AlgerianPhone.Format(t1) : "";
        _phone2.Text = contact.Telephone2 is { } t2 ? AlgerianPhone.Format(t2) : "";
        _phone1.ErrorText = _phone2.ErrorText = "";
        _note.Text = contact.Note ?? "";
        _favori.Checked = contact.Favori;
        _save.Enabled = false;
        _loading = false;
    }

    private void ShowStatus(string text, bool error = false)
    {
        _status.Text = text;
        _status.Appearance.ForeColor = error ? Color.Firebrick : Color.SeaGreen;
    }
}
