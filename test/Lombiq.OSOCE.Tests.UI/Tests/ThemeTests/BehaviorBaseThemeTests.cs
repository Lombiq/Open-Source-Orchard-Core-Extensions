using Lombiq.BaseTheme.Tests.UI.Extensions;
using Lombiq.Tests.UI.Extensions;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.Tests.UI.Tests.ThemeTests;

public class BehaviorBaseThemeTests : UITestBase
{
    public BehaviorBaseThemeTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task ThemeFeaturesShouldWork() =>
        AddSyncConfiguration(configuration => configuration.SetupConfiguration.SetupWithHttpClient = false)
            .RunAsync(async context =>
            {
                await context.SignInDirectlyAsync();
                await context.TestBaseThemeFeaturesAsync();

                await context.SignInDirectlyAsync();
                await context.TestBaseThemeSiteSettingsAsync();
            });
}
