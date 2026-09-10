using System;
using System.Collections.Generic;

namespace Patterns.Observer.Solution;

public enum OrderStatus { Placed, Shipped, Delivered, Cancelled }

public interface IOrderObserver
{
    void OnStatusChanged(string orderId, OrderStatus newStatus);
}

// The "subject": holds a list of subscribers it knows nothing about beyond
// the IOrderObserver interface. It never imports CustomerSmsObserver or
// AnalyticsObserver directly - that's what keeps new listener types from
// requiring any change here.
public sealed class OrderProcessor
{
    private readonly List<IOrderObserver> _observers = [];

    public void Subscribe(IOrderObserver observer) => _observers.Add(observer);

    public void Unsubscribe(IOrderObserver observer) => _observers.Remove(observer);

    public void UpdateStatus(string orderId, OrderStatus newStatus)
    {
        foreach (var observer in _observers)
        {
            observer.OnStatusChanged(orderId, newStatus);
        }
    }
}

public sealed class CustomerSmsObserver(string phoneNumber) : IOrderObserver
{
    public void OnStatusChanged(string orderId, OrderStatus newStatus) =>
        Console.WriteLine($"[SMS to {phoneNumber}] order {orderId} is now {newStatus}");
}

public sealed class AnalyticsObserver : IOrderObserver
{
    public void OnStatusChanged(string orderId, OrderStatus newStatus) =>
        Console.WriteLine($"[Analytics] order {orderId} -> {newStatus}");

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

        // Only AnalyticsObserver is still subscribed, so only it reacts -
        // OrderProcessor didn't need any change to support that.
        processor.UpdateStatus("ORD-1", OrderStatus.Delivered);
        Console.WriteLine("(only analytics should have logged the line above)");
    }
}
