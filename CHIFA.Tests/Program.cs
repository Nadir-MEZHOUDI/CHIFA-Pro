using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using CHIFA.Stat.Dtos;
using CHIFA.Stat.ViewModels;

using DataModel;

using LinqToDB;
using LinqToDB.Data;

namespace CHIFA.Tests;
public class Program
{
    static ChifaDb db = new ChifaDb();
    static int N = 10;
    static List<(string MethodName, TimeSpan Time)> timeDic = new List<(string method, TimeSpan time)>();

    public static async Task Main()
    {
        try
        {
            DataConnection.TurnTraceSwitchOn();
            // DataConnection.WriteTraceLine = (s1, s2, s3) => LogQuery(s1, s2, s3);
            Benchmark benchmark = new Benchmark();
            timeDic.Clear();

            long time = Stopwatch.GetTimestamp();
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine($"-------------------Run {i} --------------------------");

                time = Stopwatch.GetTimestamp();
                benchmark.GetBordereaux();
                Console.WriteLine($"GetBordereaux: {Stopwatch.GetElapsedTime(time)}");
                time = Stopwatch.GetTimestamp();
                await ChifaStatService.Instance.LoadDataAsync();
                Console.WriteLine($"Load All Data Async In : {Stopwatch.GetElapsedTime(time)}");

                time = Stopwatch.GetTimestamp();
                ChifaStatService.Instance.LoadData();
                Console.WriteLine($"Load All Data In : {Stopwatch.GetElapsedTime(time)}");

            }
            for (int i = 0; i < N; i++)
            {
                Console.WriteLine($"-------------------Run {i} --------------------------");

                time = Stopwatch.GetTimestamp();
                benchmark.GetBordereaux();
                LogQuery(nameof(benchmark.GetBordereaux), time);

                time = Stopwatch.GetTimestamp();
                benchmark.GetMedics();
                LogQuery(nameof(benchmark.GetMedics), time);

                time = Stopwatch.GetTimestamp();
                benchmark.GetFactures();
                LogQuery(nameof(benchmark.GetFactures), time);




                time = Stopwatch.GetTimestamp();
                SelectAnonymous();
                LogQuery(nameof(SelectAnonymous), time);

                time = Stopwatch.GetTimestamp();
                SelectObject();
                LogQuery(nameof(SelectObject), time);

                //time = Stopwatch.GetTimestamp();
                //Method6();
                //LogQuery(nameof(Method6));


                Console.WriteLine($"-------------------End {i} --------------------------");
            }
            ShowMedians();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    private static void ShowMedians()
    {
        Console.WriteLine($"-------------------Medians --------------------------");
        Console.ForegroundColor = ConsoleColor.Blue;

        var list = timeDic.GroupBy(x => x.MethodName).Select(x => new
        {
            MethodName = x.Key,
            Avrg = TimeSpan.FromTicks(Convert.ToInt64(x.Average(x => x.Time.Ticks)))
        })
        .OrderByDescending(x => x.Avrg)
        .ToList();
        foreach (var item in list)
        {
            Console.WriteLine($"{item.MethodName} : {item.Avrg}");
        }
        Console.ResetColor();
    }

    public static object Method1()
    {
        var list = db.Factures
           .Select(x => x.DateFact.Value.Year)
           .Distinct()
           .ToList()
           .OrderDescending()
           .ToList();
        return list;
    }

    public static object Method2()
    {
        var list = db.Factures
           .Select(x => x.DateFact.Value.Year)
           .Distinct()
           .OrderByDescending(x => x)
           .ToList();
        return list;

    }
    public static object Method3()
    {
        var list = db.Factures
           .Select(x => x.DateFact.Value.Year)
           .Distinct()
           .ToList()
           .OrderDescending()
           .ToList();
        return list;
    }



    public static object SelectAnonymous()
    {
        var list = db.Bordereaus.Select(x => new
        {
            x.NumBord,
            x.Center.Nom,
            Factures = x.Factures.Select(f => new
            {
                f.NumFact,
                f.MontFact,
                f.MontMaj,
                f.NumAssure,
                Details = f.DetailFacts.Select(x => new
                {
                    x.Qte,
                    x.Ppa
                })
            })
        })
             .ToList();
        Console.WriteLine("SelectAnonymous Count: " + list.Count);
        return list;
    }
    public static object SelectObject()
    {
        var list = db.Bordereaus

   .Select(x => new BordStat
   {
       NumBord = x.NumBord,
       Center = x.Center.Nom,
       Factures = x.Factures.Select(f => new FactureStat
       {
           NumFact = f.NumFact,
           Montant = f.MontFact,
           Maj = f.MontMaj,
           NumAssure = f.NumAssure,
           Details = f.DetailFacts.Select(d => new DetailFactStat
           {
               Qte = d.Qte,
               Ppa = d.Ppa
           })
       })
   })
   .ToList();
        Console.WriteLine("SelectObject Count: " + list.Count);

        return list;
    }
    public static object Method6()
    {
        var list = db.Bordereaus
            .LoadWith(x => x.Center)
            .LoadWith(x => x.Factures)
            .ThenLoad(f => f.DetailFacts)
            .ToList();
        Console.WriteLine("Mathod 4 Count: " + list.Count);

        return list;
    }
    public static void LogQuery(string? s1, string? s2, object s3 = null)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(s1, s2);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(s3);
        Console.ResetColor();
    }

    public static void LogQuery(string Method, long time)
    {
        var elapsed = Stopwatch.GetElapsedTime(time);
        timeDic.Add((Method, elapsed));
        Console.WriteLine($"{Method}: {elapsed}");

    }

}





