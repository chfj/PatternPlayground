using System;
using System.Collections.Generic;

namespace Patterns.Observer.Task;

// Scenario: an OrderProcessor updates an order's status over its lifetime
// (Placed -> Shipped -> Delivered). A customer wants an SMS on every status
// change; an analytics service wants to log every change too. Neither
// listener should be hard-coded into OrderProcessor - they subscribe, and
// OrderProcessor just broadcasts.

public enum OrderStatus { Placed, Shipped, Delivered, Cancelled }

public interface IOrderObserver
{
    void OnStatusChanged(string orderId, OrderStatus newStatus);
}

public sealed class OrderProcessor
{
    private readonly List<IOrderObserver> _observers = [];

    public void Subscribe(IOrderObserver observer) => _observers.Add(observer);

    public void Unsubscribe(IOrderObserver observer) => _observers.Remove(observer);

    // TODO: notify every currently-subscribed observer that `orderId`
    // changed to `newStatus` (call OnStatusChanged on each one).
    public void UpdateStatus(string orderId, OrderStatus newStatus)
    {
        throw new NotImplementedException();
    }
}

public sealed class CustomerSmsObserver(string phoneNumber) : IOrderObserver
{
    // TODO: print something like "[SMS to <phoneNumber>] order <orderId> is now <newStatus>"
    public void OnStatusChanged(string orderId, OrderStatus newStatus)
    {
        throw new NotImplementedException();
    }
}

public sealed class AnalyticsObserver : IOrderObserver
{
    // TODO: print something like "[Analytics] order <orderId> -> <newStatus>"
    public void OnStatusChanged(string orderId, OrderStatus newStatus)
    {
        throw new NotImplementedException();
    }

    public static void Demo()
    {
        var processor = new OrderProcessor();
        var customerObserver = new CustomerSmsObserver("+1-555-0100");
        var analyticsObserver = new AnalyticsObserver();

        processor.Subscribe(customerObserver);
        processor.Subscribe(analyticsObserver);

        processor.UpdateStatus("ORD-1", OrderStatus.Placed);
        processor.UpdateStatus("ORD-1", OrderStatus.Shipped);

        Console.WriteLine("\nCustomer unsubscribes (maybe they closed the app)...");
        processor.Unsubscribe(customerObserver);

        processor.UpdateStatus("ORD-1", OrderStatus.Delivered);
        Console.WriteLine("(only analytics should have logged the line above)");
    }
}
