using System;

namespace Patterns.ChainOfResponsibility.Task;

// Scenario: support tickets come in with a severity. Low-severity tickets
// should be handled by Level 1 support; if a ticket is too severe for the
// current handler, it escalates to the next tier. Whoever submits a ticket
// just hands it to the front of the chain and doesn't decide who ultimately
// handles it.

public enum TicketSeverity { Low, Medium, High }

public sealed record SupportTicket(string Id, string Description, TicketSeverity Severity);

public abstract class SupportHandler
{
    private SupportHandler? _next;

    // Wiring the chain together: returns `next` so calls can be chained,
    // e.g. level1.SetNext(level2).SetNext(level3).
    public SupportHandler SetNext(SupportHandler next)
    {
        _next = next;
        return next;
    }

    public void Handle(SupportTicket ticket)
    {
        if (CanHandle(ticket))
        {
            Resolve(ticket);
            return;
        }

        // TODO: if there's a next handler in the chain, hand the ticket to
        // it. If there ISN'T a next handler (this is the end of the
        // chain), print that the ticket went unhandled.
        throw new NotImplementedException();
    }

    protected abstract bool CanHandle(SupportTicket ticket);
    protected abstract void Resolve(SupportTicket ticket);
}

public sealed class Level1Support : SupportHandler
{
    protected override bool CanHandle(SupportTicket ticket) => ticket.Severity == TicketSeverity.Low;

    protected override void Resolve(SupportTicket ticket) =>
        Console.WriteLine($"[L1] resolved ticket {ticket.Id}: {ticket.Description}");
}

public sealed class Level2Support : SupportHandler
{
    // TODO: Level 2 handles Medium severity.
    protected override bool CanHandle(SupportTicket ticket) => throw new NotImplementedException();

    protected override void Resolve(SupportTicket ticket) =>
        Console.WriteLine($"[L2] resolved ticket {ticket.Id}: {ticket.Description}");
}

public sealed class Level3Support : SupportHandler
{
    // TODO: Level 3 handles High severity.
    protected override bool CanHandle(SupportTicket ticket) => throw new NotImplementedException();

    protected override void Resolve(SupportTicket ticket) =>
        Console.WriteLine($"[L3] resolved ticket {ticket.Id}: {ticket.Description}");

    public static void Demo()
    {
        var level1 = new Level1Support();
        var level2 = new Level2Support();
        var level3 = new Level3Support();
        level1.SetNext(level2).SetNext(level3);

        SupportTicket[] tickets =
        [
            new("T-1", "Forgot password", TicketSeverity.Low),
            new("T-2", "App crashes on startup", TicketSeverity.Medium),
            new("T-3", "Data loss in production", TicketSeverity.High),
        ];

        foreach (var ticket in tickets)
        {
            // Every ticket enters the chain at the SAME point (level1) -
            // where it actually gets resolved depends entirely on the chain.
            level1.Handle(ticket);
        }
    }
}
