namespace CHIFA.Services.Dtos;

[DataContract]
public class BordereauDto
{
   [DataMember(Order=1)]public string? Center { get; set; } = string.Empty;
   [DataMember(Order=2)]public DateTime? LastFacture { get; set; }
   [DataMember(Order=3)]public DateTime? FirstFacture { get; set; }
   [DataMember(Order=4)]public decimal? Maj { get; set; }
   [DataMember(Order=5)]public decimal? MontOff { get; set; }
   [DataMember(Order=6)]public int? Nmbr { get; set; }
   [DataMember(Order=7)]public string? Num { get; set; }
   [DataMember(Order=8)]public string? Obs { get; set; }
   [DataMember(Order=9)]public char? State { get; set; }
   [DataMember(Order=10)]public decimal? Virment { get; set; } = 0;
   [DataMember(Order=11)]public DateTime? DateOuverture { get; set; }
   [DataMember(Order=12)]public DateTime? DateDepotFtp { get; set; }
   [DataMember(Order=13)]public DateTime? DateCloture { get; set; }
    public string DepotFtp => DateDepotFtp is { Year: > 2000 } ? DateDepotFtp.Value.ToString("MM-dd-yyyy HH:mm", CultureInfo.InvariantCulture) : string.Empty;
    public decimal? Ecart => Virment <= 0 ? 0 : MontGlobal - Virment;
    public decimal? MFAE => Nmbr * 5;
    public int Etat => State == 'C' ? 2 : State == 'O' ? 1 : 0;
    public decimal? MontGlobal => MontOff + Maj + MFAE;
}