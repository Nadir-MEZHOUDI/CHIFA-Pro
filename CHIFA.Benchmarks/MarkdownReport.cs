using System.Text;
using BenchmarkDotNet.Reports;

namespace CHIFA.Benchmarks;

public static class MarkdownReport
{
    private static readonly string[] Trailing = { "Mean", "Error", "StdDev", "Ratio", "RatioSD", "Allocated", "Alloc Ratio" };

    public static void Save(IEnumerable<Summary> summaries, string path)
    {
        var sb = new StringBuilder();
        sb.AppendLine("# CHIFA Benchmarks - LinqToDB vs Dapper");
        sb.AppendLine();
        sb.AppendLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        sb.AppendLine();
        sb.AppendLine("Each benchmark reports median wall time and allocated memory per invocation. Baseline = LinqToDB (Ratio=1.00).");
        sb.AppendLine();
        sb.AppendLine("## Summary");
        sb.AppendLine();
        sb.AppendLine("| Benchmark | Method | Mean | Ratio | Allocated | Alloc Ratio |");
        sb.AppendLine("|---|---|---:|---:|---:|---:|");

        foreach (var s in summaries)
        {
            var benchName = s.Title.Split('-')[0].Split('.').Last();
            var rows = s.Table.FullContent;
            if (rows.Length < 2) continue;
            // Find the Job column index by scanning all rows for the first occurrence of "Quick"
            int quickIdx = -1;
            foreach (var r in rows)
            {
                var idx = Array.FindIndex(r, c => c == "Quick");
                if (idx >= 0) { quickIdx = idx; break; }
            }
            if (quickIdx < 0) continue;

            foreach (var r in rows)
            {
                if (r.Length <= quickIdx || r[quickIdx] != "Quick") continue;
                var n = r.Length;
                if (n < 4) continue;
                var allocRatio = r[n - 1];
                var allocated = r[n - 2];

                int meanIdx = -1;
                for (int i = n - 3; i >= Math.Max(0, n - 7); i--)
                {
                    if (r[i] != null && (r[i].EndsWith(" ms") || r[i].EndsWith(" ns") || r[i].EndsWith(" us")))
                    {
                        meanIdx = i;
                        break;
                    }
                }
                if (meanIdx < 0) continue;
                var mean = r[meanIdx];

                string ratio = "?";
                for (int i = n - 3; i > meanIdx; i--)
                {
                    var cell = r[i];
                    if (cell == null) continue;
                    if (cell == "?") { ratio = cell; break; }
                    if (cell.Contains("/")) continue;
                    if (cell == "1.00" || (cell.Contains(".") && !cell.EndsWith("ms") && !cell.EndsWith("us") && !cell.EndsWith("ns")))
                    {
                        ratio = cell;
                    }
                }

                var method = r[0];
                sb.AppendLine($"| {benchName} | {method} | {mean} | {ratio} | {allocated} | {allocRatio} |");
            }
        }

        sb.AppendLine();
        sb.AppendLine("## Full results");
        sb.AppendLine();
        foreach (var s in summaries)
        {
            sb.AppendLine($"### {s.Title.Replace("-", " - ", StringComparison.Ordinal)}");
            sb.AppendLine();
            foreach (var line in s.Table.FullContent)
            {
                sb.AppendLine(string.Join(" | ", line.Select(c => c ?? "")));
            }
            sb.AppendLine();
        }

        File.WriteAllText(path, sb.ToString());
    }
}

