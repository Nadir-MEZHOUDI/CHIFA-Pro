using CHIFA.DAL.DTOs;
using CHIFA.DAL.Helpers;

using DataModel;

using System.Linq.Expressions;

namespace CHIFA.DAL.DataServices;

public static class StatisticsExtensions
{

    private static readonly Period defaultPeriod = new();

    public static Expression<Func<Facture, bool>> SetPeriod(this Expression<Func<Facture, bool>>? predicate, Period? period = default)
    {
        period ??= defaultPeriod;

        predicate ??= _ => true;

        predicate = predicate.And(x => x.DateFact != null);

        if (period?.From.HasValue == true)
            predicate = predicate.And(x => x.DateFact > period.From);

        if (period?.To.HasValue == true)
            predicate = predicate.And(x => x.DateFact < period.To);

        return predicate;

    }
    public static Expression<Func<DetailFact, bool>> SetPeriod(this Expression<Func<DetailFact, bool>>? predicate, Period? period = default)
    {
        period ??= defaultPeriod;
        predicate ??= _ => true;

        predicate = predicate.And(x => x.Facture.DateFact.HasValue);

        if (period?.From.HasValue == true)
            predicate = predicate.And(x => x.Facture.DateFact > period.From);

        if (period?.To.HasValue == true)
            predicate = predicate.And(x => x.Facture.DateFact < period.To);

        return predicate;
    }
    public static Expression<Func<Bordereau, bool>> SetPeriod(this Expression<Func<Bordereau, bool>>? predicate, Period? period = default)
    {
        period ??= defaultPeriod;

        predicate ??= _ => true;

        predicate = predicate.And(x => x.DateExtract !=null);

        if (period?.From.HasValue == true)
            predicate = predicate.And(x => x.DateExtract > period.From);

        if (period?.To.HasValue == true)
            predicate = predicate.And(x => x.DateExtract < period.To);

        return predicate;
    }
    public static string FullName(this Medicament? m) => $"{m?.NomCom} {m?.Dosage} {m?.Conditionnement}";

}