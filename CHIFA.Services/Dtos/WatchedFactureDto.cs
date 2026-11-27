namespace CHIFA.Services.Dtos;

/// <summary>A new facture as the watch alerts need it: who, how much, and what was dispensed.</summary>
public class WatchedFactureDto
{
    public string NumFact { get; set; } = "";
    public DateTime DateFact { get; set; }
    public string? NumAssure { get; set; }
    public string? Rang { get; set; }
    public string? Malade { get; set; }
    public decimal MontFact { get; set; }
    public List<WatchedLigneDto> Lignes { get; set; } = [];

    public decimal Boites => Lignes.Sum(l => l.Qte);
}

public class WatchedLigneDto
{
    public string NumEnr { get; set; } = "";
    public string? Nom { get; set; }
    public decimal Qte { get; set; }
    public bool Psychotrope { get; set; }
}
