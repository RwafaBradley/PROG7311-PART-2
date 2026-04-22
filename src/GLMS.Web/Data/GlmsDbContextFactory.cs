// Learning edition: every section is explained inline so the code reads like a guided walkthrough.
// Think of the app like a logistics office: this factory is the spare key that helps EF Core create migrations.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace GLMS.Web.Data;

public class GlmsDbContextFactory : IDesignTimeDbContextFactory<GlmsDbContext>
{
    public GlmsDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? "Server=(localdb)\\MSSQLLocalDB;Database=GLMS_Prototype;Trusted_Connection=True;TrustServerCertificate=True;";

        var optionsBuilder = new DbContextOptionsBuilder<GlmsDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new GlmsDbContext(optionsBuilder.Options);
    }
}
