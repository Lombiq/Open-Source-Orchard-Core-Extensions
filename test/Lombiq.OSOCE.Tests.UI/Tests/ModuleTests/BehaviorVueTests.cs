using Atata.HtmlValidation;
using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using Lombiq.VueJs.Samples.Controllers;
using Lombiq.VueJs.Tests.UI.Extensions;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

public class BehaviorVueTests : UITestBase
{
    public BehaviorVueTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Theory, Chrome]
    public Task RecipeDataShouldBeDisplayedCorrectly(Browser browser) =>
        ExecuteTestAfterSetupAsync(
            async context =>
            {
                await context.SignInDirectlyAsync();
                await context.TestVueSampleBehaviorAsync();
            },
            browser);

    // See Skip comment.
#pragma warning disable xUnit1004 // Test methods should not be skipped
    [Theory(Skip = "Fails in dev, skipped until we fix this: https://github.com/Lombiq/Orchard-Vue.js/issues/93"), Chrome]
#pragma warning restore xUnit1004 // Test methods should not be skipped
    public Task QrCardScanShouldWorkAsync(Browser browser) =>
        ExecuteTestAfterSetupAsync(
            context => context.TestQrCardFoundAsync(),
            browser,
            configuration =>
            {
                configuration.BrowserConfiguration.ConfigureFakeVideoSourceForPositiveTest();
                configuration.HtmlValidationConfiguration.AssertHtmlValidationResultAsync = AssertHtmValidationResultAsync;
            });

    // See Skip comment.
#pragma warning disable xUnit1004 // Test methods should not be skipped
    [Theory(Skip = "Fails in dev, skipped until we fix this: https://github.com/Lombiq/Orchard-Vue.js/issues/93"), Chrome]
#pragma warning restore xUnit1004 // Test methods should not be skipped
    public Task QrCardScanShouldReportNotFoundAsync(Browser browser) =>
        ExecuteTestAfterSetupAsync(
            context => context.TestQrCardNotFoundAsync(),
            browser,
            configuration =>
            {
                configuration.BrowserConfiguration.ConfigureFakeVideoSourceForNegativeTest();
                configuration.HtmlValidationConfiguration.AssertHtmlValidationResultAsync = AssertHtmValidationResultAsync;
                configuration.AssertBrowserLog = logEntries =>
                    OrchardCoreUITestExecutorConfiguration.AssertBrowserLogIsEmpty(
                        logEntries.Where(logEntry =>
                            // The fetch call reports an error to the browser console if the
                            // QrCardController.GetBusinessCard() action results in NotFound.
                            !(
                                logEntry.Message.ContainsOrdinalIgnoreCase(nameof(QrCardController.GetBusinessCard))
                                && logEntry.Message.ContainsOrdinalIgnoreCase(
                                    "Failed to load resource: the server responded with a status of 404"))));
            });

    private static async Task AssertHtmValidationResultAsync(HtmlValidationResult validationResult)
    {
        var errors = (await validationResult.GetErrorsAsync())
            .Where(error =>
                !error.ContainsOrdinalIgnoreCase("The autoplay attribute is not allowed on <video>")
                && !error.ContainsOrdinalIgnoreCase("title text cannot be longer than 70 characters"));
        errors.ShouldBeEmpty();
    }
}
