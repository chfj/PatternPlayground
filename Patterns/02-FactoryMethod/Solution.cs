using System;

namespace Patterns.FactoryMethod.Solution;

public interface INotification
{
    void Send(string message);
}

public sealed class EmailNotification : INotification
{
    public void Send(string message) => Console.WriteLine($"[Email] {message}");
}

public sealed class SmsNotification : INotification
{
    public void Send(string message) => Console.WriteLine($"[SMS] {message}");
}

public sealed class PushNotification : INotification
{
    public void Send(string message) => Console.WriteLine($"[Push] {message}");
}

// Abstract Creator: owns the workflow that stays constant (build, then
// send) and declares the "factory method" that varies per subclass. Callers
// that only see NotificationCreator never need to know which concrete
// INotification type is involved.
public abstract class NotificationCreator
{
    protected abstract INotification CreateNotification();

    public void Notify(string message)
    {
        var notification = CreateNotification();
        notification.Send(message);
    }
}

// Each concrete creator's only job is answering "which product?" — all the
// orchestration logic lives once, in the base class.
public sealed class EmailNotificationCreator : NotificationCreator
{
    protected override INotification CreateNotification() => new EmailNotification();
}

public sealed class SmsNotificationCreator : NotificationCreator
{
    protected override INotification CreateNotification() => new SmsNotification();
}

public sealed class PushNotificationCreator : NotificationCreator
{
    protected override INotification CreateNotification() => new PushNotification();
}

public static class NotificationService
{
    // This little switch is the ONLY place in the whole app that needs to
    // know the mapping from "channel name" to "concrete creator type" —
    // every other call site just works against the NotificationCreator
    // abstraction.
    public static NotificationCreator GetCreatorFor(string preferredChannel) => preferredChannel switch
    {
        "email" => new EmailNotificationCreator(),
        "sms" => new SmsNotificationCreator(),
        "push" => new PushNotificationCreator(),
        _ => throw new ArgumentException($"Unknown channel: {preferredChannel}", nameof(preferredChannel)),
    };

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
