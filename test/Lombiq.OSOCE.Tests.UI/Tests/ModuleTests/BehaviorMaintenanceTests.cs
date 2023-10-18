using Lombiq.Hosting.Tenants.Maintenance.Tests.UI.Extensions;
using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Services;
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

    [Theory, Chrome]
    public Task MaintenanceTaskShouldBeExecutedSuccessfully(Browser browser) =>
        ExecuteTestAfterSetupAsync(
            async context => await context.TestSiteUrlMaintenanceExecutionAsync(),
            browser,
            configuration => configuration.SetUpdateSiteUrlMaintenanceConfiguration());

    [Theory, Chrome]
    public Task AddSiteOwnerPermissionToRoleMaintenanceTaskShouldBeExecutedSuccessfully(Browser browser) =>
        ExecuteTestAfterSetupAsync(
            async context => await context.TestSiteOwnerPermissionToRoleMaintenanceExecutionAsync(),
            browser,
            configuration => configuration.SetAddSiteOwnerPermissionToRoleMaintenanceConfiguration());

    [Theory, Chrome]
    public Task ChangeUserSensitiveContentMaintenanceTaskShouldBeExecutedSuccessfully(Browser browser) =>
        ExecuteTestAfterSetupAsync(
            async context => await context.ChangeUserSensitiveContentMaintenanceExecutionAsync(),
            browser,
            configuration => configuration.ChangeUserSensitiveContentMaintenanceConfiguration());
}
