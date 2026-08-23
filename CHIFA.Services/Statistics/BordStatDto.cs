namespace CHIFA.Services.Statistics;

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
    public DateOnly? Date => DateFin.HasValue ? DateOnly.FromDateTime(DateFin.Value) : null;
    public int? Jours => DateFin.HasValue && DateDebut.HasValue ? (int)(DateFin.Value - DateDebut.Value).TotalDays + 1 : null;
    public decimal? MontantJour => Jours.HasValue && Jours > 0 ? (MontantOff / Jours).ToDecimal() : null;
    public decimal? FactureJour => Jours.HasValue && Jours > 0 ? (Factures / Jours).ToDecimal() : null;
    public decimal? MontantFacture => ((MontantOff / Factures)).ToDecimal();
    public decimal? MontantGlobal => MontantOff + MontantMaj + MontantFE;

    public decimal? Marge => (MontantOff / 6).ToDecimal();
    public decimal? Brut => (Marge + MontantMaj).ToDecimal();
    public decimal? Net => (Brut - Ecart).ToDecimal();
    public decimal? Ecart => (Virement > 0 ? MontantGlobal - Virement : 0).ToDecimal();

}
