using CHIFA.Services.Dtos;
using FluentAssertions;
using Xunit;

namespace CHIFA.Tests.Services;

public class ControleMedicalDtoTests
{
    [Theory]
    [InlineData('C', 0, CmStatut.EnAttente, "Envoyée — en attente")]
    [InlineData('T', 1, CmStatut.Accordee, "Accordée — à signer")]
    [InlineData('T', 2, CmStatut.RejetPartiel, "Rejet partiel — à signer")]
    [InlineData('T', 3, CmStatut.RejetGlobal, "Rejet global")]
    [InlineData('T', 4, CmStatut.Convoque, "Malade convoqué au CM")]
    [InlineData('S', 1, CmStatut.Signee, "Signée — accordée")]
    [InlineData('S', 2, CmStatut.RejetPartiel, "Signée — rejet partiel")]
    public void Decode_FollowsChifaOfficine(char etat, int decision, CmStatut statut, string label)
    {
        ControleMedicalDto.Decode(etat, decision).Should().Be(statut);
        ControleMedicalDto.Label(etat, decision).Should().Be(label);
    }

    [Theory]
    [InlineData(0, 2, "Rejeté")]
    [InlineData(1, 1, "Partiel")]
    [InlineData(2, 0, "Accordé")]
    [InlineData(0, 0, "—")]
    public void LigneDecision_ComesFromTheQuantities(int accord, int rejet, string decision)
        => new ControleMedicalLigneDto { Qte = 2, QteAccord = accord, QteRej = rejet }.Decision.Should().Be(decision);
}
