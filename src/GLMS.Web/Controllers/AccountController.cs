// Learning edition: every section is explained inline so the code reads like a guided walkthrough.
// Think of the app like a logistics office: controllers are the reception desk, services are the back office,
// models are the forms, and the in-memory store is the filing cabinet while the database is still simulated.

// Imports a namespace so this file can use shared classes, services, or framework features.
using GLMS.Web.Services;
// Imports a namespace so this file can use shared classes, services, or framework features.
using GLMS.Web.ViewModels;
// Imports a namespace so this file can use shared classes, services, or framework features.
using Microsoft.AspNetCore.Authorization;
// Imports a namespace so this file can use shared classes, services, or framework features.
using Microsoft.AspNetCore.Authentication;
// Imports a namespace so this file can use shared classes, services, or framework features.
using Microsoft.AspNetCore.Authentication.Cookies;
// Imports a namespace so this file can use shared classes, services, or framework features.
using Microsoft.AspNetCore.Mvc;

// Declares the namespace so the class belongs to the GLMS.Web area.
namespace GLMS.Web.Controllers;

// Defines a class, which groups related data or behavior together.
public class AccountController : Controller
// Marks the beginning or end of a block in C#.
{
    // Stores a dependency that the class will reuse without re-creating it.
    private readonly IAuthService _authService;

    // Declares a method or property that other parts of the app can use.
    public AccountController(IAuthService authService)
    // Marks the beginning or end of a block in C#.
    {
        // Uses an existing field or dependency rather than creating a duplicate copy.
        _authService = authService;
    // Marks the beginning or end of a block in C#.
    }

    // Adds metadata that changes how ASP.NET Core treats this action or property.
    [HttpGet]
    // Adds metadata that changes how ASP.NET Core treats this action or property.
    [AllowAnonymous]
    // Declares a method or property that other parts of the app can use.
    public IActionResult Login(string? returnUrl = null)
    // Marks the beginning or end of a block in C#.
    {
        ViewBag.ReturnUrl = returnUrl;
        ViewBag.Credentials = new[]
        // Marks the beginning or end of a block in C#.
        {
            "admin / Admin@123 (Admin)",
            "manager / Manager@123 (Manager)",
            "viewer / Viewer@123 (Viewer)"
        };
        // Sends a value back to the caller and ends this branch of logic.
        return View(new LoginViewModel { ReturnUrl = returnUrl });
    // Marks the beginning or end of a block in C#.
    }

    // Adds metadata that changes how ASP.NET Core treats this action or property.
    [HttpPost]
    // Adds metadata that changes how ASP.NET Core treats this action or property.
    [AllowAnonymous]
    // Adds metadata that changes how ASP.NET Core treats this action or property.
    [ValidateAntiForgeryToken]
    // Declares a method or property that other parts of the app can use.
    public async Task<IActionResult> Login(LoginViewModel model)
    // Marks the beginning or end of a block in C#.
    {
        // Checks a rule before continuing, so invalid states are stopped early.
        if (!ModelState.IsValid)
        // Marks the beginning or end of a block in C#.
        {
            // Sends a value back to the caller and ends this branch of logic.
            return View(model);
        // Marks the beginning or end of a block in C#.
        }

        // Checks a rule before continuing, so invalid states are stopped early.
        if (!_authService.Validate(model.Username, model.Password, out var user) || user is null)
        // Marks the beginning or end of a block in C#.
        {
            ModelState.AddModelError(string.Empty, "Invalid username or password.");
            ViewBag.Credentials = new[]
            // Marks the beginning or end of a block in C#.
            {
                "admin / Admin@123 (Admin)",
                "manager / Manager@123 (Manager)",
                "viewer / Viewer@123 (Viewer)"
            };
            // Sends a value back to the caller and ends this branch of logic.
            return View(model);
        // Marks the beginning or end of a block in C#.
        }

        // Creates a variable and lets C# infer the type from the right-hand side.
        var principal = _authService.CreatePrincipal(user);
        // Waits for an asynchronous operation without freezing the whole app.
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        // Checks a rule before continuing, so invalid states are stopped early.
        if (!string.IsNullOrWhiteSpace(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
        // Marks the beginning or end of a block in C#.
        {
            // Sends a value back to the caller and ends this branch of logic.
            return Redirect(model.ReturnUrl);
        // Marks the beginning or end of a block in C#.
        }

        // Sends a value back to the caller and ends this branch of logic.
        return RedirectToAction("Index", "Home");
    // Marks the beginning or end of a block in C#.
    }

    // Adds metadata that changes how ASP.NET Core treats this action or property.
    [HttpPost]
    // Adds metadata that changes how ASP.NET Core treats this action or property.
    [Authorize]
    // Adds metadata that changes how ASP.NET Core treats this action or property.
    [ValidateAntiForgeryToken]
    // Declares a method or property that other parts of the app can use.
    public async Task<IActionResult> Logout()
    // Marks the beginning or end of a block in C#.
    {
        // Waits for an asynchronous operation without freezing the whole app.
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        // Sends a value back to the caller and ends this branch of logic.
        return RedirectToAction(nameof(Login));
    // Marks the beginning or end of a block in C#.
    }

    // Adds metadata that changes how ASP.NET Core treats this action or property.
    [HttpGet]
    // Declares a method or property that other parts of the app can use.
    public IActionResult AccessDenied()
    // Marks the beginning or end of a block in C#.
    {
        // Sends a value back to the caller and ends this branch of logic.
        return View();
    // Marks the beginning or end of a block in C#.
    }
// Marks the beginning or end of a block in C#.
}
