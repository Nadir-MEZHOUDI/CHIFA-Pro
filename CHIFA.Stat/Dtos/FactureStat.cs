namespace CHIFA.Stat.Dtos;

public class FactureStat
{
    public string? NumFact { get; set; }
    public decimal? Montant { get; set; }
    public string? NumBord { get; set; }
    public DateTime? DateSoin { get; set; }
    public DateTime? DateFact { get; set; }
    public string? Center { get; set; }
    public string? NumAssure { get; set; } = string.Empty;
    public string? Beneficiaire { get; set; }
    public decimal? Maj { get; set; }
    public string? NumMalad { get; set; }

    public int Lines => Details.Count();
    public decimal Boites => Details.Sum(x => x.Qte);


    public int Year => DateFact?.Year ?? 0;
    public int Month => DateFact?.Month ?? 0;
    public int Day => DateFact?.Day ?? 0;
    public string? DayName => DateFact?.ToString("dddd");
    public string? MonthName => DateFact?.ToString("MMMM");
    public string YearMonth => $"{Year}-{Month}";
    public DayOfWeek? DayOfWeek => DateFact?.DayOfWeek;
    public string? Date => DateFact?.ToString("dddd  dd-MM-yyyy");

    public IEnumerable<DetailFactStat> Details { get; set; }
}

