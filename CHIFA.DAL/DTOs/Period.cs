namespace CHIFA.DAL.DTOs;

public class Period
{
    public DateTime? From { get; set; } = DateTime.Today.AddYears(-2);
    public DateTime? To { get; set; } = DateTime.Today;
}