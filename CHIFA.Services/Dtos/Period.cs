namespace CHIFA.Services.Dtos;

public class Period
{
    public DateTime? From { get; set; } = DateTime.Today.AddYears(-2);
    public DateTime? To { get; set; } = DateTime.Today;
    public  static DateTime MaxDate { get; set; }
    public  static DateTime MinDate { get; set; }
    
}