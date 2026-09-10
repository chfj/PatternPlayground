using Patterns.FactoryMethod.Task;

namespace Tests;

public class FactoryMethodTests
{
    [Fact]
    public void EmailNotification_Send_PrintsEmailTagAndMessage()
    {
        var output = ConsoleCapture.Capture(() => new EmailNotification().Send("hello"));

        Assert.Contains("Email", output);
        Assert.Contains("hello", output);
    }

    [Fact]
    public void SmsNotification_Send_PrintsSmsTagAndMessage()
    {
        var output = ConsoleCapture.Capture(() => new SmsNotification().Send("hello"));

        Assert.Contains("SMS", output);
        Assert.Contains("hello", output);
    }

    [Fact]
    public void PushNotification_Send_PrintsPushTagAndMessage()
    {
        var output = ConsoleCapture.Capture(() => new PushNotification().Send("hello"));

        Assert.Contains("Push", output);
        Assert.Contains("hello", output);
    }

    [Fact]
    public void EmailNotificationCreator_Notify_SendsViaEmail()
    {
        var output = ConsoleCapture.Capture(() => new EmailNotificationCreator().Notify("order shipped"));

        Assert.Contains("Email", output);
        Assert.Contains("order shipped", output);
    }

    [Fact]
    public void SmsNotificationCreator_Notify_SendsViaSms()
    {
        var output = ConsoleCapture.Capture(() => new SmsNotificationCreator().Notify("order shipped"));

        Assert.Contains("SMS", output);
    }

    [Fact]
    public void PushNotificationCreator_Notify_SendsViaPush()
    {
        var output = ConsoleCapture.Capture(() => new PushNotificationCreator().Notify("order shipped"));

        Assert.Contains("Push", output);
    }

    [Theory]
    [InlineData("email", "Email")]
    [InlineData("sms", "SMS")]
    [InlineData("push", "Push")]
    public void GetCreatorFor_ReturnsTheCreatorMatchingTheChannel(string channel, string expectedTag)
    {
        var creator = NotificationService.GetCreatorFor(channel);

        var output = ConsoleCapture.Capture(() => creator.Notify("test message"));

        Assert.Contains(expectedTag, output);
    }

    [Fact]
    public void GetCreatorFor_WithUnknownChannel_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => NotificationService.GetCreatorFor("carrier-pigeon"));
    }
}
