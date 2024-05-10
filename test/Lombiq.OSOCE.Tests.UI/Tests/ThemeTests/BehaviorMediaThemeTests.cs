using Lombiq.Hosting.MediaTheme.Tests.UI.Extensions;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Pages;
using Lombiq.Tests.UI.Services;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.ThemeTests;

public class BehaviorMediaThemeTests : UITestBase
{
    public const string TestTenantName = "test";

    public BehaviorMediaThemeTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task MediaThemeShouldWorkWhenDeployed() =>
        ExecuteTestAfterSetupAsync(async context =>
        {
            await context.TestMediaThemeDeployedBehaviorAsync();
            await CreateAndSwitchToTenantAsync(context);
            await context.TestMediaThemeDeployedBehaviorAsync(TestTenantName);
        });

    [Fact]
    public Task MediaThemeShouldWorkLocally() =>
        ExecuteTestAfterSetupAsync(async context =>
        {
            await context.TestMediaThemeLocalBehaviorAsync();
            await CreateAndSwitchToTenantAsync(context);
            await context.TestMediaThemeLocalBehaviorAsync();
        });

    [Fact]
    public Task MediaThemeTemplateAccessShouldBeBlocked() =>
        ExecuteTestAfterSetupAsync(async context =>
        {
            await CreateAndSwitchToTenantAsync(context);
            await context.TestMediaThemeTemplatePageAsync();
        });

    private static Task CreateAndSwitchToTenantAsync(UITestContext context) =>
        context.CreateAndSwitchToTenantAsync(
            TestTenantName,
            TestTenantName,
            new OrchardCoreSetupParameters
            {
                SiteName = "Media Theme Test Tenant",
                RecipeId = "Lombiq.OSOCE.Tests",
                TablePrefix = TestTenantName,
                UserName = "admin",
            });
}
