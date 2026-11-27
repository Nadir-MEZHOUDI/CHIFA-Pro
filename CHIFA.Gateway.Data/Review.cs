namespace CHIFA.Gateway.Data;

/// <summary>A subscribed pharmacy's rating of SmartCHIFA, shown on the website. One per pharmacy.</summary>
public class Review
{
    public const int MaxCommentLength = 1000;
    public const int MaxNameLength = 100;

    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid PharmacyId { get; set; }
    public Pharmacy Pharmacy { get; set; } = null!;

    /// <summary>1 to 5.</summary>
    public int Stars { get; set; }

    public required string Comment { get; set; }

    /// <summary>Name shown on the website, e.g. the pharmacy's or the pharmacist's.</summary>
    public required string AuthorName { get; set; }

    public string? City { get; set; }

    /// <summary>Shown on the website until the admin hides it.</summary>
    public bool IsPublished { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
