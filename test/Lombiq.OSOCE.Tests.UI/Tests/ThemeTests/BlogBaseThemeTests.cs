using Lombiq.BaseTheme.Tests.UI.Extensions;
using Lombiq.OSOCE.Tests.UI.Helpers;
using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using OpenQA.Selenium;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.ThemeTests;

// Different tests with different setups should not be run at the same time, as it upsets the shape table so shapes
// would be seen as missing even when their feature is enabled.
[Collection(nameof(BlogBehaviorBaseThemeTests))]
[CollectionDefinition(nameof(BlogBehaviorBaseThemeTests), DisableParallelization = true)]
public class BlogBehaviorBaseThemeTests : UITestBase
{
    public BlogBehaviorBaseThemeTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Theory, Chrome]
    public Task ThemeWithoutSetupShouldWork(Browser browser) =>
        ExecuteTestAfterSetupAndThemeSwitchAsync(
            async context =>
            {
                await context.SignInDirectlyAsync();

                // Verify that the feature is indeed enabled.
                await context.GoToRelativeUrlAsync("/Admin/Features");
                await context.ClickAndFillInWithRetriesAsync(By.Id("search-box"), "Helpful Widgets");
                context.Exists(By.Id("btn-disable-Lombiq_HelpfulExtensions_Widgets"));

                await context.GoToHomePageAsync();
                await context.TestBaseThemeFeaturesAsync(skipLogin: true);

                // Verify the menu items added by the Blog recipe.
                context.Get(By.CssSelector(".menuWidget__content .nav-link[href='/']")).Text.Trim().ShouldBe("Home");
                context.Get(By.CssSelector(".menuWidget__content .nav-link[href='/about']")).Text.Trim().ShouldBe("About");
            },
            browser);

    private Task ExecuteTestAfterSetupAndThemeSwitchAsync(Func<UITestContext, Task> testAsync, Browser browser) =>
        ExecuteTestAsync(
            testAsync,
            browser,
            async context =>
            {
                var homePageUri = await SetupHelpers.RunBlogSetupAsync(context);

                await context.SignInDirectlyAsync();
                await context.GoToRelativeUrlAsync("/Admin/Themes");

                await context.ClickReliablyOnAsync(By.CssSelector(
                    "form[action='/Admin/Themes/SetCurrentTheme/Lombiq.BaseTheme.Samples'] button"));
                context.ShouldBeSuccess();

                return homePageUri;
            },
            configuration =>
            {
                ChangeConfiguration(configuration);

                // Disable HTML validation, because we have no control over the HTML in the Blog and the content added
                // by the Blog recipe.
                configuration.HtmlValidationConfiguration.RunHtmlValidationAssertionOnAllPageChanges = false;

                return Task.CompletedTask;
            });
}
