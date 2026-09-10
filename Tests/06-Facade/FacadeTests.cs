using Patterns.Facade.Task;

namespace Tests;

public class FacadeTests
{
    private static CheckoutFacade CreateFacade() => new(
        new InventoryService(),
        new PaymentService(),
        new ShippingService(),
        new NotificationService());

    [Fact]
    public void PlaceOrder_WithAvailableStockAndValidPayment_Succeeds()
    {
        var facade = CreateFacade();

        var result = facade.PlaceOrder("cust-1", "WIDGET", 2, 39.98m);

        Assert.True(result);
    }

    [Fact]
    public void PlaceOrder_WithOutOfStockSku_Fails()
    {
        var facade = CreateFacade();

        var result = facade.PlaceOrder("cust-2", "GADGET", 1, 15.00m);

        Assert.False(result);
    }

    [Fact]
    public void PlaceOrder_WithZeroAmount_FailsBecausePaymentDeclines()
    {
        var facade = CreateFacade();

        var result = facade.PlaceOrder("cust-3", "WIDGET", 1, 0m);

        Assert.False(result);
    }

    [Fact]
    public void PlaceOrder_RunsInventoryPaymentShippingNotificationInThatOrder()
    {
        var facade = CreateFacade();

        var output = ConsoleCapture.Capture(() => facade.PlaceOrder("cust-1", "WIDGET", 1, 9.99m));

        var inventoryIndex = output.IndexOf("[Inventory]", StringComparison.Ordinal);
        var paymentIndex = output.IndexOf("[Payment]", StringComparison.Ordinal);
        var shippingIndex = output.IndexOf("[Shipping]", StringComparison.Ordinal);
        var notificationIndex = output.IndexOf("[Notification]", StringComparison.Ordinal);

        Assert.True(inventoryIndex >= 0);
        Assert.True(inventoryIndex < paymentIndex);
        Assert.True(paymentIndex < shippingIndex);
        Assert.True(shippingIndex < notificationIndex);
    }
}
