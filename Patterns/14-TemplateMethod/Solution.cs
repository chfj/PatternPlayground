using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Patterns.TemplateMethod.Solution;

public sealed record ImportedRow(string Name, string Email);

public abstract class DataImporter
{
    public void Import(string rawData)
    {
        Console.WriteLine($"--- {GetType().Name} ---");
        var rows = ReadRecords(rawData);
        Console.WriteLine($"Read {rows.Count} row(s)");

        if (!Validate(rows))
        {
            Console.WriteLine("Validation failed - aborting import.");
            return;
        }

        var transformed = Transform(rows);
        Save(transformed);
    }

    protected abstract List<ImportedRow> ReadRecords(string rawData);

    protected virtual bool Validate(List<ImportedRow> rows) =>
        rows.Count > 0 && rows.All(r => !string.IsNullOrWhiteSpace(r.Email));

    protected abstract List<ImportedRow> Transform(List<ImportedRow> rows);

    private void Save(List<ImportedRow> rows)
    {
        Console.WriteLine($"Saving {rows.Count} row(s):");
        foreach (var row in rows)
        {
            Console.WriteLine($"  {row.Name} <{row.Email}>");
        }
    }
}

public sealed class CsvDataImporter : DataImporter
{
    protected override List<ImportedRow> ReadRecords(string rawData)
    {
        return rawData
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(line =>
            {
                var parts = line.Split(',', 2);
                var name = parts[0];
                var email = parts.Length > 1 ? parts[1] : string.Empty;
                return new ImportedRow(name, email);
            })
            .ToList();
    }

    protected override List<ImportedRow> Transform(List<ImportedRow> rows) =>
        rows.Select(r => r with { Email = r.Email.ToLowerInvariant() }).ToList();
}

public sealed class JsonDataImporter : DataImporter
{
    protected override List<ImportedRow> ReadRecords(string rawData) =>
        JsonSerializer.Deserialize<List<ImportedRow>>(rawData) ?? [];

    // Identical logic to CsvDataImporter.Transform - proof that the format
    // difference lives entirely in ReadRecords, not in every step.
    protected override List<ImportedRow> Transform(List<ImportedRow> rows) =>
        rows.Select(r => r with { Email = r.Email.ToLowerInvariant() }).ToList();

    public static void Demo()
    {
        var csvImporter = new CsvDataImporter();
        csvImporter.Import("Alice,ALICE@EXAMPLE.COM\nBob,Bob@Example.com");

        Console.WriteLine();
        var jsonImporter = new JsonDataImporter();
        jsonImporter.Import("""[{"Name":"Carol","Email":"CAROL@EXAMPLE.COM"}]""");

        Console.WriteLine();
        Console.WriteLine("Importing CSV with a malformed row (no email):");
        var badImporter = new CsvDataImporter();
        // Both importers ran through the exact same Import() skeleton -
        // only this one fails Validate() because ReadRecords produced an
        // empty Email, showing the shared step catches format-specific bad data.
        badImporter.Import("NoEmailHere");
    }
}
