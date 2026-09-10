using Patterns.Decorator.Task;

namespace Tests;

public class DecoratorTests
{
    private static readonly string[][] SampleRows =
    [
        ["Name", "Amount"],
        ["Widget", "19.99"],
    ];

    [Fact]
    public void CsvFileExporter_Export_JoinsCellsWithCommasAndRowsWithNewlines()
    {
        var exporter = new CsvFileExporter();

        var result = exporter.Export(SampleRows);

        Assert.Equal("Name,Amount\nWidget,19.99", result);
    }

    [Fact]
    public void CompressionDecorator_WrapsTheInnerExportersOutput()
    {
        IFileExporter exporter = new CompressionDecorator(new CsvFileExporter());

        var result = exporter.Export(SampleRows);

        Assert.StartsWith("COMPRESSED[", result);
        Assert.Contains("Name,Amount", result);
    }

    [Fact]
    public void EncryptionDecorator_WrapsTheInnerExportersOutput()
    {
        IFileExporter exporter = new EncryptionDecorator(new CsvFileExporter());

        var result = exporter.Export(SampleRows);

        Assert.StartsWith("ENCRYPTED[", result);
        Assert.Contains("Name,Amount", result);
    }

    [Fact]
    public void Decorators_CanBeStackedInAnyCombination()
    {
        IFileExporter exporter = new EncryptionDecorator(new CompressionDecorator(new CsvFileExporter()));

        var result = exporter.Export(SampleRows);

        Assert.StartsWith("ENCRYPTED[COMPRESSED[", result);
    }

    [Fact]
    public void AuditLogDecorator_ReturnsContentUnchanged()
    {
        IFileExporter plain = new CsvFileExporter();
        IFileExporter audited = new AuditLogDecorator(new CsvFileExporter());

        Assert.Equal(plain.Export(SampleRows), audited.Export(SampleRows));
    }

    [Fact]
    public void AuditLogDecorator_LogsASideEffect()
    {
        IFileExporter audited = new AuditLogDecorator(new CsvFileExporter());

        var output = ConsoleCapture.Capture(() => audited.Export(SampleRows));

        Assert.NotEmpty(output);
    }
}
