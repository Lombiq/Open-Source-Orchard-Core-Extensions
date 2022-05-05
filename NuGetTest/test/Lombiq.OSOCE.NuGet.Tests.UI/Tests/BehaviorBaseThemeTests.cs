using Lombiq.BaseTheme.Tests.UI.Extensions;
using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using OpenQA.Selenium;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.NuGet.Tests.UI.Tests;

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
                await context.SignInDirectlyAsync();

                // Run layers & zones recipe, and enable Lombiq.BaseTheme.Samples theme.
                await context.ExecuteRecipeDirectlyAsync("Lombiq.BaseTheme.LayersAndZones");
                await context.GoToRelativeUrlAsync("/Admin/Themes");
                await context.ClickReliablyOnAsync(
                    By.CssSelector("form[action='/Admin/Themes/SetCurrentTheme/Lombiq.BaseTheme.Samples'] button"));

                // Execute tests on the home page using the sample theme.
                await context.GoToHomePageAsync();
                await context.TestBaseThemeFeaturesAsync();
            },
            browser);
}
