using Lombiq.Hosting.Tenants.Maintenance.Tests.UI.Extensions;
using Lombiq.Tests.UI.Samples.Helpers;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

public class BehaviorMaintenanceTests : UITestBase
{
    public BehaviorMaintenanceTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task UpdateSiteUrlMaintenanceTaskShouldBeExecutedSuccessfully() =>
        ExecuteTestAfterSetupAsync(
            context => context.TestSiteUrlMaintenanceExecutionAsync(),
            configuration => configuration.SetUpdateSiteUrlMaintenanceConfiguration());

    [Fact]
    public Task AddAdministratorRoleToUsersWithRoleMaintenanceTaskShouldBeExecutedSuccessfully() =>
        ExecuteTestAfterSetupAsync(
            context => context.TestAdministratorRoleToUsersWithRoleMaintenanceExecutionAsync(),
            configuration => configuration.SetAddAdministratorRoleToUsersWithRoleConfiguration());

    // This test uses a different setup because the maintenance does changes to the DB on startup only necessary for
    // this test (like depersonalizing user accounts). This would occasionally cause failures in subsequent tests.
    [Fact]
    public Task ChangeUserSensitiveContentMaintenanceTaskShouldBeExecutedSuccessfully() =>
        UseSetup(SetupHelpers.RunSetupAsync)
            .AddSyncConfiguration(configuration => configuration.ChangeUserSensitiveContentMaintenanceConfiguration())
            .RunAsync(context => context.ChangeUserSensitiveContentMaintenanceExecutionAsync());
}
