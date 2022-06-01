using Lombiq.BaseTheme.Tests.UI.Extensions;
using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using OpenQA.Selenium;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests;

public class BehaviorBaseThemeTests : UITestBase
{
    public BehaviorBaseThemeTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Theory, Chrome]
    public Task ThemeFeaturesShouldWork(Browser browser) =>
        ExecuteTestAfterSetupAsync(
            async context =>
            {
                await context.SignInDirectlyAndGoToHomepageAsync();
                await context.TestBaseThemeFeaturesAsync();
            },
            browser);

    [Theory, Chrome]
    public Task TestAdminBackgroundTasksAsMonkeyRecursivelyShouldWorkWithAdminUser(
        Browser browser) =>
        ExecuteTestAfterSetupAsync(
            async context =>
            {
                if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return;
                await context.GoToRelativeUrlAsync("/nasdjklandasjlasjlsd");
                await context.GoToHomePageAsync();
                context.Get(By.Id("nasdjklandasjlasjlsd"));
            },
            browser);
}
