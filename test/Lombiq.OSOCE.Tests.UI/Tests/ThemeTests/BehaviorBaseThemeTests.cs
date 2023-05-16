using Lombiq.BaseTheme.Tests.UI.Extensions;
using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Pages;
using Lombiq.Tests.UI.Services;
using OpenQA.Selenium;
using Shouldly;
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

                // Verify the menu items added by the Blog recipe.
                context.Get(By.CssSelector(".menuWidget__content .nav-link[href='/']")).Text.Trim().ShouldBe("Home");
                context.Get(By.CssSelector(".menuWidget__content .nav-link[href='/about']")).Text.Trim().ShouldBe("About");

                await context.TestBaseThemeFeaturesAsync(skipLogin: true);
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
