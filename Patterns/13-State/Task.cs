using System;

namespace Patterns.State.Task;

// Scenario: an order moves through Placed -> Paid -> Shipped -> Delivered,
// or can be Cancelled from Placed or Paid (but not after it's shipped).
// Each state object knows which transitions are legal FROM that state and
// returns the next state; Order itself just holds "whatever the current
// state object is" and delegates to it.

public interface IOrderState
{
    string Name { get; }
    IOrderState Pay(Order order);
    IOrderState Ship(Order order);
    IOrderState Deliver(Order order);
    IOrderState Cancel(Order order);
}

// Default behavior for every transition is "not allowed from here" -
// concrete states only need to override the transitions that ARE legal.
public abstract class OrderStateBase : IOrderState
{
    public abstract string Name { get; }

    public virtual IOrderState Pay(Order order) => Reject(order, nameof(Pay));
    public virtual IOrderState Ship(Order order) => Reject(order, nameof(Ship));
    public virtual IOrderState Deliver(Order order) => Reject(order, nameof(Deliver));
    public virtual IOrderState Cancel(Order order) => Reject(order, nameof(Cancel));

    protected IOrderState Reject(Order order, string action)
    {
        Console.WriteLine($"  [rejected] cannot {action} order {order.Id} while it's {Name}");
        return this;
    }
}

public sealed class PlacedState : OrderStateBase
{
    public override string Name => "Placed";

    // TODO: Pay() should move to PaidState (print a transition message and
    // return `new PaidState()`).
    public override IOrderState Pay(Order order)
    {
        throw new NotImplementedException();
    }

    // TODO: Cancel() should move to CancelledState.
    public override IOrderState Cancel(Order order)
    {
        throw new NotImplementedException();
    }
}

public sealed class PaidState : OrderStateBase
{
    public override string Name => "Paid";

    // TODO: Ship() should move to ShippedState.
    public override IOrderState Ship(Order order)
    {
        throw new NotImplementedException();
    }

    // TODO: Cancel() should move to CancelledState (you can still cancel
    // after paying, just not after shipping).
    public override IOrderState Cancel(Order order)
    {
        throw new NotImplementedException();
    }
}

public sealed class ShippedState : OrderStateBase
{
    public override string Name => "Shipped";

    // TODO: Deliver() should move to DeliveredState.
    public override IOrderState Deliver(Order order)
    {
        throw new NotImplementedException();
    }
}

// Terminal states: every transition is illegal, so the base class's
// default "reject" behavior is exactly right with no overrides needed.
public sealed class DeliveredState : OrderStateBase
{
    public override string Name => "Delivered";
}

public sealed class CancelledState : OrderStateBase
{
    public override string Name => "Cancelled";
}

public sealed class Order(string id)
{
    public string Id { get; } = id;
    public IOrderState State { get; private set; } = new PlacedState();

    public void Pay() => State = State.Pay(this);
    public void Ship() => State = State.Ship(this);
    public void Deliver() => State = State.Deliver(this);
    public void Cancel() => State = State.Cancel(this);

    public static void Demo()
    {
        var order = new Order("ORD-77");
        Console.WriteLine($"Order {order.Id} starts as {order.State.Name}");

        order.Pay();
        Console.WriteLine($"After Pay(): {order.State.Name}");

        order.Ship();
        Console.WriteLine($"After Ship(): {order.State.Name}");

        Console.WriteLine("Trying to cancel a shipped order (should be rejected):");
        order.Cancel();
        Console.WriteLine($"State is still: {order.State.Name}");

        order.Deliver();
        Console.WriteLine($"After Deliver(): {order.State.Name}");
    }
}
