// Learning edition: every section is explained inline so the code reads like a guided walkthrough.
// Think of the app like a logistics office: controllers are the reception desk, services are the back office,
// models are the forms, and the database context is the organised filing system used by the back office.

using GLMS.Web.Data;
using GLMS.Web.Models;
using GLMS.Web.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GLMS.Web.Services;

public interface IAppDataService
{
    Task<List<Client>> GetClientsAsync();
    Task<Client?> GetClientAsync(int id);
    Task<Client> CreateClientAsync(Client client);

    Task<List<Contract>> GetContractsAsync(string? search, ContractStatus? status, DateTime? startDate, DateTime? endDate);
    Task<Contract?> GetContractAsync(int id);
    Task<Contract> CreateContractAsync(Contract contract);
    Task<bool> UpdateContractStatusAsync(int id, ContractStatus status);
    Task<bool> AttachAgreementAsync(int id, string path, string fileName);

    Task<List<ServiceRequest>> GetServiceRequestsAsync();
    Task<ServiceRequest?> GetServiceRequestAsync(int id);
    Task<ServiceRequest> CreateServiceRequestAsync(ServiceRequest request);

    List<SelectListItem> GetClientSelectList();
    List<SelectListItem> GetActiveContractSelectList();
    DashboardViewModel BuildDashboard(string userName, string userRole, decimal rate);
}

public class AppDataService : IAppDataService
{
    private readonly GlmsDbContext _db;
    private readonly INotificationSubject _notifications;
    private readonly InMemoryStore _store;

    public AppDataService(GlmsDbContext db, INotificationSubject notifications, InMemoryStore store)
    {
        _db = db;
        _notifications = notifications;
        _store = store;
    }

    public async Task<List<Client>> GetClientsAsync()
    {
        return await _db.Clients.AsNoTracking().OrderBy(x => x.Name).ToListAsync();
    }

    public async Task<Client?> GetClientAsync(int id)
    {
        return await _db.Clients.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Client> CreateClientAsync(Client client)
    {
        _db.Clients.Add(client);
        await _db.SaveChangesAsync();
        _notifications.Notify(new NotificationEvent("Client Created", $"Client '{client.Name}' was created.", DateTime.UtcNow));
        return client;
    }

    public async Task<List<Contract>> GetContractsAsync(string? search, ContractStatus? status, DateTime? startDate, DateTime? endDate)
    {
        var query = _db.Contracts.AsNoTracking().Include(x => x.Client).AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(c =>
                EF.Functions.Like(c.ContractNumber, $"%{term}%") ||
                (c.Client != null && EF.Functions.Like(c.Client.Name, $"%{term}%")) ||
                EF.Functions.Like(c.ServiceLevel, $"%{term}%"));
        }

        if (status.HasValue)
        {
            query = query.Where(c => c.Status == status.Value);
        }

        if (startDate.HasValue)
        {
            var from = startDate.Value.Date;
            query = query.Where(c => c.StartDate.Date >= from);
        }

        if (endDate.HasValue)
        {
            var to = endDate.Value.Date;
            query = query.Where(c => c.EndDate.Date <= to);
        }

        return await query.OrderByDescending(x => x.CreatedAtUtc).ToListAsync();
    }

    public async Task<Contract?> GetContractAsync(int id)
    {
        return await _db.Contracts
            .Include(x => x.Client)
            .Include(x => x.ServiceRequests)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Contract> CreateContractAsync(Contract contract)
    {
        var client = await _db.Clients.FirstOrDefaultAsync(x => x.Id == contract.ClientId);
        if (client is null)
        {
            throw new InvalidOperationException("Selected client does not exist.");
        }

        contract.Client = client;
        _db.Contracts.Add(contract);
        await _db.SaveChangesAsync();
        _notifications.Notify(new NotificationEvent("Contract Created", $"Contract '{contract.ContractNumber}' was created.", DateTime.UtcNow));
        return contract;
    }

    public async Task<bool> UpdateContractStatusAsync(int id, ContractStatus status)
    {
        var contract = await _db.Contracts.FirstOrDefaultAsync(x => x.Id == id);
        if (contract is null)
        {
            return false;
        }

        contract.Status = status;
        await _db.SaveChangesAsync();
        _notifications.Notify(new NotificationEvent("Contract Status Changed", $"Contract '{contract.ContractNumber}' moved to {status}.", DateTime.UtcNow));
        return true;
    }

    public async Task<bool> AttachAgreementAsync(int id, string path, string fileName)
    {
        var contract = await _db.Contracts.FirstOrDefaultAsync(x => x.Id == id);
        if (contract is null)
        {
            return false;
        }

        contract.SignedAgreementPath = path;
        contract.SignedAgreementFileName = fileName;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<List<ServiceRequest>> GetServiceRequestsAsync()
    {
        return await _db.ServiceRequests
            .AsNoTracking()
            .Include(x => x.Contract)
            .ThenInclude(x => x.Client)
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToListAsync();
    }

    public async Task<ServiceRequest?> GetServiceRequestAsync(int id)
    {
        return await _db.ServiceRequests
            .AsNoTracking()
            .Include(x => x.Contract)
            .ThenInclude(x => x.Client)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ServiceRequest> CreateServiceRequestAsync(ServiceRequest request)
    {
        var contract = await _db.Contracts.FirstOrDefaultAsync(x => x.Id == request.ContractId);
        if (contract is null)
        {
            throw new InvalidOperationException("Selected contract does not exist.");
        }

        request.Contract = contract;
        _db.ServiceRequests.Add(request);
        await _db.SaveChangesAsync();
        _notifications.Notify(new NotificationEvent("Service Request Created", $"Request #{request.Id} created against contract ID {request.ContractId}.", DateTime.UtcNow));
        return request;
    }

    public List<SelectListItem> GetClientSelectList()
    {
        return _db.Clients.AsNoTracking().OrderBy(x => x.Name)
            .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
            .ToList();
    }

    public List<SelectListItem> GetActiveContractSelectList()
    {
        return _db.Contracts.AsNoTracking()
            .Where(x => x.Status == ContractStatus.Active || x.Status == ContractStatus.Draft)
            .OrderBy(x => x.ContractNumber)
            .Select(x => new SelectListItem($"{x.ContractNumber} ({x.Status})", x.Id.ToString()))
            .ToList();
    }

    public DashboardViewModel BuildDashboard(string userName, string userRole, decimal rate)
    {
        return new DashboardViewModel
        {
            UserName = userName,
            UserRole = userRole,
            ClientCount = _db.Clients.Count(),
            ContractCount = _db.Contracts.Count(),
            ServiceRequestCount = _db.ServiceRequests.Count(),
            CurrentUsdToZarRate = rate,
            Notifications = _store.Notifications.Take(8).ToList()
        };
    }
}
