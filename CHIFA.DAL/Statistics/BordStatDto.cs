namespace CHIFA.DAL.Statistics;

public class BordStatDto
{
    public DateTime? DateDebut { get; set; }
    public DateTime? DateFin { get; set; }
    public DateOnly Date => DateOnly.FromDateTime(DateFin.Value);
    public string Num { get; set; } = "";
    public int Factures { get; set; } = 0;
    public string? Center { get; set; }
    public decimal? MontantMaj { get; set; }
    public decimal? MontantFact { get; set; }
    public decimal? MontantOff { get; set; }
    public decimal? MontantFE { get; set; }
    public decimal? Virement { get; set; }
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

public static class BordStatDtoExtensions
{
    public static decimal ToDecimal(this decimal? value) => Math.Round ((value ?? 0),2);
    public static decimal ToDecimal(this int? value) => Math.Round((decimal)(value ?? 0),2);
}