namespace CHIFA.DAL.Statistics;

public class MonthlyStat
{
    public string Date => $"{Month:D2} - {Year:D4}";
    public int? Month { get; internal set; }
    public int? Year { get;  set; }
    public DateTime? DateDebut { get; set; }
    public DateTime? DateFin { get; set; }
    public int Factures { get; set; } = 0;
    public int? Jours => (int)(DateFin?.Date - DateDebut?.Date)?.TotalDays + 1;
    public decimal? MontantJour => MontantOff / Jours;
    public decimal? FactureJour => Factures / Jours;
    public decimal? MontantFacture => MontantOff / Factures;
    public decimal? MontantMaj { get;  set; }
    public decimal? MontantFact { get;  set; }
    public decimal? MontantOff { get;  set; }
    public decimal? Marge => MontantOff / 6;
    public decimal? Brute => Marge + MontantMaj;
}
