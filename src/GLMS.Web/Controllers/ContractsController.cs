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
public class ContractsController : Controller
// Marks the beginning or end of a block in C#.
{
    // Stores a dependency that the class will reuse without re-creating it.
    private readonly IAppDataService _dataService;
    // Stores a dependency that the class will reuse without re-creating it.
    private readonly IContractFactory _contractFactory;
    // Stores a dependency that the class will reuse without re-creating it.
    private readonly IFileValidationService _fileValidationService;
    // Stores a dependency that the class will reuse without re-creating it.
    private readonly IWebHostEnvironment _environment;

    // Declares a method or property that other parts of the app can use.
    public ContractsController(
        IAppDataService dataService,
        IContractFactory contractFactory,
        IFileValidationService fileValidationService,
        IWebHostEnvironment environment)
    // Marks the beginning or end of a block in C#.
    {
        // Uses an existing field or dependency rather than creating a duplicate copy.
        _dataService = dataService;
        // Uses an existing field or dependency rather than creating a duplicate copy.
        _contractFactory = contractFactory;
        // Uses an existing field or dependency rather than creating a duplicate copy.
        _fileValidationService = fileValidationService;
        // Uses an existing field or dependency rather than creating a duplicate copy.
        _environment = environment;
    // Marks the beginning or end of a block in C#.
    }

    // Declares a method or property that other parts of the app can use.
    public async Task<IActionResult> Index(string? search, ContractStatus? status, DateTime? startDate, DateTime? endDate)
    // Marks the beginning or end of a block in C#.
    {
        // Creates a variable and lets C# infer the type from the right-hand side.
        var model = new ContractFilterViewModel
        // Marks the beginning or end of a block in C#.
        {
            Search = search,
            Status = status,
            StartDate = startDate,
            EndDate = endDate,
            Results = await _dataService.GetContractsAsync(search, status, startDate, endDate),
            StatusOptions = Enum.GetValues<ContractStatus>()
                .Select(x => new SelectListItem(x.ToString(), x.ToString(), status == x))
                .ToList()
        };

        // Sends a value back to the caller and ends this branch of logic.
        return View(model);
    // Marks the beginning or end of a block in C#.
    }

    // Adds metadata that changes how ASP.NET Core treats this action or property.
    [HttpGet]
    // Adds metadata that changes how ASP.NET Core treats this action or property.
    [Authorize(Roles = "Admin,Manager")]
    // Declares a method or property that other parts of the app can use.
    public async Task<IActionResult> Create()
    // Marks the beginning or end of a block in C#.
    {
        // Creates a variable and lets C# infer the type from the right-hand side.
        var model = new ContractCreateViewModel
        // Marks the beginning or end of a block in C#.
        {
            Clients = _dataService.GetClientSelectList()
        };
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
    public async Task<IActionResult> Create(ContractCreateViewModel model)
    // Marks the beginning or end of a block in C#.
    {
        model.Clients = _dataService.GetClientSelectList();

        // Checks a rule before continuing, so invalid states are stopped early.
        if (!ModelState.IsValid)
        // Marks the beginning or end of a block in C#.
        {
            // Sends a value back to the caller and ends this branch of logic.
            return View(model);
        // Marks the beginning or end of a block in C#.
        }

        // Checks a rule before continuing, so invalid states are stopped early.
        if (model.EndDate.Date < model.StartDate.Date)
        // Marks the beginning or end of a block in C#.
        {
            ModelState.AddModelError(nameof(model.EndDate), "End date must be after the start date.");
            // Sends a value back to the caller and ends this branch of logic.
            return View(model);
        // Marks the beginning or end of a block in C#.
        }

        string? savedPath = null;
        string? savedFileName = null;

        // Checks a rule before continuing, so invalid states are stopped early.
        if (model.SignedAgreement is not null)
        // Marks the beginning or end of a block in C#.
        {
            try
            // Marks the beginning or end of a block in C#.
            {
                // Uses an existing field or dependency rather than creating a duplicate copy.
                _fileValidationService.ValidatePdf(model.SignedAgreement);

                // Creates a variable and lets C# infer the type from the right-hand side.
                var uploadFolder = Path.Combine(_environment.WebRootPath, "uploads", "contracts");
                Directory.CreateDirectory(uploadFolder);

                savedFileName = $"{Guid.NewGuid():N}.pdf";
                savedPath = Path.Combine(uploadFolder, savedFileName);

                // Waits for an asynchronous operation without freezing the whole app.
                await using var stream = new FileStream(savedPath, FileMode.Create);
                // Waits for an asynchronous operation without freezing the whole app.
                await model.SignedAgreement.CopyToAsync(stream);
            // Marks the beginning or end of a block in C#.
            }
            catch (Exception ex)
            // Marks the beginning or end of a block in C#.
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                // Sends a value back to the caller and ends this branch of logic.
                return View(model);
            // Marks the beginning or end of a block in C#.
            }
        // Marks the beginning or end of a block in C#.
        }

        // Creates a variable and lets C# infer the type from the right-hand side.
        var contract = _contractFactory.Create(
            model.ClientId,
            model.ContractNumber,
            model.StartDate,
            model.EndDate,
            model.Status,
            model.ServiceLevel);

        contract.SignedAgreementPath = savedPath;
        contract.SignedAgreementFileName = model.SignedAgreement?.FileName ?? savedFileName;

        // Waits for an asynchronous operation without freezing the whole app.
        await _dataService.CreateContractAsync(contract);
        // Checks a rule before continuing, so invalid states are stopped early.
        if (!string.IsNullOrWhiteSpace(savedPath))
        // Marks the beginning or end of a block in C#.
        {
            // Waits for an asynchronous operation without freezing the whole app.
            await _dataService.AttachAgreementAsync(contract.Id, savedPath, contract.SignedAgreementFileName ?? string.Empty);
        // Marks the beginning or end of a block in C#.
        }

        TempData["Message"] = "Contract created successfully.";
        // Sends a value back to the caller and ends this branch of logic.
        return RedirectToAction(nameof(Index));
    // Marks the beginning or end of a block in C#.
    }

    // Adds metadata that changes how ASP.NET Core treats this action or property.
    [HttpPost]
    // Adds metadata that changes how ASP.NET Core treats this action or property.
    [Authorize(Roles = "Admin,Manager")]
    // Adds metadata that changes how ASP.NET Core treats this action or property.
    [ValidateAntiForgeryToken]
    // Declares a method or property that other parts of the app can use.
    public async Task<IActionResult> ChangeStatus(int id, ContractStatus status)
    // Marks the beginning or end of a block in C#.
    {
        // Creates a variable and lets C# infer the type from the right-hand side.
        var ok = await _dataService.UpdateContractStatusAsync(id, status);
        TempData["Message"] = ok ? "Contract status updated." : "Contract not found.";
        // Sends a value back to the caller and ends this branch of logic.
        return RedirectToAction(nameof(Index));
    // Marks the beginning or end of a block in C#.
    }

    // Declares a method or property that other parts of the app can use.
    public async Task<IActionResult> Details(int id)
    // Marks the beginning or end of a block in C#.
    {
        // Creates a variable and lets C# infer the type from the right-hand side.
        var contract = await _dataService.GetContractAsync(id);
        // Checks a rule before continuing, so invalid states are stopped early.
        if (contract is null)
        // Marks the beginning or end of a block in C#.
        {
            // Sends a value back to the caller and ends this branch of logic.
            return NotFound();
        // Marks the beginning or end of a block in C#.
        }
        // Sends a value back to the caller and ends this branch of logic.
        return View(contract);
    // Marks the beginning or end of a block in C#.
    }

    // Declares a method or property that other parts of the app can use.
    public async Task<IActionResult> DownloadAgreement(int id)
    // Marks the beginning or end of a block in C#.
    {
        // Creates a variable and lets C# infer the type from the right-hand side.
        var contract = await _dataService.GetContractAsync(id);
        // Checks a rule before continuing, so invalid states are stopped early.
        if (contract is null || string.IsNullOrWhiteSpace(contract.SignedAgreementPath) || !System.IO.File.Exists(contract.SignedAgreementPath))
        // Marks the beginning or end of a block in C#.
        {
            // Sends a value back to the caller and ends this branch of logic.
            return NotFound();
        // Marks the beginning or end of a block in C#.
        }

        // Creates a variable and lets C# infer the type from the right-hand side.
        var fileBytes = await System.IO.File.ReadAllBytesAsync(contract.SignedAgreementPath);
        // Creates a variable and lets C# infer the type from the right-hand side.
        var fileName = contract.SignedAgreementFileName ?? Path.GetFileName(contract.SignedAgreementPath);
        // Sends a value back to the caller and ends this branch of logic.
        return File(fileBytes, "application/pdf", fileName);
    // Marks the beginning or end of a block in C#.
    }
// Marks the beginning or end of a block in C#.
}
