using System;
using System.Collections.Generic;

namespace Runner;

internal static class Program
{
    private sealed record MenuEntry(int Number, string Name, string Summary, Action Demo);

    private static void Main()
    {
        var entries = new List<MenuEntry>
        {
            new(1, "Singleton", "Guarantee a single, globally reachable instance of something that must not be duplicated.", Patterns.Singleton.Solution.SaveManager.Demo),
            new(2, "Factory Method", "Let subclasses/callers decide which concrete type to instantiate behind a common interface.", Patterns.FactoryMethod.Solution.NotificationService.Demo),
            new(3, "Builder", "Construct a complex object step by step, keeping optional parts out of a giant constructor.", Patterns.Builder.Solution.InvoiceBuilder.Demo),
            new(4, "Adapter", "Make an existing interface work with client code that expects a different one.", Patterns.Adapter.Solution.XmlGatewayPaymentAdapter.Demo),
            new(5, "Decorator", "Attach behavior to an object dynamically without touching its class or its siblings.", Patterns.Decorator.Solution.ExportDemo.Demo),
            new(6, "Facade", "Offer one simple entry point over a set of complex, interdependent subsystems.", Patterns.Facade.Solution.CheckoutFacade.Demo),
            new(7, "Proxy", "Control access to an expensive or sensitive object via a stand-in with the same interface.", Patterns.Proxy.Solution.CachingVideoServiceProxy.Demo),
            new(8, "Composite", "Treat individual objects and groups of objects through the same interface.", Patterns.Composite.Solution.Squad.Demo),
            new(9, "Observer", "Notify a dynamic set of interested parties whenever an object's state changes.", Patterns.Observer.Solution.AnalyticsObserver.Demo),
            new(10, "Strategy", "Swap an algorithm's implementation at runtime without changing the code that uses it.", Patterns.Strategy.Solution.Reference.Demo),
            new(11, "Command", "Turn a request into an object so it can be queued, logged, or undone.", Patterns.Command.Solution.Reference.Demo),
            new(12, "Iterator", "Traverse a collection's elements without exposing how it's stored internally.", Patterns.Iterator.Solution.Reference.Demo),
            new(13, "State", "Let an object change its behavior when its internal state changes, without giant if/switch blocks.", Patterns.State.Solution.Reference.Demo),
            new(14, "Template Method", "Fix the skeleton of an algorithm in a base class, letting subclasses fill in the steps.", Patterns.TemplateMethod.Solution.Reference.Demo),
            new(15, "Chain of Responsibility", "Pass a request along a chain of handlers until one of them handles it.", Patterns.ChainOfResponsibility.Solution.Reference.Demo),
        };

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("=== PatternPlayground ===");
            foreach (var entry in entries)
            {
                Console.WriteLine($"{entry.Number,2}. {entry.Name}");
            }
            Console.WriteLine(" 0. Exit");
            Console.Write("Pick a pattern: ");

            var input = Console.ReadLine();
            if (input is null || input.Trim() == "0")
            {
                break;
            }

            if (!int.TryParse(input.Trim(), out var choice))
            {
                Console.WriteLine("Not a number, try again.");
                continue;
            }

            var match = entries.Find(e => e.Number == choice);
            if (match is null)
            {
                Console.WriteLine("No pattern with that number.");
                continue;
            }

            Console.WriteLine();
            Console.WriteLine($"--- {match.Name} ---");
            Console.WriteLine($"When to use: {match.Summary}");
            Console.WriteLine();

            try
            {
                match.Demo();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[demo threw: {ex.GetType().Name}: {ex.Message}]");
            }
        }

        Console.WriteLine("Bye.");
    }
}
