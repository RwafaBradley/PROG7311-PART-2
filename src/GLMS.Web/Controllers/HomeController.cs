// Learning edition: every section is explained inline so the code reads like a guided walkthrough.
// Think of the app like a logistics office: controllers are the reception desk, services are the back office,
// models are the forms, and the in-memory store is the filing cabinet while the database is still simulated.

// Imports a namespace so this file can use shared classes, services, or framework features.
using GLMS.Web.Services;
// Imports a namespace so this file can use shared classes, services, or framework features.
using Microsoft.AspNetCore.Authorization;
// Imports a namespace so this file can use shared classes, services, or framework features.
using Microsoft.AspNetCore.Mvc;

// Declares the namespace so the class belongs to the GLMS.Web area.
namespace GLMS.Web.Controllers;

// Adds metadata that changes how ASP.NET Core treats this action or property.
[Authorize]
// Defines a class, which groups related data or behavior together.
public class HomeController : Controller
// Marks the beginning or end of a block in C#.
{
    // Stores a dependency that the class will reuse without re-creating it.
    private readonly IAppDataService _dataService;
    // Stores a dependency that the class will reuse without re-creating it.
    private readonly ICurrencyConversionStrategy _currencyStrategy;

    // Declares a method or property that other parts of the app can use.
    public HomeController(IAppDataService dataService, ICurrencyConversionStrategy currencyStrategy)
    // Marks the beginning or end of a block in C#.
    {
        // Uses an existing field or dependency rather than creating a duplicate copy.
        _dataService = dataService;
        // Uses an existing field or dependency rather than creating a duplicate copy.
        _currencyStrategy = currencyStrategy;
    // Marks the beginning or end of a block in C#.
    }

    // Declares a method or property that other parts of the app can use.
    public IActionResult Index()
    // Marks the beginning or end of a block in C#.
    {
        // Creates a variable and lets C# infer the type from the right-hand side.
        var name = User.Identity?.Name ?? "User";
        // Creates a variable and lets C# infer the type from the right-hand side.
        var role = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Role)?.Value ?? "Unknown";
        // Creates a variable and lets C# infer the type from the right-hand side.
        var dashboard = _dataService.BuildDashboard(name, role, _currencyStrategy.GetUsdToZarRate());
        ViewBag.StrategyName = _currencyStrategy.StrategyName;
        // Sends a value back to the caller and ends this branch of logic.
        return View(dashboard);
    // Marks the beginning or end of a block in C#.
    }

    // Declares a method or property that other parts of the app can use.
    public IActionResult Privacy()
    // Marks the beginning or end of a block in C#.
    {
        // Sends a value back to the caller and ends this branch of logic.
        return View();
    // Marks the beginning or end of a block in C#.
    }

    // Adds metadata that changes how ASP.NET Core treats this action or property.
    [AllowAnonymous]
    // Declares a method or property that other parts of the app can use.
    public IActionResult Error()
    // Marks the beginning or end of a block in C#.
    {
        // Sends a value back to the caller and ends this branch of logic.
        return View();
    // Marks the beginning or end of a block in C#.
    }
// Marks the beginning or end of a block in C#.
}
