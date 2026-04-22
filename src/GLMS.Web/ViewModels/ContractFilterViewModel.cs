// Learning edition: every section is explained inline so the code reads like a guided walkthrough.
// Think of the app like a logistics office: controllers are the reception desk, services are the back office,
// models are the forms, and the in-memory store is the filing cabinet while the database is still simulated.

// Imports a namespace so this file can use shared classes, services, or framework features.
using GLMS.Web.Models;
// Imports a namespace so this file can use shared classes, services, or framework features.
using Microsoft.AspNetCore.Mvc.Rendering;

// Declares the namespace so the class belongs to the GLMS.Web area.
namespace GLMS.Web.ViewModels;

// Defines a class, which groups related data or behavior together.
public class ContractFilterViewModel
// Marks the beginning or end of a block in C#.
{
    // Declares a property that stores data on the object.
    public string? Search { get; set; }
    // Declares a property that stores data on the object.
    public ContractStatus? Status { get; set; }
    // Declares a property that stores data on the object.
    public DateTime? StartDate { get; set; }
    // Declares a property that stores data on the object.
    public DateTime? EndDate { get; set; }

    // Declares a method or property that other parts of the app can use.
    public List<Contract> Results { get; set; } = new();
    // Declares a method or property that other parts of the app can use.
    public List<SelectListItem> StatusOptions { get; set; } = new();
// Marks the beginning or end of a block in C#.
}
