using System;
using System.Xml.Linq;

namespace Patterns.Adapter.Task;

// Scenario: the app's checkout code is written against a clean, modern
// IPaymentProcessor interface. But the payment gateway actually available
// is a "legacy" vendor SDK (LegacyXmlPaymentGateway below) that only speaks
// XML requests/responses. You can't change the vendor SDK (pretend it ships
// as a compiled library) and you don't want XML leaking into checkout code.
//
// Your job: write an adapter that implements IPaymentProcessor by
// translating calls into the legacy XML shape and translating the XML
// response back.

public sealed record PaymentResult(bool Success, string Reference);

public interface IPaymentProcessor
{
    PaymentResult Charge(string customerId, decimal amount);
}

// Pretend this class ships in a third-party NuGet package — treat it as
// unchangeable "legacy" code you must adapt to, not rewrite.
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

public sealed class XmlGatewayPaymentAdapter(LegacyXmlPaymentGateway legacyGateway) : IPaymentProcessor
{
    // TODO:
    //  1. Build an XML request the legacy gateway understands:
    //     <PaymentRequest><CustomerId>...</CustomerId><Amount>...</Amount></PaymentRequest>
    //  2. Call legacyGateway.SubmitPaymentXml(...) with it.
    //  3. Parse the XML response and translate it into a PaymentResult
    //     (Success = Status == "OK", Reference = the <Reference> value).
    public PaymentResult Charge(string customerId, decimal amount)
    {
        throw new NotImplementedException();
    }

    public static void Demo()
    {
        // Checkout code only ever talks to IPaymentProcessor — it has no
        // idea the real gateway underneath speaks XML.
        IPaymentProcessor processor = new XmlGatewayPaymentAdapter(new LegacyXmlPaymentGateway());

        var success = processor.Charge("cust-42", 19.99m);
        Console.WriteLine($"Charge cust-42 $19.99 -> success={success.Success}, ref={success.Reference}");

        var declined = processor.Charge("cust-99", 0m);
        Console.WriteLine($"Charge cust-99 $0.00 -> success={declined.Success}, ref={declined.Reference}");
    }
}
