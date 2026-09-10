using System;
using System.Collections.Generic;
using System.Linq;

namespace Patterns.Builder.Task;

// Scenario: an invoicing tool needs to assemble invoices with a required
// customer name and line items, plus OPTIONAL parts: a percentage discount,
// and a footer note. Build this step by step with a fluent builder instead
// of one constructor with a pile of optional parameters.

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

public sealed class InvoiceBuilder
{
    private string? _customerName;
    private readonly List<InvoiceLineItem> _lineItems = [];
    private decimal _discountPercent;
    private string? _footerNote;

    // TODO: store the customer name and return `this` so calls can chain.
    public InvoiceBuilder ForCustomer(string customerName)
    {
        throw new NotImplementedException();
    }

    // TODO: append a line item and return `this`.
    public InvoiceBuilder AddLineItem(string description, decimal amount)
    {
        throw new NotImplementedException();
    }

    // TODO: record the discount percentage (optional step) and return `this`.
    public InvoiceBuilder WithDiscount(decimal percent)
    {
        throw new NotImplementedException();
    }

    // TODO: record a footer note (optional step) and return `this`.
    public InvoiceBuilder WithFooterNote(string note)
    {
        throw new NotImplementedException();
    }

    // TODO: assemble and return the finished, immutable Invoice from
    // whatever steps were called above. Throw InvalidOperationException if
    // ForCustomer was never called.
    public Invoice Build()
    {
        throw new NotImplementedException();
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

        var minimalInvoice = new InvoiceBuilder()
            .ForCustomer("Small Client LLC")
            .AddLineItem("One-off fix", 75m)
            .Build();
        minimalInvoice.Print();
    }
}
