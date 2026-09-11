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
        if (!Directory.Exists(searchDirectory))
        {
            return;
        }

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
}