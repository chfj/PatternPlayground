namespace Tests;

// Shared helper for tests whose subject only communicates by printing to
// the console (no return value to assert on directly) - captures
// everything written during `action` and hands it back as a string.
internal static class ConsoleCapture
{
    public static string Capture(Action action)
    {
        var originalOut = Console.Out;
        var writer = new StringWriter();
        Console.SetOut(writer);
        try
        {
            action();
        }
        finally
        {
            Console.SetOut(originalOut);
        }

        return writer.ToString();
    }
}
