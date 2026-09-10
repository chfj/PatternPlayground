using Patterns.Strategy.Task;

namespace Tests;

public class StrategyTests
{
    [Fact]
    public void StandardShipping_UsesFiveDollarBasePlusFiftyCentsPerKgPlusFiveCentsPerKm()
    {
        var strategy = new StandardShippingStrategy();

        var cost = strategy.CalculateCost(weightKg: 3m, distanceKm: 100m);

        Assert.Equal(11.50m, cost); // 5 + 0.50*3 + 0.05*100
    }

    [Fact]
    public void ExpressShipping_UsesTwelveDollarBasePlusOneDollarPerKgPlusTenCentsPerKm()
    {
        var strategy = new ExpressShippingStrategy();

        var cost = strategy.CalculateCost(weightKg: 3m, distanceKm: 100m);

        Assert.Equal(25.00m, cost); // 12 + 1.00*3 + 0.10*100
    }

    [Fact]
    public void OvernightShipping_UsesTwentyFiveDollarBasePlusTwoDollarsPerKgPlusTwentyCentsPerKm()
    {
        var strategy = new OvernightShippingStrategy();

        var cost = strategy.CalculateCost(weightKg: 3m, distanceKm: 100m);

        Assert.Equal(51.00m, cost); // 25 + 2.00*3 + 0.20*100
    }

    [Fact]
    public void Calculator_DelegatesToTheCurrentStrategy()
    {
        var calculator = new ShippingCalculator(new StandardShippingStrategy());

        var cost = calculator.Calculate(3m, 100m);

        Assert.Equal(11.50m, cost);
    }

    [Fact]
    public void Calculator_SetStrategy_SwapsWhichStrategyIsUsed()
    {
        var calculator = new ShippingCalculator(new StandardShippingStrategy());

        calculator.SetStrategy(new OvernightShippingStrategy());
        var cost = calculator.Calculate(3m, 100m);

        Assert.Equal(51.00m, cost);
    }
}
