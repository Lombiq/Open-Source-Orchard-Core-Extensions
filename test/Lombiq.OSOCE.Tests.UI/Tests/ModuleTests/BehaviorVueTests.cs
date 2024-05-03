using Atata.HtmlValidation;
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

    [Fact]
    public Task RecipeDataShouldBeDisplayedCorrectly() =>
        ExecuteTestAfterSetupAsync(
            async context =>
            {
                await context.SignInDirectlyAsync();
                await context.TestVueSampleBehaviorAsync();
            });

    [Fact]
    public Task QrCardScanShouldWorkAsync() =>
        ExecuteTestAfterSetupAsync(
            context => context.TestQrCardFoundAsync(),
            configuration =>
            {
                configuration.BrowserConfiguration.ConfigureFakeVideoSourceForPositiveTest();
                configuration.HtmlValidationConfiguration.AssertHtmlValidationResultAsync = AssertHtmValidationResultAsync;
            });

    [Fact]
    public Task QrCardScanShouldReportNotFoundAsync() =>
        ExecuteTestAfterSetupAsync(
            context => context.TestQrCardNotFoundAsync(),
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

    private static Task AssertHtmValidationResultAsync(HtmlValidationResult validationResult)
    {
        var errors = validationResult.GetParsedErrors()
            .Where(error =>
                error.RuleId is not "no-autoplay" and
                    not "long-title");
        errors.ShouldBeEmpty();
        return Task.CompletedTask;
    }
}
