namespace CHIFA.DAL.DTOs;

public class Gp
{
    public string Class => Type == 'G' ? "Générique" : Type == 'P' ? "Princeps" : "--";
    public decimal Montant { get; set; }
    public char? Type { get; set; }
}