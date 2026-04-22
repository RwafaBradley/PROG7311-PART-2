// Learning edition: every section is explained inline so the code reads like a guided walkthrough.
// Think of the app like a logistics office: this context is the organised filing cabinet for the SQL database.

using GLMS.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace GLMS.Web.Data;

public class GlmsDbContext : DbContext
{
    public GlmsDbContext(DbContextOptions<GlmsDbContext> options) : base(options)
    {
    }

    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Contract> Contracts => Set<Contract>();
    public DbSet<ServiceRequest> ServiceRequests => Set<ServiceRequest>();
    public DbSet<AppUser> Users => Set<AppUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).HasMaxLength(120).IsRequired();
            entity.Property(x => x.ContactDetails).HasMaxLength(160).IsRequired();
            entity.Property(x => x.Region).HasMaxLength(80).IsRequired();

            entity.HasMany(x => x.Contracts)
                  .WithOne(x => x.Client)
                  .HasForeignKey(x => x.ClientId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Contract>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.ContractNumber).HasMaxLength(60).IsRequired();
            entity.Property(x => x.ServiceLevel).HasMaxLength(200).IsRequired();
            entity.Property(x => x.SignedAgreementPath).HasMaxLength(260);
            entity.Property(x => x.SignedAgreementFileName).HasMaxLength(100);

            entity.HasMany(x => x.ServiceRequests)
                  .WithOne(x => x.Contract)
                  .HasForeignKey(x => x.ContractId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ServiceRequest>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Description).HasMaxLength(250).IsRequired();
            entity.Property(x => x.UsdAmount).HasPrecision(18, 2);
            entity.Property(x => x.ExchangeRate).HasPrecision(18, 6);
            entity.Property(x => x.ZarAmount).HasPrecision(18, 2);
        });

        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.HasKey(x => x.Username);
            entity.Property(x => x.Username).HasMaxLength(80).IsRequired();
            entity.Property(x => x.Password).HasMaxLength(120).IsRequired();
            entity.Property(x => x.Role).HasMaxLength(40).IsRequired();
            entity.Property(x => x.FullName).HasMaxLength(120).IsRequired();
        });
    }
}
