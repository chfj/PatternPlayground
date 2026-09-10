using System;

namespace Patterns.State.Solution;

public interface IOrderState
{
    string Name { get; }
    IOrderState Pay(Order order);
    IOrderState Ship(Order order);
    IOrderState Deliver(Order order);
    IOrderState Cancel(Order order);
}

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

// Each concrete state ONLY overrides the transitions that are legal from
// it - Order.Pay()/Ship()/etc. never has to ask "am I allowed to do this
// right now?" itself. The current state object answers that by which
// methods it overrides.
public sealed class PlacedState : OrderStateBase
{
    public override string Name => "Placed";

    public override IOrderState Pay(Order order)
    {
        Console.WriteLine($"  order {order.Id}: Placed -> Paid");
        return new PaidState();
    }

    public override IOrderState Cancel(Order order)
    {
        Console.WriteLine($"  order {order.Id}: Placed -> Cancelled");
        return new CancelledState();
    }
}

public sealed class PaidState : OrderStateBase
{
    public override string Name => "Paid";

    public override IOrderState Ship(Order order)
    {
        Console.WriteLine($"  order {order.Id}: Paid -> Shipped");
        return new ShippedState();
    }

    public override IOrderState Cancel(Order order)
    {
        Console.WriteLine($"  order {order.Id}: Paid -> Cancelled");
        return new CancelledState();
    }
}

public sealed class ShippedState : OrderStateBase
{
    public override string Name => "Shipped";

    public override IOrderState Deliver(Order order)
    {
        Console.WriteLine($"  order {order.Id}: Shipped -> Delivered");
        return new DeliveredState();
    }
}

public sealed class DeliveredState : OrderStateBase
{
    public override string Name => "Delivered";
}

public sealed class CancelledState : OrderStateBase
{
    public override string Name => "Cancelled";
}

// Order just forwards each action to its current state object and stores
// whatever state comes back - it has no idea which transitions are legal.
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
