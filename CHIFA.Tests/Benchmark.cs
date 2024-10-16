using BenchmarkDotNet.Attributes;

using CHIFA.Stat.ViewModels;

using System;

namespace CHIFA.Tests;
public class Benchmark
{

    [GlobalSetup]
    public static void Setup()
    {
        Console.WriteLine(@"-------------------------------Run Setup --------------------------");
    }
    ChifaStatService db = ChifaStatService.Instance;


    [Benchmark]
    public object GetFactures()
    {
        var list = db.Factures;
        Console.WriteLine("GetFactures Count: " + list.Count);
        return list;
    }

    [Benchmark]
    public object GetBordereaux()
    {
        var list = db.Bordereaux;
        Console.WriteLine("GetBordereaux Count: " + list.Count);
        return list;
    }
    [Benchmark]
    public object GetMedics()
    {
        var list = db.Medicaments;
        Console.WriteLine("GetMedics Count: " + list.Count);
        return list;
    }

    

}