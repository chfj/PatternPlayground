using Patterns.State.Task;

namespace Tests;

public class StateTests
{
    [Fact]
    public void NewOrder_StartsInPlacedState()
    {
        var order = new Order("ORD-1");

        Assert.Equal("Placed", order.State.Name);
    }

    [Fact]
    public void Pay_MovesFromPlacedToPaid()
    {
        var order = new Order("ORD-1");

        order.Pay();

        Assert.Equal("Paid", order.State.Name);
    }

    [Fact]
    public void Ship_MovesFromPaidToShipped()
    {
        var order = new Order("ORD-1");
        order.Pay();

        order.Ship();

        Assert.Equal("Shipped", order.State.Name);
    }

    [Fact]
    public void Deliver_MovesFromShippedToDelivered()
    {
        var order = new Order("ORD-1");
        order.Pay();
        order.Ship();

        order.Deliver();

        Assert.Equal("Delivered", order.State.Name);
    }

    [Fact]
    public void Cancel_FromPlaced_MovesToCancelled()
    {
        var order = new Order("ORD-1");

        order.Cancel();

        Assert.Equal("Cancelled", order.State.Name);
    }

    [Fact]
    public void Cancel_FromPaid_MovesToCancelled()
    {
        var order = new Order("ORD-1");
        order.Pay();

        order.Cancel();

        Assert.Equal("Cancelled", order.State.Name);
    }

    [Fact]
    public void Cancel_AfterShipped_IsRejectedAndStateStaysShipped()
    {
        var order = new Order("ORD-1");
        order.Pay();
        order.Ship();

        order.Cancel();

        Assert.Equal("Shipped", order.State.Name);
    }

    [Fact]
    public void Ship_WithoutPayingFirst_IsRejectedAndStateStaysPlaced()
    {
        var order = new Order("ORD-1");

        order.Ship();

        Assert.Equal("Placed", order.State.Name);
    }
}
