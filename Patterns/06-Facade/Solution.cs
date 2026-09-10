using System;
using System.Collections.Generic;

namespace Patterns.Facade.Solution;

public sealed class InventoryService
{
    private readonly Dictionary<string, int> _stock = new()
    {
        ["WIDGET"] = 5,
        ["GADGET"] = 0,
    };

    public bool Reserve(string sku, int quantity)
    {
        if (!_stock.TryGetValue(sku, out var available) || available < quantity)
        {
            Console.WriteLine($"[Inventory] cannot reserve {quantity}x {sku} (have {(_stock.GetValueOrDefault(sku))})");
            return false;
        }
        _stock[sku] = available - quantity;
        Console.WriteLine($"[Inventory] reserved {quantity}x {sku}");
        return true;
    }
}

public sealed class PaymentService
{
    public bool Charge(string customerId, decimal amount)
    {
        Console.WriteLine($"[Payment] charging {customerId} {amount:C}");
        return amount > 0;
    }
}

public sealed class ShippingService
{
    public string Book(string customerId, string sku, int quantity)
    {
        var tracking = $"TRACK-{customerId}-{sku}";
        Console.WriteLine($"[Shipping] booked shipment {tracking} ({quantity}x {sku})");
        return tracking;
    }
}

public sealed class NotificationService
{
    public void SendConfirmation(string customerId, string trackingNumber)
    {
        Console.WriteLine($"[Notification] confirmation sent to {customerId}: tracking {trackingNumber}");
    }
}

// The facade doesn't add new capability — every one of these steps was
// already possible by calling the four subsystems directly. What it adds is
// a single, correctly-ordered entry point, so callers can't accidentally
// charge payment before confirming stock, or forget the confirmation step.
public sealed class CheckoutFacade(
    InventoryService inventory,
    PaymentService payment,
    ShippingService shipping,
    NotificationService notifications)
{
    public bool PlaceOrder(string customerId, string sku, int quantity, decimal amount)
    {
        if (!inventory.Reserve(sku, quantity))
        {
            return false;
        }

        if (!payment.Charge(customerId, amount))
        {
            Console.WriteLine("[Checkout] payment declined, aborting order");
            return false;
        }

        var trackingNumber = shipping.Book(customerId, sku, quantity);
        notifications.SendConfirmation(customerId, trackingNumber);
        return true;
    }

    public static void Demo()
    {
        // The caller only ever sees this one line to place an order —
        // never the four-subsystem dance behind it.
        var facade = new CheckoutFacade(
            new InventoryService(),
            new PaymentService(),
            new ShippingService(),
            new NotificationService());

        Console.WriteLine("Order 1: 2x WIDGET for $39.98");
        var ok1 = facade.PlaceOrder("cust-1", "WIDGET", 2, 39.98m);
        Console.WriteLine($"Order 1 result: {(ok1 ? "SUCCESS" : "FAILED")}\n");

        Console.WriteLine("Order 2: 1x GADGET (out of stock)");
        var ok2 = facade.PlaceOrder("cust-2", "GADGET", 1, 15.00m);
        Console.WriteLine($"Order 2 result: {(ok2 ? "SUCCESS" : "FAILED")}");
    }
}
