// Learning edition: every section is explained inline so the code reads like a guided walkthrough.
// Think of the app like a logistics office: controllers are the reception desk, services are the back office,
// models are the forms, and the in-memory store is the filing cabinet while the database is still simulated.

// Declares the namespace so the class belongs to the GLMS.Web area.
namespace GLMS.Web.ViewModels;

// Defines a class, which groups related data or behavior together.
public class DashboardViewModel
// Marks the beginning or end of a block in C#.
{
    // Declares a property that stores data on the object.
    public string UserName { get; set; } = string.Empty;
    // Declares a property that stores data on the object.
    public string UserRole { get; set; } = string.Empty;
    // Declares a property that stores data on the object.
    public int ClientCount { get; set; }
    // Declares a property that stores data on the object.
    public int ContractCount { get; set; }
    // Declares a property that stores data on the object.
    public int ServiceRequestCount { get; set; }
    // Declares a property that stores data on the object.
    public decimal CurrentUsdToZarRate { get; set; }
    // Declares a method or property that other parts of the app can use.
    public List<string> Notifications { get; set; } = new();
// Marks the beginning or end of a block in C#.
}
