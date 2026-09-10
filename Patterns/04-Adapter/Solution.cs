using System;
using System.Xml.Linq;

namespace Patterns.Adapter.Solution;

public sealed record PaymentResult(bool Success, string Reference);

// This is the interface checkout code depends on. It's ours to design, so
// it's clean and JSON/DTO-shaped — no hint of XML anywhere.
public interface IPaymentProcessor
{
    PaymentResult Charge(string customerId, decimal amount);
}

// Unchangeable "vendor" code: only speaks XML in and XML out.
public sealed class LegacyXmlPaymentGateway
{
    public string SubmitPaymentXml(string xmlRequest)
    {
        var request = XElement.Parse(xmlRequest);
        var customerId = request.Element("CustomerId")!.Value;
        var amount = decimal.Parse(request.Element("Amount")!.Value);

        var approved = amount > 0;
        var response = new XElement("PaymentResponse",
            new XElement("Status", approved ? "OK" : "DECLINED"),
            new XElement("Reference", approved ? $"TXN-{customerId}-{(int)(amount * 100)}" : "N/A"));
        return response.ToString();
    }
}

// The adapter: implements the interface the REST of the app wants
// (IPaymentProcessor) by wrapping an instance of the interface the legacy
// gateway actually offers. All the XML plumbing is contained here — one
// class knows about it, nothing else does.
public sealed class XmlGatewayPaymentAdapter(LegacyXmlPaymentGateway legacyGateway) : IPaymentProcessor
{
    public PaymentResult Charge(string customerId, decimal amount)
    {
        var request = new XElement("PaymentRequest",
            new XElement("CustomerId", customerId),
            new XElement("Amount", amount));

        var responseXml = legacyGateway.SubmitPaymentXml(request.ToString());
        var response = XElement.Parse(responseXml);

        var status = response.Element("Status")!.Value;
        var reference = response.Element("Reference")!.Value;

        return new PaymentResult(Success: status == "OK", Reference: reference);
    }

    public static void Demo()
    {
        // Swapping the legacy gateway for a different vendor later only
        // means writing a new adapter — this line, and everything above it
        // conceptually, never changes.
        IPaymentProcessor processor = new XmlGatewayPaymentAdapter(new LegacyXmlPaymentGateway());

        var success = processor.Charge("cust-42", 19.99m);
        Console.WriteLine($"Charge cust-42 $19.99 -> success={success.Success}, ref={success.Reference}");

        var declined = processor.Charge("cust-99", 0m);
        Console.WriteLine($"Charge cust-99 $0.00 -> success={declined.Success}, ref={declined.Reference}");
    }
}
