using Patterns.ChainOfResponsibility.Task;

namespace Tests;

public class ChainOfResponsibilityTests
{
    private static Level1Support BuildFullChain()
    {
        var level1 = new Level1Support();
        var level2 = new Level2Support();
        var level3 = new Level3Support();
        level1.SetNext(level2).SetNext(level3);
        return level1;
    }

    [Fact]
    public void LowSeverityTicket_IsResolvedByLevel1()
    {
        var level1 = BuildFullChain();
        var ticket = new SupportTicket("T-1", "Forgot password", TicketSeverity.Low);

        var output = ConsoleCapture.Capture(() => level1.Handle(ticket));

        Assert.Contains("[L1]", output);
    }

    [Fact]
    public void MediumSeverityTicket_EscalatesPastLevel1ToLevel2()
    {
        var level1 = BuildFullChain();
        var ticket = new SupportTicket("T-2", "App crashes", TicketSeverity.Medium);

        var output = ConsoleCapture.Capture(() => level1.Handle(ticket));

        Assert.Contains("[L2]", output);
        Assert.DoesNotContain("[L1]", output);
    }

    [Fact]
    public void HighSeverityTicket_EscalatesAllTheWayToLevel3()
    {
        var level1 = BuildFullChain();
        var ticket = new SupportTicket("T-3", "Data loss", TicketSeverity.High);

        var output = ConsoleCapture.Capture(() => level1.Handle(ticket));

        Assert.Contains("[L3]", output);
        Assert.DoesNotContain("[L1]", output);
        Assert.DoesNotContain("[L2]", output);
    }

    [Fact]
    public void TicketBeyondTheChainsCapability_GoesUnhandledWithoutThrowing()
    {
        // A chain that stops at Level1 has nowhere to escalate a
        // High-severity ticket to.
        var level1 = new Level1Support();
        var ticket = new SupportTicket("T-9", "Too severe", TicketSeverity.High);

        var exception = Record.Exception(() =>
        {
            var output = ConsoleCapture.Capture(() => level1.Handle(ticket));
            Assert.DoesNotContain("[L1]", output);
        });

        Assert.Null(exception);
    }
}
