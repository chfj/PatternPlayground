using System;
using System.Collections.Generic;
using System.Linq;

namespace Patterns.Builder.Solution;

public sealed record InvoiceLineItem(string Description, decimal Amount);

public sealed class Invoice
{
    public required string CustomerName { get; init; }
    public required IReadOnlyList<InvoiceLineItem> LineItems { get; init; }
    public decimal DiscountPercent { get; init; }
    public string? FooterNote { get; init; }

    public decimal Subtotal => LineItems.Sum(item => item.Amount);
    public decimal Total => Subtotal * (1 - DiscountPercent / 100m);

    public void Print()
    {
        Console.WriteLine($"Invoice for {CustomerName}");
        foreach (var item in LineItems)
        {
            Console.WriteLine($"  - {item.Description}: {item.Amount:C}");
        }
        if (DiscountPercent > 0)
        {
            Console.WriteLine($"  Discount: {DiscountPercent}%");
        }
        Console.WriteLine($"  Total: {Total:C}");
        if (FooterNote is not null)
        {
            Console.WriteLine($"  Note: {FooterNote}");
        }
    }
}

// The builder accumulates mutable state across several fluent calls, then
// produces one immutable Invoice at the end. Splitting "assembly" (mutable,
// step-by-step) from "the finished thing" (immutable record-like class)
// means the Invoice itself never has to worry about being constructed in a
// half-finished state.
public sealed class InvoiceBuilder
{
    private string? _customerName;
    private readonly List<InvoiceLineItem> _lineItems = [];
    private decimal _discountPercent;
    private string? _footerNote;

    public InvoiceBuilder ForCustomer(string customerName)
    {
        _customerName = customerName;
        return this; // returning `this` is what enables the fluent chain
    }

    public InvoiceBuilder AddLineItem(string description, decimal amount)
    {
        _lineItems.Add(new InvoiceLineItem(description, amount));
        return this;
    }

    public InvoiceBuilder WithDiscount(decimal percent)
    {
        _discountPercent = percent;
        return this;
    }

    public InvoiceBuilder WithFooterNote(string note)
    {
        _footerNote = note;
        return this;
    }

    public Invoice Build()
    {
        if (_customerName is null)
        {
            throw new InvalidOperationException($"{nameof(ForCustomer)} must be called before {nameof(Build)}.");
        }

        // Optional steps (discount, footer) simply default to "no effect"
        // if the caller never invoked them — no null-heavy constructor
        // arguments required at the call site.
        return new Invoice
        {
            CustomerName = _customerName,
            LineItems = _lineItems.AsReadOnly(),
            DiscountPercent = _discountPercent,
            FooterNote = _footerNote,
        };
    }

    public static void Demo()
    {
        var fullInvoice = new InvoiceBuilder()
            .ForCustomer("Acme Corp")
            .AddLineItem("Consulting (10h)", 1500m)
            .AddLineItem("Support plan", 200m)
            .WithDiscount(10m)
            .WithFooterNote("Thanks for your business!")
            .Build();
        fullInvoice.Print();

        Console.WriteLine();

        // Same builder process, but only the required steps are used —
        // that's the payoff of pulling optional parts out of a constructor.
        var minimalInvoice = new InvoiceBuilder()
            .ForCustomer("Small Client LLC")
            .AddLineItem("One-off fix", 75m)
            .Build();
        minimalInvoice.Print();
    }
}
