using Lombiq.Hosting.Tenants.IdleTenantManagement.Tests.UI.Extensions;
using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Services;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.NuGet.Tests.UI.Tests;

public class IdleTenantTests : UITestBase
{
    public IdleTenantTests(ITestOutputHelper testOutputHelper)
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
                    await IdleTenantManagementExtensions.AssertAppLogsWithIdleCheckAsync(webApplicationInstance);
                };
            },
            browser,
            configuration => configuration.SetMaxIdleMinutesAndLoggingForUITest());
}
