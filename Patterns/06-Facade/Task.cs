using System;
using System.Collections.Generic;

namespace Patterns.Facade.Task;

// Scenario: placing an order touches four independent subsystems, in a
// specific order: reserve stock, charge payment, book shipping, send a
// confirmation. Every one of those subsystems has its own API. Build a
// CheckoutFacade that gives callers one PlaceOrder() method instead of
// making every caller re-learn and re-sequence all four subsystems.

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
        return amount > 0; // simulate declines for zero/invalid amounts
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

public sealed class CheckoutFacade(
    InventoryService inventory,
    PaymentService payment,
    ShippingService shipping,
    NotificationService notifications)
{
    // TODO: implement the full checkout workflow:
    //  1. Reserve inventory for `sku`/`quantity`. If unavailable, return false immediately.
    //  2. Charge `amount` to `customerId`. If declined, return false (inventory is
    //     already reserved in this simplified version — that's fine for the exercise).
    //  3. Book shipping and capture the tracking number.
    //  4. Send a confirmation notification with that tracking number.
    //  5. Return true.
    public bool PlaceOrder(string customerId, string sku, int quantity, decimal amount)
    {
        throw new NotImplementedException();
    }

    public static void Demo()
    {
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
