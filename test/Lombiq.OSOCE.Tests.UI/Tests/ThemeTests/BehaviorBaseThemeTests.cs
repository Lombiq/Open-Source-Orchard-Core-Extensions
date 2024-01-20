using Lombiq.BaseTheme.Tests.UI.Extensions;
using Lombiq.Tests.UI.Extensions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.ThemeTests;

public class BehaviorBaseThemeTests(ITestOutputHelper testOutputHelper) : UITestBase(testOutputHelper)
{
    [Fact]
    public Task ThemeFeaturesShouldWork() =>
        ExecuteTestAfterSetupAsync(
            async context =>
            {
                await context.SignInDirectlyAndGoToHomepageAsync();
                await context.TestBaseThemeFeaturesAsync();

                await context.SignInDirectlyAsync();
                await context.TestBaseThemeSiteSettingsAsync();
            });
}
