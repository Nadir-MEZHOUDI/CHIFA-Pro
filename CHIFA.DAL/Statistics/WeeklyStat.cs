namespace CHIFA.DAL.Statistics;

public class WeeklyStat
{
    public string Date => $"{Month:D2} - {Year:D4}";
    public int? Month => DateDebut?.Month;
    public int? Year => DateDebut?.Year;
    public DateTime? DateDebut { get; set; }
    public DateTime? DateFin => DateDebut?.AddDays(6);
    public int Factures { get; set; } = 0; 
    public decimal? MontantMaj { get;  set; }
    public decimal? MontantFact { get;  set; }
    public decimal? MontantOff { get;  set; }
    public decimal? MontantFE { get;  set; }
    public int? Jours => (int)(DateFin?.Date - DateDebut?.Date)?.TotalDays + 1;

    public decimal? MontantJour => (MontantOff / Jours).ToDecimal();
    public decimal? FactureJour => (Factures / Jours).ToDecimal();
    public decimal? MontantFacture => (MontantOff / Factures).ToDecimal();
   
    public decimal? Marge => (MontantOff / 6).ToDecimal();
    public decimal? Brute => (Marge + MontantMaj).ToDecimal();

    public decimal? MontantGlobal => (MontantOff + MontantMaj + MontantFE).ToDecimal();
}
