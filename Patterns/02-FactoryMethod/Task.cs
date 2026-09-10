using System;

namespace Patterns.FactoryMethod.Task;

// Scenario: a notification service must alert users through whichever
// channel they prefer (email, SMS, or push). The sending logic ("build the
// notification, then send it") is the same regardless of channel — only
// *which* notification type gets built differs. That "which concrete type"
// decision is exactly what Factory Method hands off to subclasses.

public interface INotification
{
    void Send(string message);
}

public sealed class EmailNotification : INotification
{
    public void Send(string message)
    {
        // TODO: print something like "[Email] <message>"
        throw new NotImplementedException();
    }
}

public sealed class SmsNotification : INotification
{
    public void Send(string message)
    {
        // TODO: print something like "[SMS] <message>"
        throw new NotImplementedException();
    }
}

public sealed class PushNotification : INotification
{
    public void Send(string message)
    {
        // TODO: print something like "[Push] <message>"
        throw new NotImplementedException();
    }
}

// The "Creator" in Factory Method: it defines the fixed workflow
// (Notify -> build a notification -> send it) but leaves *which*
// notification gets built up to each subclass.
public abstract class NotificationCreator
{
    // TODO: each subclass must supply the concrete INotification to use.
    protected abstract INotification CreateNotification();

    public void Notify(string message)
    {
        var notification = CreateNotification();
        notification.Send(message);
    }
}

public sealed class EmailNotificationCreator : NotificationCreator
{
    // TODO: return a new EmailNotification
    protected override INotification CreateNotification() => throw new NotImplementedException();
}

public sealed class SmsNotificationCreator : NotificationCreator
{
    // TODO: return a new SmsNotification
    protected override INotification CreateNotification() => throw new NotImplementedException();
}

public sealed class PushNotificationCreator : NotificationCreator
{
    // TODO: return a new PushNotification
    protected override INotification CreateNotification() => throw new NotImplementedException();
}

public static class NotificationService
{
    // TODO: given a user's preferred channel ("email" | "sms" | "push"),
    // return the matching NotificationCreator. Throw ArgumentException for
    // anything else.
    public static NotificationCreator GetCreatorFor(string preferredChannel)
    {
        throw new NotImplementedException();
    }

    public static void Demo()
    {
        string[] users = ["email", "sms", "push"];
        foreach (var channel in users)
        {
            var creator = GetCreatorFor(channel);
            creator.Notify($"Your order shipped! (delivered via {channel})");
        }
    }
}
