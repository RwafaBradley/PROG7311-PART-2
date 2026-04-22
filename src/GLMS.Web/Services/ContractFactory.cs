// Learning edition: every section is explained inline so the code reads like a guided walkthrough.
// Think of the app like a logistics office: controllers are the reception desk, services are the back office,
// models are the forms, and the in-memory store is the filing cabinet while the database is still simulated.

// Imports a namespace so this file can use shared classes, services, or framework features.
using GLMS.Web.Models;

// Declares the namespace so the class belongs to the GLMS.Web area.
namespace GLMS.Web.Services;

// Defines a contract that other classes must follow.
public interface IContractFactory
// Marks the beginning or end of a block in C#.
{
    Contract Create(int clientId, string contractNumber, DateTime startDate, DateTime endDate, ContractStatus status, string serviceLevel);
// Marks the beginning or end of a block in C#.
}

// Defines a class, which groups related data or behavior together.
public class ContractFactory : IContractFactory
// Marks the beginning or end of a block in C#.
{
    // Declares a method or property that other parts of the app can use.
    public Contract Create(int clientId, string contractNumber, DateTime startDate, DateTime endDate, ContractStatus status, string serviceLevel)
    // Marks the beginning or end of a block in C#.
    {
        // Sends a value back to the caller and ends this branch of logic.
        return new Contract
        // Marks the beginning or end of a block in C#.
        {
            ClientId = clientId,
            ContractNumber = contractNumber.Trim(),
            StartDate = startDate.Date,
            EndDate = endDate.Date,
            Status = status,
            ServiceLevel = serviceLevel.Trim()
        };
    // Marks the beginning or end of a block in C#.
    }
// Marks the beginning or end of a block in C#.
}
