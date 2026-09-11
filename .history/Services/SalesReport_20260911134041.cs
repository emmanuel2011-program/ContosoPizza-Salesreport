using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ContosoPizza.Services;

public class SalesData
{
    public decimal Total { get; set; }
}

public static class SalesReporter
{
    public static void GenerateSalesSummaryReport(string searchDirectory, string reportOutputPath)
    {
        // Auto-create sample store directories and sales.json files if missing
        EnsureSampleDataExists(searchDirectory);

        var salesFiles = Directory.EnumerateFiles(searchDirectory, "sales.json", SearchOption.AllDirectories);

        decimal grandTotal = 0;
        var reportLines = new List<string>
        {
            "Sales Summary Report",
            "--------------------------------"
        };

        foreach (var file in salesFiles)
        {
            string jsonString = File.ReadAllText(file);
            var salesData = JsonSerializer.Deserialize<SalesData>(jsonString);

            decimal fileTotal = salesData?.Total ?? 0;
            grandTotal += fileTotal;

            string relativePath = Path.GetRelativePath(searchDirectory, file);
            reportLines.Add($"{relativePath}: {fileTotal:C}");
        }

        reportLines.Add("--------------------------------");
        reportLines.Add($"Total Sales: {grandTotal:C}");

        File.WriteAllLines(reportOutputPath, reportLines);
    }

    private static void EnsureSampleDataExists(string storesDir)
    {
        if (!Directory.Exists(storesDir))
        {
            Directory.CreateDirectory(Path.Combine(storesDir, "201"));
            Directory.CreateDirectory(Path.Combine(storesDir, "202"));

            File.WriteAllText(Path.Combine(storesDir, "201", "sales.json"), "{\"Total\": 2500.00}");
            File.WriteAllText(Path.Combine(storesDir, "202", "sales.json"), "{\"Total\": 3150.50}");
        }
    }
}