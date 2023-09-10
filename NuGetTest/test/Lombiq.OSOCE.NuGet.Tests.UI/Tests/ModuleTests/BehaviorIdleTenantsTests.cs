using Lombiq.Hosting.Tenants.IdleTenantManagement.Tests.UI.Extensions;
using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using static Lombiq.OSOCE.NuGet.Tests.UI.Constants.RecipeIds;

namespace Lombiq.OSOCE.NuGet.Tests.UI.Tests.ModuleTests;

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
                await context.SignInDirectlyAsync();

                await context.TestIdleTenantManagerBehaviorAsync(NugetRecipeId);

                context.Configuration.AssertAppLogsAsync = webApplicationInstance =>
                    IdleTenantManagementExtensions.AssertAppLogsWithIdleCheckAsync(webApplicationInstance);
            },
            browser,
            configuration => configuration.SetMaxIdleMinutesAndLoggingForUITest());
}
