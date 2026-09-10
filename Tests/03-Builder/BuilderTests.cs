using Patterns.Builder.Task;

namespace Tests;

public class BuilderTests
{
    [Fact]
    public void Build_WithCustomerAndLineItems_ProducesCorrectSubtotal()
    {
        var invoice = new InvoiceBuilder()
            .ForCustomer("Acme Corp")
            .AddLineItem("Consulting", 1500m)
            .AddLineItem("Support", 200m)
            .Build();

        Assert.Equal("Acme Corp", invoice.CustomerName);
        Assert.Equal(1700m, invoice.Subtotal);
    }

    [Fact]
    public void Build_WithDiscount_AppliesItToTheTotal()
    {
        var invoice = new InvoiceBuilder()
            .ForCustomer("Acme Corp")
            .AddLineItem("Consulting", 1000m)
            .WithDiscount(10m)
            .Build();

        Assert.Equal(900m, invoice.Total);
    }

    [Fact]
    public void Build_WithoutDiscount_TotalEqualsSubtotal()
    {
        var invoice = new InvoiceBuilder()
            .ForCustomer("Small Client")
            .AddLineItem("Fix", 75m)
            .Build();

        Assert.Equal(75m, invoice.Total);
    }

    [Fact]
    public void Build_WithFooterNote_IsIncludedOnTheInvoice()
    {
        var invoice = new InvoiceBuilder()
            .ForCustomer("Acme Corp")
            .AddLineItem("Consulting", 100m)
            .WithFooterNote("Thanks!")
            .Build();

        Assert.Equal("Thanks!", invoice.FooterNote);
    }

    [Fact]
    public void Build_WithoutFooterNote_LeavesItNull()
    {
        var invoice = new InvoiceBuilder()
            .ForCustomer("Acme Corp")
            .AddLineItem("Consulting", 100m)
            .Build();

        Assert.Null(invoice.FooterNote);
    }

    [Fact]
    public void Build_WithoutCallingForCustomer_ThrowsInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() =>
            new InvoiceBuilder().AddLineItem("Fix", 75m).Build());
    }

    [Fact]
    public void ForCustomer_ReturnsTheSameBuilderForChaining()
    {
        var builder = new InvoiceBuilder();

        var afterCustomer = builder.ForCustomer("Acme Corp");

        Assert.Same(builder, afterCustomer);
    }
}
