namespace CHIFA.Contract.Statistics;
[DataContract]
public class YearlyStat
{
    [DataMember(Order = 1)] public int? Year { get; init; }
    [DataMember(Order = 2)] public DateTime? DateDebut { get; init; }
    [DataMember(Order = 3)] public DateTime? DateFin { get; init; }
    [DataMember(Order = 4)] public int Factures { get; init; }
    [DataMember(Order = 5)] public decimal? MontantMaj { get; init; }
    [DataMember(Order = 6)] public decimal? MontantFE { get; init; }
    [DataMember(Order = 7)] public decimal? MontantFact { get; init; }
    [DataMember(Order = 8)] public decimal? MontantOff { get; init; }
    [DataMember(Order = 9)] public int Assureis { get; init; }
    [DataMember(Order = 10)] public int Beneficiaires { get; init; }
    public string Date => $"{Year:D4}";
    public int? Jours => (int)(DateFin?.Date - DateDebut?.Date)?.TotalDays! + 1;
    public decimal? MontantJour => (MontantOff / Jours).ToDecimal();
    public decimal? FactureJour => (Factures / Jours).ToDecimal();
    public decimal? MontantFacture => (MontantOff / Factures).ToDecimal();
    public decimal? MontantGlobal => (MontantOff + MontantMaj + MontantFE).ToDecimal();
    public decimal? Marge => (MontantOff / 6).ToDecimal();
    public decimal? Brut => (Marge + MontantMaj).ToDecimal();

}
