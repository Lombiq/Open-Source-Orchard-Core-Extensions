using Atata.HtmlValidation;
using Lombiq.Tests.UI.Extensions;
using Lombiq.VueJs.Samples.Controllers;
using Lombiq.VueJs.Tests.UI.Extensions;
using Shouldly;
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

                // The fetch call reports an error to the browser console if the QrCardController.GetBusinessCard()
                // action results in NotFound.
                configuration.ResponseLogFilter = e =>
                    e.IsNonSuccessResponseAndNotExpectedNotFoundResponse(nameof(QrCardController.GetBusinessCard));
            });

    private static Task AssertHtmValidationResultAsync(HtmlValidationResult validationResult)
    {
        var errors = validationResult.GetParsedErrors()
            .Where(error =>
                error.RuleId is not "no-autoplay" and
                    not "long-title");
        errors.ShouldBeEmpty(HtmlValidationResultExtensions.GetParsedErrorMessageString(errors));
        return Task.CompletedTask;
    }
}
