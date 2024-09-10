using CHIFA.DAL.Helpers;

namespace CHIFA.DAL.Statistics;

public class WeeklyStat
{
    public string Date => $"{Month:D2} - {Year:D4}";
    public int? Month => DateDebut?.Month;
    public int? Year => DateDebut?.Year;
    public DateTime? DateDebut { get; init; }
    public DateTime? DateFin => DateDebut?.AddDays(6);
    public int Factures { get; init; }
    public decimal? MontantMaj { get;  init; }
    public decimal? MontantFact { get;      init; }
    public decimal? MontantOff { get;  init; }
    public decimal? MontantFE { get;  init; }
    public int Assureis { get; init; }
    public int Beneficiaires { get; init; }
    public int? Jours => (int)(DateFin?.Date - DateDebut?.Date)?.TotalDays + 1;

    public decimal? MontantJour => (MontantOff / Jours).ToDecimal();
    public decimal? FactureJour => (Factures / Jours).ToDecimal();
    public decimal? MontantFacture => (MontantOff / Factures).ToDecimal();
   
    public decimal? Marge => (MontantOff / 6).ToDecimal();
    public decimal? Brute => (Marge + MontantMaj).ToDecimal();

    public decimal? MontantGlobal => (MontantOff + MontantMaj + MontantFE).ToDecimal();

    public string Key { get; internal set; }
}
