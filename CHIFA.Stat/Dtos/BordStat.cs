namespace CHIFA.Stat.Dtos;

public class BordStat
{
    public int NmbrOrd => Factures.Count();
    public string NumBord { get; set; } = string.Empty;
    public string Center { get; set; } = string.Empty;
    public DateTime? MinDate => Factures.Min(x => x.DateFact);
    public DateTime? MaxDate => Factures.Max(x => x.DateFact);

    public decimal Boites => Factures.Sum(f => f.Details.Sum(c => c.Qte));
    public int Lines => Factures.Sum(f => f.Details.Count());
    public decimal? Maj => Factures.Sum(f => f.Maj);
    public decimal? Montant => Factures.Sum(f => f.Montant);
    public bool IsOpen { get; set; }



    public int Year => MinDate?.Year ?? 0;
    public int Month => MinDate?.Month ?? 0;
    public int Day => MinDate?.Day ?? 0;
    public string DayName => MinDate?.ToString("dddd");
    public string MonthName => MinDate?.ToString("MMMM");
    public string YearMonth => $"{Year}-{Month}";
    public DayOfWeek? DayOfWeek => MinDate?.DayOfWeek;
    public string Date => MinDate?.ToString("dddd  dd/MM/yyyy");


    public IEnumerable<FactureStat> Factures { get; set; }

}