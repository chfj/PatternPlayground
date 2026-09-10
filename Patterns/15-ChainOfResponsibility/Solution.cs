using System;

namespace Patterns.ChainOfResponsibility.Solution;

public enum TicketSeverity { Low, Medium, High }

public sealed record SupportTicket(string Id, string Description, TicketSeverity Severity);

// Each handler only needs to know about the NEXT link in the chain, not
// the whole chain's shape - that's what lets callers rewire the chain
// (reorder, insert, remove tiers) without touching this class at all.
public abstract class SupportHandler
{
    private SupportHandler? _next;

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

        if (_next is not null)
        {
            _next.Handle(ticket);
        }
        else
        {
            Console.WriteLine($"[unhandled] no handler available for ticket {ticket.Id} ({ticket.Severity})");
        }
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
    protected override bool CanHandle(SupportTicket ticket) => ticket.Severity == TicketSeverity.Medium;

    protected override void Resolve(SupportTicket ticket) =>
        Console.WriteLine($"[L2] resolved ticket {ticket.Id}: {ticket.Description}");
}

public sealed class Level3Support : SupportHandler
{
    protected override bool CanHandle(SupportTicket ticket) => ticket.Severity == TicketSeverity.High;

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
            // Every ticket is submitted the same way, to the same entry
            // point - level1 escalates whatever it can't handle, and so on
            // down the chain, without the caller ever routing anything.
            level1.Handle(ticket);
        }
    }
}
