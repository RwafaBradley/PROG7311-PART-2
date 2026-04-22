// Learning edition: every section is explained inline so the code reads like a guided walkthrough.
// Think of the app like a logistics office: controllers are the reception desk, services are the back office,
// models are the forms, and the database is the filing cabinet used by the back office.

using System.ComponentModel.DataAnnotations;

namespace GLMS.Web.Models;

public class Contract
{
    public int Id { get; set; }

    [Required]
    public int ClientId { get; set; }

    public Client? Client { get; set; }

    [Required]
    [StringLength(60)]
    public string ContractNumber { get; set; } = string.Empty;

    [Required]
    public DateTime StartDate { get; set; } = DateTime.UtcNow.Date;

    [Required]
    public DateTime EndDate { get; set; } = DateTime.UtcNow.Date.AddMonths(12);

    [Required]
    public ContractStatus Status { get; set; } = ContractStatus.Draft;

    [Required]
    [StringLength(200)]
    public string ServiceLevel { get; set; } = "Standard";

    [StringLength(260)]
    public string? SignedAgreementPath { get; set; }

    [StringLength(100)]
    public string? SignedAgreementFileName { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public List<ServiceRequest> ServiceRequests { get; set; } = new();
}
