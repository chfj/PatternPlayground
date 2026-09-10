using Patterns.Adapter.Task;

namespace Tests;

public class AdapterTests
{
    [Fact]
    public void Charge_WithPositiveAmount_ReturnsSuccessAndAReference()
    {
        IPaymentProcessor processor = new XmlGatewayPaymentAdapter(new LegacyXmlPaymentGateway());

        var result = processor.Charge("cust-42", 19.99m);

        Assert.True(result.Success);
        Assert.Equal("TXN-cust-42-1999", result.Reference);
    }

    [Fact]
    public void Charge_WithZeroAmount_ReturnsDeclined()
    {
        IPaymentProcessor processor = new XmlGatewayPaymentAdapter(new LegacyXmlPaymentGateway());

        var result = processor.Charge("cust-99", 0m);

        Assert.False(result.Success);
        Assert.Equal("N/A", result.Reference);
    }

    [Fact]
    public void Charge_BuildsARequestTheLegacyGatewayCanParseWithoutThrowing()
    {
        // If the adapter builds malformed XML, the legacy gateway's
        // XElement.Parse call inside SubmitPaymentXml throws - so simply
        // not throwing here is itself a meaningful assertion that the
        // adapter built a well-formed request.
        IPaymentProcessor processor = new XmlGatewayPaymentAdapter(new LegacyXmlPaymentGateway());

        var exception = Record.Exception(() => processor.Charge("cust-1", 5m));

        Assert.Null(exception);
    }
}
