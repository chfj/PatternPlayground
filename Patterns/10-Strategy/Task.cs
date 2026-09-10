using System;

namespace Patterns.Strategy.Task;

// Scenario: a checkout page calculates shipping cost using whichever
// option the customer picked - Standard, Express, or Overnight - each with
// its own pricing formula. The checkout flow itself ("calculate cost for
// this package") should stay identical no matter which option is active.

public interface IShippingStrategy
{
    string Name { get; }
    decimal CalculateCost(decimal weightKg, decimal distanceKm);
}

public sealed class StandardShippingStrategy : IShippingStrategy
{
    public string Name => "Standard";

    // TODO: flat $5 base + $0.50 per kg + $0.05 per km
    public decimal CalculateCost(decimal weightKg, decimal distanceKm)
    {
        throw new NotImplementedException();
    }
}

public sealed class ExpressShippingStrategy : IShippingStrategy
{
    public string Name => "Express";

    // TODO: flat $12 base + $1.00 per kg + $0.10 per km
    public decimal CalculateCost(decimal weightKg, decimal distanceKm)
    {
        throw new NotImplementedException();
    }
}

public sealed class OvernightShippingStrategy : IShippingStrategy
{
    public string Name => "Overnight";

    // TODO: flat $25 base + $2.00 per kg + $0.20 per km
    public decimal CalculateCost(decimal weightKg, decimal distanceKm)
    {
        throw new NotImplementedException();
    }
}

public sealed class ShippingCalculator(IShippingStrategy strategy)
{
    private IShippingStrategy _strategy = strategy;

    // TODO: swap the active strategy.
    public void SetStrategy(IShippingStrategy strategy)
    {
        throw new NotImplementedException();
    }

    // TODO: delegate to the current strategy's CalculateCost.
    public decimal Calculate(decimal weightKg, decimal distanceKm)
    {
        throw new NotImplementedException();
    }

    public static void Demo()
    {
        var calculator = new ShippingCalculator(new StandardShippingStrategy());
        const decimal weight = 3m;
        const decimal distance = 100m;

        foreach (IShippingStrategy strategy in new IShippingStrategy[]
                 {
                     new StandardShippingStrategy(),
                     new ExpressShippingStrategy(),
                     new OvernightShippingStrategy(),
                 })
        {
            calculator.SetStrategy(strategy);
            var cost = calculator.Calculate(weight, distance);
            Console.WriteLine($"{strategy.Name} shipping for {weight}kg over {distance}km: {cost:C}");
        }
    }
}
