using Lombiq.Hosting.Tenants.Maintenance.Tests.UI.Extensions;
using Lombiq.OSOCE.NuGet.Tests.UI.Helpers;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.NuGet.Tests.UI.Tests.ModuleTests;

public class BehaviorMaintenanceTests : UITestBase
{
    public BehaviorMaintenanceTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    // This test uses a different setup because the maintenance does changes to the DB on startup only necessary for
    // this test (like depersonalizing user accounts). This would occasionally cause failures in subsequent tests.
    [Fact]
    public Task ChangeUserSensitiveContentMaintenanceTaskShouldBeExecutedSuccessfully() => TestBuilder
        .UseSetup(SetupHelpers.RunSetupAsync)
        .AddConfiguration(configuration => configuration.ChangeUserSensitiveContentMaintenanceConfiguration())
        .RunAsync(context => context.ChangeUserSensitiveContentMaintenanceExecutionAsync());
}
