using System;
using System.Collections.Generic;
using System.Linq;

namespace Patterns.Decorator.Solution;

public interface IFileExporter
{
    string Export(IReadOnlyList<string[]> rows);
}

public sealed class CsvFileExporter : IFileExporter
{
    public string Export(IReadOnlyList<string[]> rows) => string.Join('\n',
        rows.Select(row => string.Join(',', row)));
}

// The base decorator implements the SAME interface as the thing it wraps
// (IFileExporter) and holds a reference to the wrapped instance. That's
// what lets decorators stack: each one is itself a valid IFileExporter, so
// it can wrap another decorator just as easily as it wraps the base
// CsvFileExporter.
public abstract class FileExporterDecorator(IFileExporter inner) : IFileExporter
{
    protected IFileExporter Inner { get; } = inner;

    public abstract string Export(IReadOnlyList<string[]> rows);
}

public sealed class CompressionDecorator(IFileExporter inner) : FileExporterDecorator(inner)
{
    public override string Export(IReadOnlyList<string[]> rows)
    {
        // Delegates to whatever it's wrapping first, then adds its own
        // behavior on top of the result — it never needs to know whether
        // Inner is the raw CsvFileExporter or another decorator.
        var innerResult = Inner.Export(rows);
        return $"COMPRESSED[{innerResult}]";
    }
}

public sealed class EncryptionDecorator(IFileExporter inner) : FileExporterDecorator(inner)
{
    public override string Export(IReadOnlyList<string[]> rows)
    {
        var innerResult = Inner.Export(rows);
        return $"ENCRYPTED[{innerResult}]";
    }
}

public sealed class AuditLogDecorator(IFileExporter inner) : FileExporterDecorator(inner)
{
    public override string Export(IReadOnlyList<string[]> rows)
    {
        // Proof a decorator's "extra behavior" doesn't have to transform
        // the content at all — a pure side effect wrapped around the same
        // call is just as valid a decorator.
        Console.WriteLine($"[audit] export of {rows.Count} row(s) requested");
        return Inner.Export(rows);
    }
}

public static class ExportDemo
{
    public static void Demo()
    {
        string[][] rows =
        [
            ["Name", "Amount"],
            ["Widget", "19.99"],
            ["Gadget", "42.00"],
        ];

        IFileExporter plain = new CsvFileExporter();
        Console.WriteLine("Plain CSV:");
        Console.WriteLine(plain.Export(rows));

        IFileExporter secureExport = new EncryptionDecorator(new CompressionDecorator(new CsvFileExporter()));
        Console.WriteLine("\nCompressed + encrypted:");
        Console.WriteLine(secureExport.Export(rows));

        IFileExporter auditedExport = new AuditLogDecorator(new CsvFileExporter());
        Console.WriteLine("\nAudited (plain content, but logs a side effect):");
        Console.WriteLine(auditedExport.Export(rows));
    }
}
