using Lombiq.Hosting.MediaTheme.Bridge.Tests.UI.Extensions;
using Lombiq.Hosting.MediaTheme.Tests.UI.Extensions;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Pages;
using Lombiq.Tests.UI.Services;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.NuGet.Tests.UI.Tests.ThemeTests;

public class BehaviorMediaThemeTests : UITestBase
{
    private const string TestTenantName = "test";
    public BehaviorMediaThemeTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task MediaThemeShouldRenderTemplatesFromMediaLibrary() =>
        ExecuteTestAfterSetupAsync(context => context.TestMediaThemeDeployedBehaviorAsync());

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
