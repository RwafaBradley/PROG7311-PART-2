// Learning edition: every section is explained inline so the code reads like a guided walkthrough.
// Think of the app like a logistics office: controllers are the reception desk, services are the back office,
// models are the forms, and the database context is the organised filing system used by the back office.

using GLMS.Web.Data;
using GLMS.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace GLMS.Web.Services;

public static class SeedData
{
    private static bool _initialised;

    public static void Initialize(IServiceProvider services)
    {
        if (_initialised)
        {
            return;
        }

        var db = services.GetRequiredService<GlmsDbContext>();
        var configuration = services.GetRequiredService<IConfiguration>();

        if (!db.Users.Any())
        {
            var seedUsers = configuration.GetSection("SeedUsers").Get<List<AppUser>>() ?? new List<AppUser>();
            db.Users.AddRange(seedUsers);
            db.SaveChanges();
        }

        if (!db.Clients.Any())
        {
            var client1 = new Client
            {
                Name = "Cape Freight Group",
                ContactDetails = "operations@capefreight.co.za",
                Region = "Western Cape"
            };

            var client2 = new Client
            {
                Name = "Johannesburg Cargo Hub",
                ContactDetails = "logistics@jcg.co.za",
                Region = "Gauteng"
            };

            db.Clients.AddRange(client1, client2);
            db.SaveChanges();

            db.Contracts.AddRange(
                new Contract
                {
                    ClientId = client1.Id,
                    ContractNumber = "GLMS-CTR-001",
                    StartDate = DateTime.UtcNow.Date.AddMonths(-2),
                    EndDate = DateTime.UtcNow.Date.AddMonths(10),
                    Status = ContractStatus.Active,
                    ServiceLevel = "Premium",
                    SignedAgreementFileName = "sample-agreement.pdf"
                },
                new Contract
                {
                    ClientId = client2.Id,
                    ContractNumber = "GLMS-CTR-002",
                    StartDate = DateTime.UtcNow.Date.AddMonths(-1),
                    EndDate = DateTime.UtcNow.Date.AddDays(20),
                    Status = ContractStatus.Draft,
                    ServiceLevel = "Standard"
                });

            db.SaveChanges();
        }

        _initialised = true;
    }
}
