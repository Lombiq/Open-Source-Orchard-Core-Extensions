using Lombiq.Hosting.Tenants.IdleTenantManagement.Tests.UI.Extensions;
using Lombiq.Tests.UI.Extensions;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

public class BehaviorIdleTenantsTests : UITestBase
{
    public BehaviorIdleTenantsTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper) =>
        AddConfiguration(configuration => configuration.ConfigureIdleTenantManagementTestSettings());

    [Fact]
    public Task ShuttingDownIdleTenantsShouldWork() =>
        RunAsync(async context =>
        {
            await context.SignInDirectlyAsync();
            await context.TestIdleTenantManagerBehaviorAsync();
        });
}
