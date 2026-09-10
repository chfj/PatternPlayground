using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Patterns.TemplateMethod.Task;

// Scenario: a data-import tool needs to support both CSV and JSON sources,
// but every import - regardless of source format - follows the same four
// steps: read raw data into rows, validate those rows, transform them
// (normalize emails), then save. Put that fixed skeleton in a base class
// and let each format only supply the format-specific steps.

public sealed record ImportedRow(string Name, string Email);

public abstract class DataImporter
{
    // The "template method": defines the fixed step order. Not overridden
    // by subclasses - they only plug into the abstract/virtual steps below.
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

    // A "hook": has a sensible default, but subclasses COULD override it.
    // Neither CSV nor JSON needs to here, which is fine - not every hook
    // needs overriding.
    protected virtual bool Validate(List<ImportedRow> rows) =>
        rows.Count > 0 && rows.All(r => !string.IsNullOrWhiteSpace(r.Email));

    protected abstract List<ImportedRow> Transform(List<ImportedRow> rows);

    // Shared, non-abstract step: every format saves the same way, so this
    // isn't exposed as something subclasses can vary.
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
    // TODO: split rawData into lines, split each line on the first comma
    // into (name, email). A line with no comma should produce an empty
    // email (don't throw) - that's what makes the "bad data" demo below
    // fail validation instead of crashing.
    protected override List<ImportedRow> ReadRecords(string rawData)
    {
        throw new NotImplementedException();
    }

    // TODO: return a new list where every row's Email is lower-cased.
    protected override List<ImportedRow> Transform(List<ImportedRow> rows)
    {
        throw new NotImplementedException();
    }
}

public sealed class JsonDataImporter : DataImporter
{
    // TODO: parse rawData as JSON using
    // JsonSerializer.Deserialize<List<ImportedRow>>(rawData) - the JSON is
    // an array of {"Name": "...", "Email": "..."} objects.
    protected override List<ImportedRow> ReadRecords(string rawData)
    {
        throw new NotImplementedException();
    }

    // TODO: same normalization as CSV - lower-case every row's Email.
    protected override List<ImportedRow> Transform(List<ImportedRow> rows)
    {
        throw new NotImplementedException();
    }

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
        badImporter.Import("NoEmailHere");
    }
}
