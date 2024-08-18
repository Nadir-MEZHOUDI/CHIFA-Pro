namespace CHIFA.DAL.DTOs;

public class Period
{
    public DateTime? From { get; set; } = DateTime.MinValue;
    public bool IsNull => From == null && To == null;
    public DateTime? To { get; set; } = DateTime.MaxValue;
}