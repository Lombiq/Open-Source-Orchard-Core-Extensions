using Lombiq.Hosting.Tenants.IdleTenantManagement.Tests.UI.Extensions;
using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Services;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

public class BehaviorIdleTenantsTests : UITestBase
{
    public BehaviorIdleTenantsTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Theory, Chrome]
    public Task ShuttingDownIdleTenantsShouldWork(Browser browser) =>
        ExecuteTestAfterSetupAsync(
            async context =>
            {
                await context.TestIdleTenantManagerBehaviorAsync();

                context.Configuration.AssertAppLogsAsync = async webApplicationInstance =>
                {
                    await AssertAppLogsDefaultOSOCEAsync(webApplicationInstance);
                    await IdleTenantManagementExtensions.AssertAppLogsWithIdleCheckAsync(webApplicationInstance);
                };
            },
            browser,
            configuration => configuration.SetMaxIdleMinutesAndLoggingForUITest());
}
