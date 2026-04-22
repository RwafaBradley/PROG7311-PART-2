// Learning edition: every section is explained inline so the code reads like a guided walkthrough.
// Think of the app like a logistics office: controllers are the reception desk, services are the back office,
// models are the forms, and the in-memory store is the filing cabinet while the database is still simulated.

// Imports a namespace so this file can use shared classes, services, or framework features.
using GLMS.Web.Models;

// Declares the namespace so the class belongs to the GLMS.Web area.
namespace GLMS.Web.Services;

// Defines a contract that other classes must follow.
public interface IServiceRequestWorkflowService
// Marks the beginning or end of a block in C#.
{
    bool CanCreateRequest(Contract? contract, out string errorMessage);
// Marks the beginning or end of a block in C#.
}

// Defines a class, which groups related data or behavior together.
public class ServiceRequestWorkflowService : IServiceRequestWorkflowService
// Marks the beginning or end of a block in C#.
{
    // Declares a method or property that other parts of the app can use.
    public bool CanCreateRequest(Contract? contract, out string errorMessage)
    // Marks the beginning or end of a block in C#.
    {
        // Checks a rule before continuing, so invalid states are stopped early.
        if (contract is null)
        // Marks the beginning or end of a block in C#.
        {
            errorMessage = "Selected contract does not exist.";
            // Sends a value back to the caller and ends this branch of logic.
            return false;
        // Marks the beginning or end of a block in C#.
        }

        // Checks a rule before continuing, so invalid states are stopped early.
        if (contract.Status is ContractStatus.Expired or ContractStatus.OnHold)
        // Marks the beginning or end of a block in C#.
        {
            errorMessage = "Service requests cannot be created on Expired or On Hold contracts.";
            // Sends a value back to the caller and ends this branch of logic.
            return false;
        // Marks the beginning or end of a block in C#.
        }

        // Checks a rule before continuing, so invalid states are stopped early.
        if (contract.EndDate.Date < DateTime.UtcNow.Date)
        // Marks the beginning or end of a block in C#.
        {
            errorMessage = "This contract has already ended.";
            // Sends a value back to the caller and ends this branch of logic.
            return false;
        // Marks the beginning or end of a block in C#.
        }

        errorMessage = string.Empty;
        // Sends a value back to the caller and ends this branch of logic.
        return true;
    // Marks the beginning or end of a block in C#.
    }
// Marks the beginning or end of a block in C#.
}
