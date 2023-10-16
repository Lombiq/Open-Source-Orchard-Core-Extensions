using Lombiq.Hosting.Tenants.Maintenance.Tests.UI.Extensions;
using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Services;
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

    [Theory, Chrome]
    public Task ChangeUserSensitiveContentMaintenanceTaskShouldBeExecutedSuccessfully(Browser browser) =>
        ExecuteTestAfterSetupAsync(
            async context => await context.ChangeUserSensitiveContentMaintenanceExecutionAsync(),
            browser,
            configuration => configuration.ChangeUserSensitiveContentMaintenanceConfiguration());
}
