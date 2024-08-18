namespace CHIFA.DAL.DTOs;

public class FactureDetailDto
{
    public bool? Ts{ get; set; }
    public string Code { get; set; }
    public decimal? DureeTrait { get; set; }
    public decimal? MajLocal { get; set; }
    public decimal? MajSub { get; set; }
    public decimal? MontAss { get; set; }
    public decimal? MontPharm { get; set; }
    public decimal? Ppa { get; set; }
    public decimal? Qt { get; set; }
    public decimal? TR { get; set; }
     public string Medicament { get; set; } = "*";

}