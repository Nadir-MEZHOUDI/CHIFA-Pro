namespace CHIFA.DAL.Statistics;

public class BordMonthlyStat
{
    public string Date => $"{Month:D2} - {Year:D4}";
    public int? Month { get;  set; }
    public int? Year { get;  set; }
    public DateTime? DateDebut { get; set; }
    public DateTime? DateFin { get; set; }
    public string Num { get; set; } = "";
    public int Factures { get; set; } = 0;
    public int? Jours => (int)(DateFin - DateDebut)?.TotalDays + 1;
    public decimal? MontantJour => MontantOff / Jours;
    public decimal? FactureJour => Factures / Jours;
    public decimal? MontantFacture => MontantOff / Factures;
    public decimal? MontantMaj { get;  set; }
    public decimal? MontantFact { get;  set; }
    public decimal? MontantOff { get;  set; }
    public decimal? Marge => MontantOff / 6;
    public decimal? Brute => Marge + MontantMaj;
    public int Bordereaux { get; set; }
    public decimal? Virement { get; set; }
    public decimal? EcartJour => Ecart / Jours;
    public decimal? EcartFacture => Ecart / Factures;
    public decimal? Net => Brute - Ecart;
    public decimal? Ecart => MontantOff - Virement;

}