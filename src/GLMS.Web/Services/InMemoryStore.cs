// Learning edition: every section is explained inline so the code reads like a guided walkthrough.
// Think of the app like a logistics office: controllers are the reception desk, services are the back office,
// models are the forms, and the in-memory store is the filing cabinet while the database is still simulated.

// Imports a namespace so this file can use shared classes, services, or framework features.
using GLMS.Web.Models;

// Declares the namespace so the class belongs to the GLMS.Web area.
namespace GLMS.Web.Services;

// Defines a class, which groups related data or behavior together.
public class InMemoryStore
// Marks the beginning or end of a block in C#.
{
    // Stores a dependency that the class will reuse without re-creating it.
    private readonly object _sync = new();

    // Declares a method or property that other parts of the app can use.
    public List<Client> Clients { get; } = new();
    // Declares a method or property that other parts of the app can use.
    public List<Contract> Contracts { get; } = new();
    // Declares a method or property that other parts of the app can use.
    public List<ServiceRequest> ServiceRequests { get; } = new();
    // Declares a method or property that other parts of the app can use.
    public List<string> Notifications { get; } = new();
    // Declares a method or property that other parts of the app can use.
    public List<string> AuditTrail { get; } = new();
    // Declares a method or property that other parts of the app can use.
    public List<AppUser> Users { get; } = new();

    private int _clientId = 1;
    private int _contractId = 1;
    private int _requestId = 1;

    // Declares a method or property that other parts of the app can use.
    public int NextClientId()
    // Marks the beginning or end of a block in C#.
    {
        lock (_sync) { return _clientId++; }
    // Marks the beginning or end of a block in C#.
    }

    // Declares a method or property that other parts of the app can use.
    public int NextContractId()
    // Marks the beginning or end of a block in C#.
    {
        lock (_sync) { return _contractId++; }
    // Marks the beginning or end of a block in C#.
    }

    // Declares a method or property that other parts of the app can use.
    public int NextRequestId()
    // Marks the beginning or end of a block in C#.
    {
        lock (_sync) { return _requestId++; }
    // Marks the beginning or end of a block in C#.
    }

    // Declares a method or property that other parts of the app can use.
    public void AddNotification(string message)
    // Marks the beginning or end of a block in C#.
    {
        lock (_sync)
        // Marks the beginning or end of a block in C#.
        {
            Notifications.Insert(0, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
        // Marks the beginning or end of a block in C#.
        }
    // Marks the beginning or end of a block in C#.
    }

    // Declares a method or property that other parts of the app can use.
    public void AddAudit(string message)
    // Marks the beginning or end of a block in C#.
    {
        lock (_sync)
        // Marks the beginning or end of a block in C#.
        {
            AuditTrail.Insert(0, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
        // Marks the beginning or end of a block in C#.
        }
    // Marks the beginning or end of a block in C#.
    }
// Marks the beginning or end of a block in C#.
}
