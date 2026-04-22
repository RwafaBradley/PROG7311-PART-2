// Learning edition: every section is explained inline so the code reads like a guided walkthrough.
// Think of the app like a logistics office: controllers are the reception desk, services are the back office,
// models are the forms, and the database is the filing cabinet used by the back office.

using System.ComponentModel.DataAnnotations;

namespace GLMS.Web.Models;

public class Client
{
    public int Id { get; set; }

    [Required]
    [StringLength(120)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(160)]
    public string ContactDetails { get; set; } = string.Empty;

    [Required]
    [StringLength(80)]
    public string Region { get; set; } = string.Empty;

    public List<Contract> Contracts { get; set; } = new();
}
