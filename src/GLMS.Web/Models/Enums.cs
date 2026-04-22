// Learning edition: every section is explained inline so the code reads like a guided walkthrough.
// Think of the app like a logistics office: controllers are the reception desk, services are the back office,
// models are the forms, and the in-memory store is the filing cabinet while the database is still simulated.

// Declares the namespace so the class belongs to the GLMS.Web area.
namespace GLMS.Web.Models;

// Defines a fixed list of allowed values, like statuses on a workflow form.
public enum ContractStatus
// Marks the beginning or end of a block in C#.
{
    Draft = 0,
    Active = 1,
    Expired = 2,
    OnHold = 3
// Marks the beginning or end of a block in C#.
}

// Defines a fixed list of allowed values, like statuses on a workflow form.
public enum RequestStatus
// Marks the beginning or end of a block in C#.
{
    New = 0,
    UnderReview = 1,
    Approved = 2,
    Rejected = 3,
    Completed = 4
// Marks the beginning or end of a block in C#.
}
