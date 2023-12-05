using Lombiq.Hosting.Tenants.Maintenance.Tests.UI.Extensions;
using Lombiq.OSOCE.NuGet.Tests.UI.Helpers;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.NuGet.Tests.UI.Tests.ModuleTests;

public class BehaviorMaintenanceTests : UITestBase
{
    public BehaviorMaintenanceTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    // This test uses ExecuteTestAsync with a different setup delegate instead of ExecuteTestAfterSetupAsync because the
    // maintenance does changes to the DB on startup only necessary for this test (like depersonalizing user accounts).
    // This would occasionally lead to failures in subsequent tests if this was the first test to run and thus create
    // the DB snapshot after running the setup.
    [Fact]
    public Task ChangeUserSensitiveContentMaintenanceTaskShouldBeExecutedSuccessfully() =>
        ExecuteTestAsync(
            context => context.ChangeUserSensitiveContentMaintenanceExecutionAsync(),
            async context =>
            {
                var homepageUri = await SetupHelpers.RunSetupAsync(context);

                return homepageUri;
            },
            configuration => configuration.ChangeUserSensitiveContentMaintenanceConfiguration());
}
