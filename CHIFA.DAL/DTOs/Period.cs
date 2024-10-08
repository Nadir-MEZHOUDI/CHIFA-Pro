using DataModel;
using LinqToDB;

namespace CHIFA.DAL.DTOs;

public class Period
{
    public DateTime? From { get; set; } = DateTime.Today.AddYears(-2);
    public DateTime? To { get; set; } = DateTime.Today;
    public  static DateTime MaxDate { get; set; }
    public  static DateTime MinDate { get; set; }


    public static async Task GetMinAndMaxDatesAsync()
    {
        await using var db = new ChifaDb();
        MinDate = await db.Factures.MinAsync(x => x.DateFact)?? new DateTime(2000,1,1);
        MaxDate = await db.Factures.MaxAsync(x => x.DateFact) ??  DateTime.Now;
    }
}