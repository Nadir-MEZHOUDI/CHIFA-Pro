using System.Security.Claims;
using CHIFA.Gateway.Features.Account;
using CHIFA.Gateway.Features.Admin;
using CHIFA.Gateway.Features.Agents;

namespace CHIFA.Gateway.Features.Reviews;

/// <param name="CanReview">The pharmacy has a paid subscription (not a trial, not expired).</param>
/// <param name="Sent">The pharmacy already gave its review: the phone stops asking.</param>
/// <param name="SuggestedName">Pre-filled « Nom affiché ».</param>
public record ReviewState(bool CanReview, bool Sent, string SuggestedName);

public record ReviewRequest(int Stars, string? Comment, string? AuthorName, string? City);

public record Testimonial(int Stars, string Comment, string AuthorName, string? City, DateTime CreatedAt);

/// <param name="Average">Over every published review, not only the ones listed.</param>
public record TestimonialsResponse(double Average, int Count, List<Testimonial> Items);

public record AdminReview(Guid Id, Guid PharmacyId, string Pharmacy, int Stars, string Comment, string AuthorName, string? City, bool IsPublished, DateTime CreatedAt);

public record ReviewModeration(bool IsPublished);

/// <summary>
/// Subscribed pharmacies rate SmartCHIFA from the phone app; the website shows the published reviews.
/// </summary>
public static class ReviewEndpoints
{
    public const string WebsiteCorsPolicy = "website";
    public const int MinCommentLength = 10;
    public const int PublicListSize = 30;

    public static void MapReviewEndpoints(this IEndpointRouteBuilder app)
    {
        var phone = app.MapGroup("/api/review").RequireOwner();
        phone.MapGet("", StateAsync);
        phone.MapPost("", SendAsync);

        app.MapGet("/api/testimonials", TestimonialsAsync).AllowAnonymous().RequireCors(WebsiteCorsPolicy);

        var admin = app.MapGroup("/admin/reviews").AllowAnonymous().AddEndpointFilter(AdminEndpoints.RequireAdminKey);
        admin.MapGet("", ListAsync);
        admin.MapPut("/{id:guid}", ModerateAsync);
    }

    public static bool CanReview(Pharmacy pharmacy, DateTime utcNow)
    {
        var subscription = AccountEndpoints.Subscription(pharmacy, utcNow);
        return !subscription.IsTrial && subscription.Status is SubscriptionStates.Active or SubscriptionStates.Expiring;
    }

    private static async Task<IResult> StateAsync(ClaimsPrincipal user, IDbContextFactory<GatewayDbContext> dbFactory, CancellationToken ct)
    {
        var pharmacyId = user.GetPharmacyId();
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        var pharmacy = await db.Pharmacies.AsNoTracking().FirstOrDefaultAsync(p => p.Id == pharmacyId, ct);
        if (pharmacy is null)
            return Results.NotFound();

        var sent = await db.Reviews.AnyAsync(r => r.PharmacyId == pharmacyId, ct);
        return Results.Ok(new ReviewState(CanReview(pharmacy, DateTime.UtcNow), sent, pharmacy.Name));
    }

    private static async Task<IResult> SendAsync(ReviewRequest request, ClaimsPrincipal user, IDbContextFactory<GatewayDbContext> dbFactory, CancellationToken ct)
    {
        var comment = request.Comment?.Trim() ?? "";
        var authorName = request.AuthorName?.Trim() ?? "";
        var city = string.IsNullOrWhiteSpace(request.City) ? null : request.City.Trim();

        var errors = new Dictionary<string, string[]>();
        if (request.Stars is < 1 or > 5)
            errors["stars"] = ["Choisissez de 1 à 5 étoiles."];
        if (comment.Length < MinCommentLength)
            errors["comment"] = [$"Écrivez au moins {MinCommentLength} caractères."];
        else if (comment.Length > Review.MaxCommentLength)
            errors["comment"] = [$"{Review.MaxCommentLength} caractères au maximum."];
        if (authorName.Length == 0)
            errors["authorName"] = ["Le nom affiché est obligatoire."];
        else if (authorName.Length > Review.MaxNameLength)
            errors["authorName"] = [$"{Review.MaxNameLength} caractères au maximum."];
        if (city?.Length > Review.MaxNameLength)
            errors["city"] = [$"{Review.MaxNameLength} caractères au maximum."];
        if (errors.Count > 0)
            return Results.ValidationProblem(errors, title: errors.Values.First()[0]);

        var pharmacyId = user.GetPharmacyId();
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        var pharmacy = await db.Pharmacies.AsNoTracking().FirstOrDefaultAsync(p => p.Id == pharmacyId, ct);
        if (pharmacy is null)
            return Results.NotFound();

        if (!CanReview(pharmacy, DateTime.UtcNow))
            return Results.Problem(statusCode: StatusCodes.Status403Forbidden,
                title: "Réservé aux pharmacies abonnées.", detail: "Les avis sont ouverts aux pharmacies avec un abonnement SmartCHIFA actif.");

        if (await db.Reviews.AnyAsync(r => r.PharmacyId == pharmacyId, ct))
            return AlreadySent();

        db.Reviews.Add(new Review { PharmacyId = pharmacyId, Stars = request.Stars, Comment = comment, AuthorName = authorName, City = city });
        try
        {
            await db.SaveChangesAsync(ct);
        }
        catch (DbUpdateException)
        {
            // Two phones of the pharmacy sent at the same time: the unique index kept the first.
            return AlreadySent();
        }

        return Results.NoContent();
    }

    private static IResult AlreadySent() =>
        Results.Problem(statusCode: StatusCodes.Status409Conflict,
            title: "Votre pharmacie a déjà donné son avis.", detail: "Merci ! Un seul avis par pharmacie.");

    private static async Task<IResult> TestimonialsAsync(HttpContext http, IDbContextFactory<GatewayDbContext> dbFactory, CancellationToken ct)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        var published = db.Reviews.AsNoTracking().Where(r => r.IsPublished);

        var stars = await published.Select(r => r.Stars).ToListAsync(ct);
        var items = await published
            .OrderByDescending(r => r.Stars).ThenByDescending(r => r.CreatedAt)
            .Take(PublicListSize)
            .Select(r => new Testimonial(r.Stars, r.Comment, r.AuthorName, r.City, r.CreatedAt))
            .ToListAsync(ct);

        http.Response.Headers.CacheControl = "public, max-age=300";
        var average = stars.Count == 0 ? 0 : Math.Round(stars.Average(), 1);
        return Results.Ok(new TestimonialsResponse(average, stars.Count, items));
    }

    private static async Task<IResult> ListAsync(IDbContextFactory<GatewayDbContext> dbFactory, CancellationToken ct)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        var reviews = await db.Reviews.AsNoTracking()
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => new AdminReview(r.Id, r.PharmacyId, r.Pharmacy.Name, r.Stars, r.Comment, r.AuthorName, r.City, r.IsPublished, r.CreatedAt))
            .ToListAsync(ct);
        return Results.Ok(reviews);
    }

    private static async Task<IResult> ModerateAsync(Guid id, ReviewModeration request, IDbContextFactory<GatewayDbContext> dbFactory, CancellationToken ct)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        var review = await db.Reviews.FindAsync([id], ct);
        if (review is null)
            return Results.NotFound();

        review.IsPublished = request.IsPublished;
        await db.SaveChangesAsync(ct);
        return Results.NoContent();
    }
}
