namespace CHIFA.Services.Statistics;

[DataContract]
public class ThisWeekStat
{
    private static readonly CultureInfo Culture = new("ar");
    public ThisWeekStat(DateTime date) => Date = date;
    public ThisWeekStat() { }
    public DateOnly? DateFact => DateOnly.FromDateTime(Date);
    public string? Day => DateFact?.ToString("dddd", Culture).ToUpper();
    public decimal? Marge => (Montant / 6).ToDecimal();
    public decimal? Brut => (Marge + Maj).ToDecimal();
    [DataMember(Order = 1)] public int Count { get; set; }
    [DataMember(Order = 2)] public DateTime Date { get; set; }
    [DataMember(Order = 3)] public decimal? Montant { get; set; } = 0;
    [DataMember(Order = 4)] public decimal? MontantAs { get; set; } = 0;
    [DataMember(Order = 5)] public decimal? MontantOff { get; set; } = 0;
    [DataMember(Order = 6)] public decimal? Maj { get; set; } = 0;
}



