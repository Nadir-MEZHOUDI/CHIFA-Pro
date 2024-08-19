namespace CHIFA.DAL.Statistics;

public class FactureByMonth
{
    public int Count { get; set; }
    public string Date { get; set; }
    public decimal? Montant { get; set; }
    public int? Day { get; internal set; }
    public int? Month { get; internal set; }
    public int? Year { get; internal set; }
    public string? DayOfWeek { get; internal set; }
}