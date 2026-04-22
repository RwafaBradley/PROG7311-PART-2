// Learning edition: every section is explained inline so the code reads like a guided walkthrough.
// Think of the app like a logistics office: controllers are the reception desk, services are the back office,
// models are the forms, and the in-memory store is the filing cabinet while the database is still simulated.

// Imports a namespace so this file can use shared classes, services, or framework features.
using GLMS.Web.Models;
// Imports a namespace so this file can use shared classes, services, or framework features.
using GLMS.Web.Services;
// Imports a namespace so this file can use shared classes, services, or framework features.
using GLMS.Web.ViewModels;
// Imports a namespace so this file can use shared classes, services, or framework features.
using Microsoft.AspNetCore.Authorization;
// Imports a namespace so this file can use shared classes, services, or framework features.
using Microsoft.AspNetCore.Mvc;
// Imports a namespace so this file can use shared classes, services, or framework features.
using Microsoft.AspNetCore.Mvc.Rendering;

// Declares the namespace so the class belongs to the GLMS.Web area.
namespace GLMS.Web.Controllers;

// Adds metadata that changes how ASP.NET Core treats this action or property.
[Authorize]
// Defines a class, which groups related data or behavior together.
public class ServiceRequestsController : Controller
// Marks the beginning or end of a block in C#.
{
    // Stores a dependency that the class will reuse without re-creating it.
    private readonly IAppDataService _dataService;
    // Stores a dependency that the class will reuse without re-creating it.
    private readonly ICurrencyConversionStrategy _currencyStrategy;
    // Stores a dependency that the class will reuse without re-creating it.
    private readonly IServiceRequestWorkflowService _workflowService;

    // Declares a method or property that other parts of the app can use.
    public ServiceRequestsController(
        IAppDataService dataService,
        ICurrencyConversionStrategy currencyStrategy,
        IServiceRequestWorkflowService workflowService)
    // Marks the beginning or end of a block in C#.
    {
        // Uses an existing field or dependency rather than creating a duplicate copy.
        _dataService = dataService;
        // Uses an existing field or dependency rather than creating a duplicate copy.
        _currencyStrategy = currencyStrategy;
        // Uses an existing field or dependency rather than creating a duplicate copy.
        _workflowService = workflowService;
    // Marks the beginning or end of a block in C#.
    }

    // Declares a method or property that other parts of the app can use.
    public async Task<IActionResult> Index()
    // Marks the beginning or end of a block in C#.
    {
        // Sends a value back to the caller and ends this branch of logic.
        return View(await _dataService.GetServiceRequestsAsync());
    // Marks the beginning or end of a block in C#.
    }

    // Adds metadata that changes how ASP.NET Core treats this action or property.
    [HttpGet]
    // Adds metadata that changes how ASP.NET Core treats this action or property.
    [Authorize(Roles = "Admin,Manager")]
    // Declares a method or property that other parts of the app can use.
    public IActionResult Create()
    // Marks the beginning or end of a block in C#.
    {
        // Creates a variable and lets C# infer the type from the right-hand side.
        var model = new ServiceRequestCreateViewModel
        // Marks the beginning or end of a block in C#.
        {
            Contracts = _dataService.GetActiveContractSelectList(),
            ExchangeRate = _currencyStrategy.GetUsdToZarRate()
        };
        model.ZarAmount = _currencyStrategy.ConvertUsdToZar(model.UsdAmount);
        ViewBag.StrategyName = _currencyStrategy.StrategyName;
        // Sends a value back to the caller and ends this branch of logic.
        return View(model);
    // Marks the beginning or end of a block in C#.
    }

    // Adds metadata that changes how ASP.NET Core treats this action or property.
    [HttpPost]
    // Adds metadata that changes how ASP.NET Core treats this action or property.
    [Authorize(Roles = "Admin,Manager")]
    // Adds metadata that changes how ASP.NET Core treats this action or property.
    [ValidateAntiForgeryToken]
    // Declares a method or property that other parts of the app can use.
    public async Task<IActionResult> Create(ServiceRequestCreateViewModel model)
    // Marks the beginning or end of a block in C#.
    {
        model.Contracts = _dataService.GetActiveContractSelectList();
        model.ExchangeRate = _currencyStrategy.GetUsdToZarRate();
        model.ZarAmount = _currencyStrategy.ConvertUsdToZar(model.UsdAmount);
        ViewBag.StrategyName = _currencyStrategy.StrategyName;

        // Checks a rule before continuing, so invalid states are stopped early.
        if (!ModelState.IsValid)
        // Marks the beginning or end of a block in C#.
        {
            // Sends a value back to the caller and ends this branch of logic.
            return View(model);
        // Marks the beginning or end of a block in C#.
        }

        // Creates a variable and lets C# infer the type from the right-hand side.
        var contract = await _dataService.GetContractAsync(model.ContractId);
        // Checks a rule before continuing, so invalid states are stopped early.
        if (!_workflowService.CanCreateRequest(contract, out var errorMessage))
        // Marks the beginning or end of a block in C#.
        {
            ModelState.AddModelError(string.Empty, errorMessage);
            // Sends a value back to the caller and ends this branch of logic.
            return View(model);
        // Marks the beginning or end of a block in C#.
        }

        // Creates a variable and lets C# infer the type from the right-hand side.
        var request = new ServiceRequest
        // Marks the beginning or end of a block in C#.
        {
            ContractId = model.ContractId,
            Description = model.Description,
            UsdAmount = model.UsdAmount,
            ExchangeRate = model.ExchangeRate,
            ZarAmount = model.ZarAmount,
            Status = RequestStatus.New
        };

        // Waits for an asynchronous operation without freezing the whole app.
        await _dataService.CreateServiceRequestAsync(request);
        TempData["Message"] = "Service request created successfully.";
        // Sends a value back to the caller and ends this branch of logic.
        return RedirectToAction(nameof(Index));
    // Marks the beginning or end of a block in C#.
    }
// Marks the beginning or end of a block in C#.
}
