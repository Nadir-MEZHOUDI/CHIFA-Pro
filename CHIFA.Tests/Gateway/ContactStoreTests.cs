using CHIFA.Agent;
using CHIFA.Contract;
using Microsoft.Extensions.Logging.Abstractions;

namespace CHIFA.Tests.Gateway;

public sealed class ContactStoreTests : IDisposable
{
    private readonly string _folder = Path.Combine(Path.GetTempPath(), "chifa-contacts-" + Guid.NewGuid().ToString("N"));

    private ContactStore NewStore() => new(NullLogger.Instance, Path.Combine(_folder, "contacts.json"));

    public void Dispose()
    {
        if (Directory.Exists(_folder))
            Directory.Delete(_folder, recursive: true);
    }

    [Fact]
    public async Task Unknown_patient_gets_an_empty_contact_and_no_file_is_needed()
    {
        var contact = await NewStore().GetAsync(new PatientKey("123", "00"));

        contact.Should().Be(new PatientContact("123", "00"));
        contact.IsEmpty.Should().BeTrue();
    }

    [Fact]
    public async Task Saved_contact_is_read_back_by_another_instance_with_normalised_phones()
    {
        await NewStore().SaveAsync(new PatientContact(" 123 ", "01", " BENALI Ahmed ", "+213 661 23 45 67", "021.23.45.67", "  Allergique à la pénicilline ", true));

        var read = await NewStore().GetAsync(new PatientKey("123", "01"));

        read.Should().Be(new PatientContact("123", "01", "BENALI Ahmed", "0661234567", "021234567", "Allergique à la pénicilline", true));
    }

    [Fact]
    public async Task Saving_again_replaces_the_contact_and_keeps_the_others()
    {
        var store = NewStore();
        await store.SaveAsync(new PatientContact("1", "00", "A", "0550000001"));
        await store.SaveAsync(new PatientContact("2", "00", "B", "0770000002", Favori: true));
        await store.SaveAsync(new PatientContact("1", "00", "A", "0660000003"));

        var all = await store.LoadAllAsync();

        all.Should().BeEquivalentTo([
            new PatientContact("1", "00", "A", "0660000003"),
            new PatientContact("2", "00", "B", "0770000002", Favori: true)
        ]);
    }

    [Fact]
    public async Task Clearing_every_field_removes_the_contact_from_the_file()
    {
        var store = NewStore();
        await store.SaveAsync(new PatientContact("1", "00", "A", "0550000001", Note: "x", Favori: true));

        var saved = await store.SaveAsync(new PatientContact("1", "00", "A", "  ", null, " "));

        saved.IsEmpty.Should().BeTrue();
        (await store.LoadAllAsync()).Should().BeEmpty();
    }

    [Fact]
    public async Task Lone_second_phone_moves_up_and_a_repeated_number_is_kept_once()
    {
        var store = NewStore();

        var moved = await store.SaveAsync(new PatientContact("1", "00", Telephone2: "0661234567"));
        var deduped = await store.SaveAsync(new PatientContact("2", "00", Telephone1: "0661234567", Telephone2: "+213661234567"));

        moved.Should().Be(new PatientContact("1", "00", Telephone1: "0661234567"));
        deduped.Should().Be(new PatientContact("2", "00", Telephone1: "0661234567"));
    }

    [Theory]
    [InlineData("0812345678", null)]
    [InlineData("0661234567", "123")]
    public async Task Invalid_phone_is_refused_with_a_french_message(string phone1, string? phone2)
    {
        var store = NewStore();

        var save = () => store.SaveAsync(new PatientContact("1", "00", Telephone1: phone1, Telephone2: phone2));

        (await save.Should().ThrowAsync<ContactRejectedException>()).Which.Message.Should().Contain("invalide");
        File.Exists(store.FilePath).Should().BeFalse();
    }

    [Fact]
    public async Task Too_long_a_note_is_refused()
    {
        var save = () => NewStore().SaveAsync(new PatientContact("1", "00", Note: new string('x', PatientContact.MaxNoteLength + 1)));

        await save.Should().ThrowAsync<ContactRejectedException>();
    }

    [Fact]
    public async Task Unreadable_file_is_never_overwritten()
    {
        var store = NewStore();
        Directory.CreateDirectory(_folder);
        await File.WriteAllTextAsync(store.FilePath, "{ not json");

        var save = () => store.SaveAsync(new PatientContact("1", "00", Telephone1: "0661234567"));

        await save.Should().ThrowAsync<ContactRejectedException>();
        (await File.ReadAllTextAsync(store.FilePath)).Should().Be("{ not json");
    }
}

public class AlgerianPhoneTests
{
    [Theory]
    [InlineData("0661234567", "0661234567")]
    [InlineData("0550 12 34 56", "0550123456")]
    [InlineData("07.70.12.34.56", "0770123456")]
    [InlineData("+213661234567", "0661234567")]
    [InlineData("+213 (0) 661 23 45 67", "0661234567")]
    [InlineData("00213 661 23 45 67", "0661234567")]
    [InlineData("213661234567", "0661234567")]
    [InlineData("661234567", "0661234567")]
    [InlineData("021 23 45 67", "021234567")]
    [InlineData("031-92-00-00", "031920000")]
    [InlineData("+213 38 86 12 34", "038861234")]
    public void Valid_numbers_are_normalised_to_national_digits(string input, string expected)
    {
        AlgerianPhone.Normalize(input).Should().Be(expected);
        AlgerianPhone.IsValid(input).Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("066123456")]       // one digit short
    [InlineData("06612345678")]     // one digit too many
    [InlineData("0861234567")]      // 08 is not a mobile prefix
    [InlineData("0123456789")]
    [InlineData("051234567")]       // 05 + 7 digits is neither mobile nor landline
    [InlineData("+33 6 12 34 56 78")]
    [InlineData("0661 23 45 6a")]
    [InlineData("06+61234567")]
    public void Invalid_numbers_are_refused(string? input)
    {
        AlgerianPhone.Normalize(input).Should().BeNull();
        AlgerianPhone.IsValid(input).Should().BeFalse();
    }

    [Theory]
    [InlineData("0661234567", "0661 23 45 67")]
    [InlineData("+213 21 23 45 67", "021 23 45 67")]
    [InlineData("pas un numéro", "pas un numéro")]
    public void Format_groups_the_digits(string input, string expected) =>
        AlgerianPhone.Format(input).Should().Be(expected);

    [Fact]
    public void International_form_drops_the_trunk_zero() =>
        AlgerianPhone.ToInternational("0661 23 45 67").Should().Be("+213661234567");

    [Fact]
    public void Contact_problem_names_the_wrong_phone()
    {
        new PatientContact("1", "00", Telephone1: "0661234567").Problem().Should().BeNull();
        new PatientContact("1", "00", Telephone2: "12").Problem().Should().StartWith("Téléphone 2");
        new PatientContact("", "00").Problem().Should().NotBeNull();
    }
}
