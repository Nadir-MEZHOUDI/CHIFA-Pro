namespace CHIFA.Contract.Statistics;

[DataContract]
public class WeeklyStat
{

    [DataMember(Order = 1)] public string StartDate { get; set; } = "";
    [DataMember(Order = 2)] public DateTime? DateDebut { get; init; }
    [DataMember(Order = 3)] public int Factures { get; init; }
    [DataMember(Order = 4)] public decimal? MontantMaj { get; init; }
    [DataMember(Order = 5)] public decimal? MontantFact { get; init; }
    [DataMember(Order = 6)] public decimal? MontantOff { get; init; }
    [DataMember(Order = 7)] public decimal? MontantFE { get; init; }
    [DataMember(Order = 8)] public int Assureis { get; init; }
    [DataMember(Order = 9)] public int Beneficiaires { get; init; }



    public string Date => $"{Month:D2} - {Year:D4}";
    public int? Month => DateDebut?.Month;
    public int? Year => DateDebut?.Year;

    public DateTime? DateFin => DateDebut?.AddDays(6);
    public int? Jours => DateFin.HasValue && DateDebut.HasValue ? (int)(DateFin.Value.Date - DateDebut.Value.Date).TotalDays + 1 : null;

    public decimal? MontantJour => (MontantOff / Jours).ToDecimal();
    public decimal? FactureJour => (Factures / Jours).ToDecimal();
    public decimal? MontantFacture => (MontantOff / Factures).ToDecimal();

    public decimal? Marge => (MontantOff / 6).ToDecimal();
    public decimal? Brut => (Marge + MontantMaj).ToDecimal();

    public decimal? MontantGlobal => (MontantOff + MontantMaj + MontantFE).ToDecimal();


}
