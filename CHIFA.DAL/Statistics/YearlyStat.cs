using CHIFA.DAL.Helpers;

namespace CHIFA.DAL.Statistics;

public class YearlyStat
{
    public string Date => $"{Year:D4}";
    public int? Year { get; init; }
    public DateTime? DateDebut { get; init; }
    public DateTime? DateFin { get; init; }
    public int Factures { get; init; }
    public decimal? MontantMaj { get; init; }
    public decimal? MontantFE { get; init; }
    public decimal? MontantFact { get; init; }
    public decimal? MontantOff { get; init; }
    public int? Jours => (int)(DateFin?.Date - DateDebut?.Date)?.TotalDays! + 1;
    public decimal? MontantJour => (MontantOff / Jours).ToDecimal();
    public decimal? FactureJour => (Factures / Jours).ToDecimal();
    public decimal? MontantFacture => (MontantOff / Factures).ToDecimal();
    public decimal? MontantGlobal => (MontantOff + MontantMaj + MontantFE).ToDecimal();

    public decimal? Marge => (MontantOff / 6).ToDecimal();
    public decimal? Brute => (Marge + MontantMaj).ToDecimal();

    public int Assureis { get;  set; }
    public int Beneficiaires { get;  set; }
}
