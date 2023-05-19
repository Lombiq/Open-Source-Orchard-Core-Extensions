using Lombiq.OSOCE.Tests.UI.Helpers;
using Lombiq.Tests.UI;
using Lombiq.Tests.UI.Constants;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Pages;
using Lombiq.Tests.UI.Samples.Helpers;
using Lombiq.Tests.UI.Services;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI;

public class UITestBase : OrchardCoreUITestBase<Program>
{
    protected UITestBase(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    protected override Task ExecuteTestAfterSetupAsync(
        Func<UITestContext, Task> testAsync,
        Browser browser,
        Func<OrchardCoreUITestExecutorConfiguration, Task> changeConfigurationAsync) =>
        ExecuteTestAsync(testAsync, browser, SetupHelpers.RunSetupAsync, changeConfigurationAsync);

    protected override Task ExecuteTestAsync(
        Func<UITestContext, Task> testAsync,
        Browser browser,
        Func<UITestContext, Task<Uri>> setupOperation,
        Func<OrchardCoreUITestExecutorConfiguration, Task> changeConfigurationAsync) =>
        base.ExecuteTestAsync(
            testAsync,
            browser,
            setupOperation,
            async configuration =>
            {
                ChangeConfiguration(configuration);
                if (changeConfigurationAsync != null) await changeConfigurationAsync(configuration);
            });

    protected Task ExecuteTestAfterSetupWithBlogRecipeAsync(
        Func<UITestContext, Task> testAsync,
        Browser browser,
        Action<OrchardCoreUITestExecutorConfiguration> changeConfiguration = null) =>
        ExecuteTestAsync(
            testAsync,
            browser,
            async context =>
            {
                // We must use a custom configuration with the "Blog" setup to test if the feature works when enabled on
                // an existing stock site without setup pre-configuration.
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

                // Disable HTML validation, because we have no control over the HTML in the Blog and the content added
                // by the Blog recipe.
                configuration.HtmlValidationConfiguration.RunHtmlValidationAssertionOnAllPageChanges = false;

                changeConfiguration?.Invoke(configuration);

                return Task.CompletedTask;
            });

    protected void ChangeConfiguration(OrchardCoreUITestExecutorConfiguration configuration)
    {
        configuration.BrowserConfiguration.DefaultBrowserSize = CommonDisplayResolutions.HdPlus;

        configuration.BrowserConfiguration.Headless =
            TestConfigurationManager.GetBoolConfiguration("BrowserConfiguration:Headless", defaultValue: false);

        configuration.AssertAppLogsAsync = AssertAppLogsHelpers.AssertOsoceAppLogsAreEmptyAsync;
    }

    public static readonly Func<IWebApplicationInstance, Task> AssertAppLogsDefaultOSOCEAsync =
        async webApplicationInstance =>
            (await webApplicationInstance.GetLogOutputAsync())
            .ReplaceOrdinalIgnoreCase(
                "|Lombiq.TrainingDemo.Services.DemoBackgroundTask|ERROR|Expected non-error",
                "|Lombiq.TrainingDemo.Services.DemoBackgroundTask|EXPECTED_ERROR|Expected non-error")
            .ReplaceOrdinalIgnoreCase(
                "|OrchardCore.Media.Core.DefaultMediaFileStoreCacheFileProvider|ERROR|Error deleting cache folder",
                "|OrchardCore.Media.Core.DefaultMediaFileStoreCacheFileProvider|EXPECTED_ERROR|Error deleting cache folder")
            .ShouldNotContain("|ERROR|");
}
