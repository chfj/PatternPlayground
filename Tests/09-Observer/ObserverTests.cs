using Patterns.Observer.Task;

namespace Tests;

public class ObserverTests
{
    private sealed class RecordingObserver : IOrderObserver
    {
        public List<(string OrderId, OrderStatus Status)> Received { get; } = [];

        public void OnStatusChanged(string orderId, OrderStatus newStatus) =>
            Received.Add((orderId, newStatus));
    }

    [Fact]
    public void UpdateStatus_NotifiesASubscribedObserver()
    {
        var processor = new OrderProcessor();
        var observer = new RecordingObserver();
        processor.Subscribe(observer);

        processor.UpdateStatus("ORD-1", OrderStatus.Shipped);

        Assert.Single(observer.Received);
        Assert.Equal(("ORD-1", OrderStatus.Shipped), observer.Received[0]);
    }

    [Fact]
    public void UpdateStatus_NotifiesEverySubscribedObserver()
    {
        var processor = new OrderProcessor();
        var first = new RecordingObserver();
        var second = new RecordingObserver();
        processor.Subscribe(first);
        processor.Subscribe(second);

        processor.UpdateStatus("ORD-1", OrderStatus.Placed);

        Assert.Single(first.Received);
        Assert.Single(second.Received);
    }

    [Fact]
    public void UpdateStatus_AfterUnsubscribe_DoesNotNotifyThatObserver()
    {
        var processor = new OrderProcessor();
        var observer = new RecordingObserver();
        processor.Subscribe(observer);
        processor.Unsubscribe(observer);

        processor.UpdateStatus("ORD-1", OrderStatus.Delivered);

        Assert.Empty(observer.Received);
    }

    [Fact]
    public void CustomerSmsObserver_OnStatusChanged_PrintsPhoneNumberOrderAndStatus()
    {
        var observer = new CustomerSmsObserver("+1-555-0100");

        var output = ConsoleCapture.Capture(() => observer.OnStatusChanged("ORD-1", OrderStatus.Shipped));

        Assert.Contains("+1-555-0100", output);
        Assert.Contains("ORD-1", output);
        Assert.Contains("Shipped", output);
    }

    [Fact]
    public void AnalyticsObserver_OnStatusChanged_PrintsOrderIdAndStatus()
    {
        var observer = new AnalyticsObserver();

        var output = ConsoleCapture.Capture(() => observer.OnStatusChanged("ORD-2", OrderStatus.Placed));

        Assert.Contains("ORD-2", output);
        Assert.Contains("Placed", output);
    }
}
