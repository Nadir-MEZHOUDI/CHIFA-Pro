using CHIFA.DAL.Helpers;

namespace CHIFA.DAL.Statistics;

public class BordStatDto
{
    public DateTime? DateDebut { get; init; }
    public DateTime? DateFin { get; init; }
    public string Num { get; init; } = "";
    public int Factures { get; init; }
    public string? Center { get; init; }
    public decimal? MontantMaj { get; init; }
    public decimal? MontantFact { get; init; }
    public decimal? MontantOff { get; init; }
    public decimal? MontantFE { get; init; }
    public decimal? Virement { get; init; }
    public int Assuries { get; init; }
    public int Beneficiaires { get; init; }
    public DateOnly Date => DateOnly.FromDateTime(DateFin.Value);
    public int? Jours => (int)(DateFin - DateDebut)?.TotalDays + 1;
    public decimal? MontantJour =>  (MontantOff / Jours).ToDecimal();
    public decimal? FactureJour => (Factures / Jours).ToDecimal();
    public decimal? MontantFacture => ((MontantOff / Factures)).ToDecimal();
    public decimal? MontantGlobal => (MontantOff + MontantMaj + MontantFE).ToDecimal();
    public decimal? Marge => (MontantOff / 6).ToDecimal();
    public decimal? Brute => (Marge + MontantMaj).ToDecimal();
    public decimal? Net => (Brute - Ecart).ToDecimal();
    public decimal? Ecart => (Virement > 0 ? MontantGlobal - Virement : 0).ToDecimal();

}
