using Lombiq.BaseTheme.Tests.UI.Extensions;
using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Pages;
using Lombiq.Tests.UI.Services;
using OpenQA.Selenium;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.ThemeTests;

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
    public Task ThemeWithoutSetupShouldWork(Browser browser) =>
        ExecuteTestAsync(
            async context =>
            {
                await context.SignInDirectlyAsync();
                await context.GoToRelativeUrlAsync("/Admin/Themes");

                await context.ClickReliablyOnAsync(By.CssSelector(
                    "form[action='/Admin/Themes/SetCurrentTheme/Lombiq.BaseTheme.Samples'] button"));
                context.ShouldBeSuccess();

                await context.GoToHomePageAsync();
                await context.TestBaseThemeFeaturesAsync();
            },
            browser,
            async context =>
            {
                // We explicitly add a custom configuration with the "Blog" setup to test if the feature works when we
                // add it to an existing site.
                var homepageUri = await context.GoToSetupPageAndSetupOrchardCoreAsync(
                    new OrchardCoreSetupParameters(context)
                    {
                        SiteName = "Lombiq's OSOCE - UI Testing - With Blog Setup",
                        RecipeId = "Blog",
                        TablePrefix = "OSOCE_blog",
                        SiteTimeZoneValue = "Europe/Budapest",
                    });

                return homepageUri;
            },
            configuration =>
            {
                ChangeConfiguration(configuration);
                return Task.CompletedTask;
            });
}
