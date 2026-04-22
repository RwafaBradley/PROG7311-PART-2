// Learning edition: every section is explained inline so the code reads like a guided walkthrough.
// Think of the app like a logistics office: controllers are the reception desk, services are the back office,
// models are the forms, and the in-memory store is the filing cabinet while the database is still simulated.

// Imports a namespace so this file can use shared classes, services, or framework features.
using System.ComponentModel.DataAnnotations;
// Imports a namespace so this file can use shared classes, services, or framework features.
using GLMS.Web.Models;
// Imports a namespace so this file can use shared classes, services, or framework features.
using Microsoft.AspNetCore.Mvc.Rendering;

// Declares the namespace so the class belongs to the GLMS.Web area.
namespace GLMS.Web.ViewModels;

// Defines a class, which groups related data or behavior together.
public class ContractCreateViewModel
// Marks the beginning or end of a block in C#.
{
    // Adds metadata that changes how ASP.NET Core treats this action or property.
    [Required]
    // Declares a property that stores data on the object.
    public int ClientId { get; set; }

    // Adds metadata that changes how ASP.NET Core treats this action or property.
    [Required]
    // Adds metadata that changes how ASP.NET Core treats this action or property.
    [StringLength(60)]
    // Declares a property that stores data on the object.
    public string ContractNumber { get; set; } = string.Empty;

    // Adds metadata that changes how ASP.NET Core treats this action or property.
    [Required]
    // Declares a property that stores data on the object.
    public DateTime StartDate { get; set; } = DateTime.UtcNow.Date;

    // Adds metadata that changes how ASP.NET Core treats this action or property.
    [Required]
    // Declares a method or property that other parts of the app can use.
    public DateTime EndDate { get; set; } = DateTime.UtcNow.Date.AddMonths(12);

    // Adds metadata that changes how ASP.NET Core treats this action or property.
    [Required]
    // Declares a property that stores data on the object.
    public ContractStatus Status { get; set; } = ContractStatus.Draft;

    // Adds metadata that changes how ASP.NET Core treats this action or property.
    [Required]
    // Adds metadata that changes how ASP.NET Core treats this action or property.
    [StringLength(200)]
    // Declares a property that stores data on the object.
    public string ServiceLevel { get; set; } = "Standard";

    // Declares a property that stores data on the object.
    public IFormFile? SignedAgreement { get; set; }

    // Declares a method or property that other parts of the app can use.
    public List<SelectListItem> Clients { get; set; } = new();
// Marks the beginning or end of a block in C#.
}
