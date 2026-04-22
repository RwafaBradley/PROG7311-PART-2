// Learning edition: every section is explained inline so the code reads like a guided walkthrough.
// Think of the app like a logistics office: controllers are the reception desk, services are the back office,
// models are the forms, and the database is the filing cabinet used by the back office.

using System.ComponentModel.DataAnnotations;

namespace GLMS.Web.Models;

public class ServiceRequest
{
    public int Id { get; set; }

    [Required]
    public int ContractId { get; set; }

    public Contract? Contract { get; set; }

    [Required]
    [StringLength(250)]
    public string Description { get; set; } = string.Empty;

    [Range(0, 999999999)]
    public decimal UsdAmount { get; set; }

    [Range(0, 999999999)]
    public decimal ExchangeRate { get; set; }

    [Range(0, 999999999)]
    public decimal ZarAmount { get; set; }

    [Required]
    public RequestStatus Status { get; set; } = RequestStatus.New;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
