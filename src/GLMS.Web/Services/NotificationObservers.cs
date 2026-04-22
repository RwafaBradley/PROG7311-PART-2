// Learning edition: every section is explained inline so the code reads like a guided walkthrough.
// Think of the app like a logistics office: controllers are the reception desk, services are the back office,
// models are the forms, and the in-memory store is the filing cabinet while the database is still simulated.

// Imports a namespace so this file can use shared classes, services, or framework features.
using GLMS.Web.Models;

// Declares the namespace so the class belongs to the GLMS.Web area.
namespace GLMS.Web.Services;

// Defines an immutable data shape for passing information around safely.
public record NotificationEvent(string Kind, string Message, DateTime OccurredAtUtc);

// Defines a contract that other classes must follow.
public interface INotificationObserver
// Marks the beginning or end of a block in C#.
{
    void Update(NotificationEvent notification);
// Marks the beginning or end of a block in C#.
}

// Defines a contract that other classes must follow.
public interface INotificationSubject
// Marks the beginning or end of a block in C#.
{
    void Subscribe(INotificationObserver observer);
    void Notify(NotificationEvent notification);
// Marks the beginning or end of a block in C#.
}

// Defines a class, which groups related data or behavior together.
public class NotificationCenter : INotificationSubject
// Marks the beginning or end of a block in C#.
{
    // Stores a dependency that the class will reuse without re-creating it.
    private readonly List<INotificationObserver> _observers = new();
    // Stores a dependency that the class will reuse without re-creating it.
    private readonly InMemoryStore _store;

    // Declares a method or property that other parts of the app can use.
    public NotificationCenter(IEnumerable<INotificationObserver> observers, InMemoryStore store)
    // Marks the beginning or end of a block in C#.
    {
        // Uses an existing field or dependency rather than creating a duplicate copy.
        _store = store;
        // Uses an existing field or dependency rather than creating a duplicate copy.
        _observers.AddRange(observers);
    // Marks the beginning or end of a block in C#.
    }

    // Declares a method or property that other parts of the app can use.
    public void Subscribe(INotificationObserver observer)
    // Marks the beginning or end of a block in C#.
    {
        // Checks a rule before continuing, so invalid states are stopped early.
        if (!_observers.Contains(observer))
        // Marks the beginning or end of a block in C#.
        {
            // Uses an existing field or dependency rather than creating a duplicate copy.
            _observers.Add(observer);
        // Marks the beginning or end of a block in C#.
        }
    // Marks the beginning or end of a block in C#.
    }

    // Declares a method or property that other parts of the app can use.
    public void Notify(NotificationEvent notification)
    // Marks the beginning or end of a block in C#.
    {
        foreach (var observer in _observers)
        // Marks the beginning or end of a block in C#.
        {
            observer.Update(notification);
        // Marks the beginning or end of a block in C#.
        }
    // Marks the beginning or end of a block in C#.
    }

    // Declares a method or property that other parts of the app can use.
    public void Record(string kind, string message)
    // Marks the beginning or end of a block in C#.
    {
        // Creates a variable and lets C# infer the type from the right-hand side.
        var notification = new NotificationEvent(kind, message, DateTime.UtcNow);
        Notify(notification);
    // Marks the beginning or end of a block in C#.
    }
// Marks the beginning or end of a block in C#.
}

// Defines a class, which groups related data or behavior together.
public class DashboardNotificationObserver : INotificationObserver
// Marks the beginning or end of a block in C#.
{
    // Stores a dependency that the class will reuse without re-creating it.
    private readonly InMemoryStore _store;

    // Declares a method or property that other parts of the app can use.
    public DashboardNotificationObserver(InMemoryStore store)
    // Marks the beginning or end of a block in C#.
    {
        // Uses an existing field or dependency rather than creating a duplicate copy.
        _store = store;
    // Marks the beginning or end of a block in C#.
    }

    // Declares a method or property that other parts of the app can use.
    public void Update(NotificationEvent notification)
    // Marks the beginning or end of a block in C#.
    {
        // Uses an existing field or dependency rather than creating a duplicate copy.
        _store.AddNotification($"{notification.Kind}: {notification.Message}");
    // Marks the beginning or end of a block in C#.
    }
// Marks the beginning or end of a block in C#.
}

// Defines a class, which groups related data or behavior together.
public class AuditNotificationObserver : INotificationObserver
// Marks the beginning or end of a block in C#.
{
    // Stores a dependency that the class will reuse without re-creating it.
    private readonly InMemoryStore _store;

    // Declares a method or property that other parts of the app can use.
    public AuditNotificationObserver(InMemoryStore store)
    // Marks the beginning or end of a block in C#.
    {
        // Uses an existing field or dependency rather than creating a duplicate copy.
        _store = store;
    // Marks the beginning or end of a block in C#.
    }

    // Declares a method or property that other parts of the app can use.
    public void Update(NotificationEvent notification)
    // Marks the beginning or end of a block in C#.
    {
        // Uses an existing field or dependency rather than creating a duplicate copy.
        _store.AddAudit($"{notification.Kind} at {notification.OccurredAtUtc:HH:mm:ss} -> {notification.Message}");
    // Marks the beginning or end of a block in C#.
    }
// Marks the beginning or end of a block in C#.
}
