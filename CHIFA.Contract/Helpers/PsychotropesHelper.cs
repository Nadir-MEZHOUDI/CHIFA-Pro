namespace CHIFA.Contract.Helpers;

public static class PsychotropesHelper
{
    /// <summary>
    /// Arrêté interministériel du 11 août 2021 - Produits psychotropes à risque avéré d'abus (Ordonnance à souche).
    /// </summary>
    public static readonly HashSet<string> PsychotropesAbusDciCodes = new(StringComparer.OrdinalIgnoreCase)
    {
        "03F013", "03F047", "03F049", "03F107", "03F111", "03F115", "03F131",
        "15A008", "15A009", "15A065", "15A066", "15A067", "15A068", "15A069", "15A101",
        "15D033", "15D034", "15D062", "15D093",
        "16B021", "16B022",
        "16C051", "16C152", "16C095"
    };

    /// <summary>
    /// Arrêté du 09 juillet 2015 - Produits psychotropes réglementés.
    /// </summary>
    public static readonly HashSet<string> PsychotropesReglementesDciCodes = new(StringComparer.OrdinalIgnoreCase)
    {
        "03F029", "16B023", "16B024", "16B098", "16B108", "16B109", "16B019", "16B020",
        "16B025", "16B026", "16B027", "16B028", "16B029", "16C050", "16B042", "16B043",
        "16B044", "16B045", "16B046", "16C052", "15A012", "15A013", "15A014", "16B078",
        "16B080", "21E030"
    };

    /// <summary>
    /// Codes DCI exclus dans les classes psychotropes (médicaments non psychotropes de la même classe).
    /// </summary>
    public static readonly HashSet<string> ExcludedDciCodes = new(StringComparer.OrdinalIgnoreCase)
    {
        "15A001", "15A002", "15A003", "15A004", "15A006", "15A016", "15A036", "15A043", "15A051",
        "15A052", "15A053", "15A056", "15A057", "15A058", "15A063", "15A064", "15A072", "15A073",
        "15A076", "15A080", "15A081", "15A082", "15A083", "15A084", "15A089", "15A090", "15A100",
        "15A102", "15A105", "15A109", "15A110", "15D028", "15D029", "15D030", "15D031", "15D032",
        "15D077", "15D097", "15D098", "16A001", "16A002", "16A003", "16A004", "16A005", "16A006",
        "16A007", "16A010", "16A011", "16A013", "16A014", "16A016", "16A017", "16A018", "16A019",
        "16A020", "16A021", "16A078", "16A086", "16A087", "16A088", "16A091", "16A092", "16A095",
        "16A096", "16A097", "16A099", "16A105", "16A106", "16A107", "16A115", "16A125", "16A126",
        "16A130", "16A154", "16A158", "16A165", "16B040", "16B041", "16B079", "16B093", "16D045",
        "16D053", "16D054", "16D055", "16D056", "16D057", "16D058", "16D059", "16D060", "16D061",
        "16D066", "16D067", "16D070", "16D071", "16D072", "16D073", "16D074", "16D075", "16D076",
        "16D081", "16D082", "16D083", "16D084", "16D085", "16D086", "16D087", "16D088", "16D089",
        "16D090", "16D091", "16D092", "16D094", "16D099", "16D100", "16D101", "16D103", "16D104",
        "16D110", "16D111", "16D112", "16D113", "16D114", "16D122", "16D132", "16D134", "16D142",
        "16D143", "16D145", "16D146", "16D155", "16D160", "16D161", "16D170", "16D171", "16E077"
    };

    /// <summary>
    /// Préfixes de classes DCI considérées comme substances psychotropes dans le système CHIFA.
    /// </summary>
    public static readonly string[] PsychotropeDciPrefixes = ["16B", "16C", "15A", "15D", "16A", "16D", "16E", "03F"];

    /// <summary>
    /// Liste de tous les codes DCI psychotropes explicites pour requête SQL.
    /// </summary>
    public static readonly List<string> ExplicitDciCodes = PsychotropesAbusDciCodes
        .Union(PsychotropesReglementesDciCodes)
        .ToList();

    public static string GetCategorie(string? codeDci)
    {
        if (string.IsNullOrWhiteSpace(codeDci)) return "Autre";
        var trimmed = codeDci.Trim().ToUpperInvariant();

        if (PsychotropesAbusDciCodes.Contains(trimmed))
            return "Risque Avéré d'Abus (Arrêté 11/08/2021 - Ordonnance à Souche)";

        if (PsychotropesReglementesDciCodes.Contains(trimmed))
            return "Substance Psychotrope Réglementée (Arrêté 09/07/2015)";

        return "Substance Psychotrope / Tableau B";
    }
}
