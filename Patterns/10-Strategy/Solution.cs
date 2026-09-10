using System;

namespace Patterns.Strategy.Solution;

public interface IShippingStrategy
{
    string Name { get; }
    decimal CalculateCost(decimal weightKg, decimal distanceKm);
}

public sealed class StandardShippingStrategy : IShippingStrategy
{
    public string Name => "Standard";
    public decimal CalculateCost(decimal weightKg, decimal distanceKm) => 5m + weightKg * 0.50m + distanceKm * 0.05m;
}

public sealed class ExpressShippingStrategy : IShippingStrategy
{
    public string Name => "Express";
    public decimal CalculateCost(decimal weightKg, decimal distanceKm) => 12m + weightKg * 1.00m + distanceKm * 0.10m;
}

public sealed class OvernightShippingStrategy : IShippingStrategy
{
    public string Name => "Overnight";
    public decimal CalculateCost(decimal weightKg, decimal distanceKm) => 25m + weightKg * 2.00m + distanceKm * 0.20m;
}

// The calculator holds a reference to whichever IShippingStrategy is
// active and always calls through the interface. Swapping strategies is
// just reassigning that reference - Calculate() itself never changes,
// and never needs an if/switch on "which shipping option."
public sealed class ShippingCalculator(IShippingStrategy strategy)
{
    private IShippingStrategy _strategy = strategy;

    public void SetStrategy(IShippingStrategy strategy) => _strategy = strategy;

    public decimal Calculate(decimal weightKg, decimal distanceKm) => _strategy.CalculateCost(weightKg, distanceKm);

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
