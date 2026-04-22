using GLMS.Web.Models;
using GLMS.Web.Services;
using Xunit;

namespace GLMS.Web.Tests;

public class WorkflowTests
{
    [Theory]
    [InlineData(ContractStatus.Expired)]
    [InlineData(ContractStatus.OnHold)]
    public void CanCreateRequest_ShouldRejectExpiredOrOnHoldContracts(ContractStatus status)
    {
        var workflow = new ServiceRequestWorkflowService();

        var contract = new Contract
        {
            Status = status,
            EndDate = DateTime.UtcNow.Date.AddDays(1)
        };

        var ok = workflow.CanCreateRequest(contract, out var error);

        Assert.False(ok);
        Assert.False(string.IsNullOrWhiteSpace(error));
    }

    [Fact]
    public void CanCreateRequest_ShouldAllowActiveContract()
    {
        var workflow = new ServiceRequestWorkflowService();

        var contract = new Contract
        {
            Status = ContractStatus.Active,
            EndDate = DateTime.UtcNow.Date.AddDays(30)
        };

        var ok = workflow.CanCreateRequest(contract, out var error);

        Assert.True(ok);
        Assert.True(string.IsNullOrWhiteSpace(error));
    }
}
