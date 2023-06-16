using Lombiq.BaseTheme.Tests.UI.Extensions;
using Lombiq.OSOCE.Tests.UI.Helpers;
using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using OpenQA.Selenium;
using Shouldly;
using System;
using System.Globalization;
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
                await context.SignInDirectlyAndGoToHomepageAsync();

                // Verify the menu items added by the Blog recipe.
                context.Get(By.CssSelector(".menuWidget__content .nav-link[href='/']")).Text.Trim().ShouldBe("Home");
                context.Get(By.CssSelector(".menuWidget__content .nav-link[href='/about']")).Text.Trim().ShouldBe("About");
            },
            browser);

    [Theory, Chrome]
    public Task ContentMenuItemShouldWorkCorrectly(Browser browser) =>
        ExecuteTestAfterSetupAndThemeSwitchAsync(
            async context =>
            {
                await context.SignInDirectlyAsync();
                await context.GoToRelativeUrlAsync("/Admin/Contents/ContentItems/Menu");
                await context.ClickReliablyOnAsync(By.ClassName("edit"));

                await context.ClickReliablyOnAsync(By.XPath($"//button[contains(., 'Add Menu Item')]"));
                await context.ClickReliablyOnAsync(By.XPath("//div[contains(@class, 'card') and .//h4[contains(., 'Content Menu Item')]]//div[contains(@class, 'card-footer')]//a"));
                await context.ClickAndFillInWithRetriesAsync(By.Id("ContentMenuItemPart_Name"), "My Content");

                // Find content item index by display text and select it from the content picker..
                context.ExecuteScript(@"
                    fetch('/Admin/ContentFields/SearchContentItems?part=ContentMenuItemPart&field=SelectedContentItem')
                        .then((response) => response.json())
                        .then((json) => json
                            .map((item, index) => { item.index = index; return item })
                            .filter((item) => item.displayText === 'Man must explore, and this is exploration at its greatest')[0]
                            .index)
                        .then((index) => {
                            const div = document.createElement('div');
                            div.id = 'target-index';
                            div.innerHTML = index;
                            document.querySelector('.ta-content').appendChild(div);
                        });");
                var index = int.Parse(context.Get(By.Id("target-index")).Text.Trim(), CultureInfo.InvariantCulture);
                await context.SetContentPickerByIndexAsync("ContentMenuItemPart", "SelectedContentItem", index);

                await context.ClickPublishAsync();
                await context.ClickPublishAsync();
                context.ShouldBeSuccess();

                await context.GoToHomePageAsync();
                context
                    .Get(By.XPath("id('navigation')//li[contains(@class, 'menuWidget__topLevel')]/a[@href='/blog/post-1']"))
                    .Text
                    .Trim()
                    .ShouldBe("My Content");
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
