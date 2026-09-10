using System;
using System.Collections.Generic;

namespace Patterns.Decorator.Task;

// Scenario: a reporting tool exports tabular data to CSV. Different jobs
// need different optional post-processing on top of that base export:
// compress it, encrypt it, audit-log that an export happened — in any
// combination, chosen at runtime. Model each optional behavior as a
// decorator wrapping IFileExporter instead of writing one subclass per
// combination.

public interface IFileExporter
{
    string Export(IReadOnlyList<string[]> rows);
}

public sealed class CsvFileExporter : IFileExporter
{
    // TODO: join each row's cells with commas, and rows with newlines.
    // e.g. [["a","b"],["1","2"]] -> "a,b\n1,2"
    public string Export(IReadOnlyList<string[]> rows)
    {
        throw new NotImplementedException();
    }
}

// A convenient base for decorators: holds the wrapped exporter so concrete
// decorators only need to say what THEY add.
public abstract class FileExporterDecorator(IFileExporter inner) : IFileExporter
{
    protected IFileExporter Inner { get; } = inner;

    public abstract string Export(IReadOnlyList<string[]> rows);
}

public sealed class CompressionDecorator(IFileExporter inner) : FileExporterDecorator(inner)
{
    // TODO: get the inner exporter's output, then wrap it to simulate
    // compression, e.g. return $"COMPRESSED[{innerResult}]"
    public override string Export(IReadOnlyList<string[]> rows)
    {
        throw new NotImplementedException();
    }
}

public sealed class EncryptionDecorator(IFileExporter inner) : FileExporterDecorator(inner)
{
    // TODO: get the inner exporter's output, then wrap it to simulate
    // encryption, e.g. return $"ENCRYPTED[{innerResult}]"
    public override string Export(IReadOnlyList<string[]> rows)
    {
        throw new NotImplementedException();
    }
}

public sealed class AuditLogDecorator(IFileExporter inner) : FileExporterDecorator(inner)
{
    // TODO: print an audit line (e.g. to Console) noting an export
    // happened, THEN return the inner exporter's output unchanged. This
    // decorator adds a side effect without altering the actual content.
    public override string Export(IReadOnlyList<string[]> rows)
    {
        throw new NotImplementedException();
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

        // Layer decorators in whatever combination this job needs, all
        // built from the exact same base exporter.
        IFileExporter secureExport = new EncryptionDecorator(new CompressionDecorator(new CsvFileExporter()));
        Console.WriteLine("\nCompressed + encrypted:");
        Console.WriteLine(secureExport.Export(rows));

        IFileExporter auditedExport = new AuditLogDecorator(new CsvFileExporter());
        Console.WriteLine("\nAudited (plain content, but logs a side effect):");
        Console.WriteLine(auditedExport.Export(rows));
    }
}
