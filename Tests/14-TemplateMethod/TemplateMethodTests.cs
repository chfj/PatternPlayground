using Patterns.TemplateMethod.Task;

namespace Tests;

public class TemplateMethodTests
{
    [Fact]
    public void CsvDataImporter_Import_LowercasesEmailsAndPrintsEachRow()
    {
        var importer = new CsvDataImporter();

        var output = ConsoleCapture.Capture(() =>
            importer.Import("Alice,ALICE@EXAMPLE.COM\nBob,Bob@Example.com"));

        Assert.Contains("Alice <alice@example.com>", output);
        Assert.Contains("Bob <bob@example.com>", output);
    }

    [Fact]
    public void CsvDataImporter_Import_ReadsTheCorrectRowCount()
    {
        var importer = new CsvDataImporter();

        var output = ConsoleCapture.Capture(() =>
            importer.Import("Alice,alice@example.com\nBob,bob@example.com\nCarol,carol@example.com"));

        Assert.Contains("Read 3 row(s)", output);
    }

    [Fact]
    public void CsvDataImporter_Import_WithNoCommaInARow_FailsValidationInsteadOfCrashing()
    {
        var importer = new CsvDataImporter();

        var output = ConsoleCapture.Capture(() => importer.Import("NoEmailHere"));

        Assert.Contains("Validation failed", output);
    }

    [Fact]
    public void JsonDataImporter_Import_ParsesJsonAndLowercasesEmails()
    {
        var importer = new JsonDataImporter();

        var output = ConsoleCapture.Capture(() =>
            importer.Import("""[{"Name":"Carol","Email":"CAROL@EXAMPLE.COM"}]"""));

        Assert.Contains("Carol <carol@example.com>", output);
    }
}
