using CHIFA.Services.Dtos;

namespace CHIFA.Tests.Dtos;

public class BordereauDtoTests
{
    [Fact]
    public void DepotFtp_WhenDateIsSet_ReturnsFormattedString()
    {
        var dto = new BordereauDto
        {
            DateDepotFtp = new DateTime(2026, 6, 10, 10, 14, 54)
        };

        dto.DepotFtp.Should().Be("06-10-2026 10:14");
    }

    [Fact]
    public void DepotFtp_WhenDateIsNull_ReturnsEmpty()
    {
        var dto = new BordereauDto { DateDepotFtp = null };

        dto.DepotFtp.Should().BeEmpty();
    }

    [Fact]
    public void DepotFtp_WhenYearIsBefore2001_ReturnsEmpty()
    {
        var dto = new BordereauDto { DateDepotFtp = new DateTime(2000, 12, 31) };

        dto.DepotFtp.Should().BeEmpty();
    }

    [Fact]
    public void DepotFtp_FormatIsExactlyMMddyyyyHHmm()
    {
        var dto = new BordereauDto
        {
            DateDepotFtp = new DateTime(2026, 1, 5, 9, 5, 30)
        };

        dto.DepotFtp.Should().Be("01-05-2026 09:05");
        dto.DepotFtp.Length.Should().Be(16);
    }

    [Fact]
    public void MFAE_IsNmbrTimesFive()
    {
        var dto = new BordereauDto { Nmbr = 19 };

        dto.MFAE.Should().Be(95);
    }

    [Fact]
    public void MFAE_WhenNmbrIsNull_ReturnsNull()
    {
        var dto = new BordereauDto { Nmbr = null };

        dto.MFAE.Should().BeNull();
    }

    [Fact]
    public void Etat_Closed_Returns2()
    {
        var dto = new BordereauDto { State = 'C' };

        dto.Etat.Should().Be(2);
    }

    [Fact]
    public void Etat_Opened_Returns1()
    {
        var dto = new BordereauDto { State = 'O' };

        dto.Etat.Should().Be(1);
    }

    [Fact]
    public void Etat_Other_Returns0()
    {
        var dto = new BordereauDto { State = 'X' };

        dto.Etat.Should().Be(0);
    }

    [Fact]
    public void MontGlobal_SumsMontOffMajAndMFAE()
    {
        var dto = new BordereauDto
        {
            MontOff = 1000m,
            Maj = 200m,
            Nmbr = 10
        };

        dto.MontGlobal.Should().Be(1000m + 200m + 50m);
    }
    
}
