using Lombiq.BaseTheme.Tests.UI.Extensions;
using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using OpenQA.Selenium;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.ThemeTests;

// Different tests with different setups should not be run on the same set. So this has to be separated from any other
// Base Theme tests to successfully set up using the Blog recipe.
public class BlogBehaviorBaseThemeTests : UITestBase
{
    public BlogBehaviorBaseThemeTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Theory, Chrome]
    public Task ThemeWithoutSetupShouldWork(Browser browser) =>
        ExecuteTestAfterSetupWithBlogRecipeAsync(
            async context =>
            {
                await context.SignInDirectlyAsync();
                await context.GoToRelativeUrlAsync("/Admin/Themes");

                await context.ClickReliablyOnAsync(By.CssSelector(
                    "form[action='/Admin/Themes/SetCurrentTheme/Lombiq.BaseTheme.Samples'] button"));
                context.ShouldBeSuccess();

                // Verify that the feature is indeed enabled.
                await context.GoToRelativeUrlAsync("/Admin/Features");
                await context.ClickAndFillInWithRetriesAsync(By.Id("search-box"), "Helpful Widgets");
                context.Exists(By.Id("btn-disable-Lombiq_HelpfulExtensions_Widgets"));

                await context.GoToHomePageAsync();
                await context.TestBaseThemeFeaturesAsync(skipLogin: true);
            },
            browser);
}
