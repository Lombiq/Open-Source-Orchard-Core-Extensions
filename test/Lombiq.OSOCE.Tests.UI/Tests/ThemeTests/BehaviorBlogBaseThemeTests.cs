using Lombiq.BaseTheme.Tests.UI.Extensions;
using Lombiq.OSOCE.Tests.UI.Helpers;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using OpenQA.Selenium;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.ThemeTests;

// Different tests with different setups should not be run at the same time, as it upsets the shape table so shapes
// would be seen as missing even when their feature is enabled.
[Collection(nameof(BehaviorBlogBaseThemeTests))]
[CollectionDefinition(nameof(BehaviorBlogBaseThemeTests), DisableParallelization = true)]
public class BehaviorBlogBaseThemeTests : UITestBase
{
    public BehaviorBlogBaseThemeTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task ThemeWithoutSetupShouldWork() =>
        ExecuteTestAfterSetupAndThemeSwitchAsync(
            async context =>
            {
                await context.SignInDirectlyAsync();
                await context.TestBaseThemeDependencyIsEnabledAsync();

                await context.GoToHomePageAsync();
                await context.TestBaseThemeFeaturesAsync(skipLogin: true);
                await context.SignInDirectlyAndGoToHomepageAsync();

                context.TestBlogRecipeMenuItemsAddedToMainMenu();
            });

    [Fact]
    public Task ContentMenuItemShouldWorkCorrectly() =>
        ExecuteTestAfterSetupAndThemeSwitchAsync(
            async context =>
            {
                await context.SignInDirectlyAsync();
                await context.TestAddingMenuItemToBlogMainMenuAsync();
            });

    private Task ExecuteTestAfterSetupAndThemeSwitchAsync(Func<UITestContext, Task> testAsync) =>
        ExecuteTestAsync(
            testAsync,
            async context =>
            {
                var homePageUri = await SetupHelpers.RunBlogSetupAsync(context);

                await context.SignInDirectlyAsync();

                await context.GoToAdminRelativeUrlAsync("/Themes");

                await context.ClickReliablyOnAsync(By.CssSelector(
                    "form[action*='SetCurrentTheme/Lombiq.BaseTheme.Samples'] button"));
                context.ShouldBeSuccess();

                return homePageUri;
            },
            configuration =>
            {
                ChangeConfiguration(configuration);

                // Disable HTML validation, because we have no control over the HTML in the Blog and the content added
                // by the Blog recipe.
                configuration.HtmlValidationConfiguration.RunHtmlValidationAssertionOnAllPageChanges = false;
            });
}
