namespace CHIFA.DAL.DTOs;

public class BordereauDto
{
    public string? Center { get; set; } = string.Empty;
    public DateTime? LastFacture { get; set; }
    public DateTime? FirstFacture { get; set; }
    public int Etat => State == 'C' ? 2 : State == 'O' ? 1 : 0;
    public decimal? Maj { get; set; }
    public decimal? MFAE => Nmbr * 5;
    public decimal? MontGlobal => MontOff + Maj + MFAE;
    public decimal? MontOff { get; set; }
    public int? Nmbr { get; set; }
    public string? Num { get; set; }
    public string? Obs { get; set; }
    public char? State { get; set; }
    public decimal? Virment { get; set; } = 0;
    public decimal? Ecart => Virment <= 0 ? 0 : MontGlobal - Virment;

    public string DepotFtp => DateDepotFtp.HasValue && DateDepotFtp.Value.Year > 2000 ? DateDepotFtp.Value.ToString() : string.Empty;
    public DateTime? DateDepotFtp { get; set; }
    public DateTime? DateOuverture { get; set; }
    public DateTime? DateCloture { get; set; }
}