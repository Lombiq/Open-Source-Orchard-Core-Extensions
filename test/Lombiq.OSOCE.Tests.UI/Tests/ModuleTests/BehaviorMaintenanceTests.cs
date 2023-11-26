using Lombiq.Hosting.Tenants.Maintenance.Tests.UI.Extensions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

public class BehaviorMaintenanceTests : UITestBase
{
    public BehaviorMaintenanceTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task MaintenanceTaskShouldBeExecutedSuccessfully() =>
        ExecuteTestAfterSetupAsync(
            context => context.TestSiteUrlMaintenanceExecutionAsync(),
            configuration => configuration.SetUpdateSiteUrlMaintenanceConfiguration());

    [Fact]
    public Task AddSiteOwnerPermissionToRoleMaintenanceTaskShouldBeExecutedSuccessfully() =>
        ExecuteTestAfterSetupAsync(
            context => context.TestSiteOwnerPermissionToRoleMaintenanceExecutionAsync(),
            configuration => configuration.SetAddSiteOwnerPermissionToRoleMaintenanceConfiguration());

    [Fact]
    public Task ChangeUserSensitiveContentMaintenanceTaskShouldBeExecutedSuccessfully() =>
        ExecuteTestAfterSetupAsync(
            context => context.ChangeUserSensitiveContentMaintenanceExecutionAsync(),
            configuration => configuration.ChangeUserSensitiveContentMaintenanceConfiguration());
}
