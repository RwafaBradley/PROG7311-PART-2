// Learning edition: every section is explained inline so the code reads like a guided walkthrough.
// Think of the app like a logistics office: controllers are the reception desk, services are the back office,
// models are the forms, and the in-memory store is the filing cabinet while the database is still simulated.

// Imports a namespace so this file can use shared classes, services, or framework features.
using System.ComponentModel.DataAnnotations;

// Declares the namespace so the class belongs to the GLMS.Web area.
namespace GLMS.Web.ViewModels;

// Defines a class, which groups related data or behavior together.
public class LoginViewModel
// Marks the beginning or end of a block in C#.
{
    // Adds metadata that changes how ASP.NET Core treats this action or property.
    [Required]
    // Declares a property that stores data on the object.
    public string Username { get; set; } = string.Empty;

    // Adds metadata that changes how ASP.NET Core treats this action or property.
    [Required]
    // Adds metadata that changes how ASP.NET Core treats this action or property.
    [DataType(DataType.Password)]
    // Declares a property that stores data on the object.
    public string Password { get; set; } = string.Empty;

    // Declares a property that stores data on the object.
    public string? ReturnUrl { get; set; }
// Marks the beginning or end of a block in C#.
}
