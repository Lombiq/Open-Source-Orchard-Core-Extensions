using Lombiq.Hosting.Tenants.IdleTenantManagement.Tests.UI.Extensions;
using Lombiq.Tests.UI.Extensions;
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

    [Fact]
    public Task ShuttingDownIdleTenantsShouldWork() =>
        ExecuteTestAfterSetupAsync(
            async context =>
            {
                await context.SignInDirectlyAsync();

                await context.TestIdleTenantManagerBehaviorAsync(TestsSetupRecipeId);

                context.Configuration.AssertAppLogsAsync = webApplicationInstance =>
                    IdleTenantManagementExtensions.AssertAppLogsWithIdleCheckAsync(webApplicationInstance);
            },
            configuration => configuration.SetMaxIdleMinutesAndLoggingForUITest());
}
