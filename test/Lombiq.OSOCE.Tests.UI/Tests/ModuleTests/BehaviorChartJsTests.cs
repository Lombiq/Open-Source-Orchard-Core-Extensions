using Atata.HtmlValidation;
using Lombiq.ChartJs.Samples.Controllers;
using Lombiq.ChartJs.Tests.UI.Extensions;
using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using Lombiq.Tests.UI.Services.Counters.Extensions;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

public class BehaviorChartJsTests : UITestBase
{
    public BehaviorChartJsTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Theory, Chrome]
    public Task RecipeDataShouldBeDisplayedCorrectly(Browser browser) =>
        ExecuteTestAfterSetupAsync(
            context => context.TestChartJsSampleBehaviorAsync(),
            browser,
            configuration =>
            {
                configuration.HtmlValidationConfiguration.AssertHtmlValidationResultAsync = AssertHtmValidationResultAsync;
                configuration.CounterConfiguration.Running.PhaseThreshold.DbReaderReadThreshold = 70;
                configuration.CounterConfiguration.Running.ConfigureForRelativeUrl<SampleController>(
                    configuration => configuration.NavigationThreshold.DbReaderReadThreshold = 51,
                    controller => controller.Balance(),
                    exactMatch: true);

                configuration.CounterConfiguration.Running.ConfigureForRelativeUrl<SampleController>(
                    configuration => configuration.NavigationThreshold.DbReaderReadThreshold = 70,
                    controller => controller.History(null, null),
                    exactMatch: true);

                return Task.CompletedTask;
            });

    private static async Task AssertHtmValidationResultAsync(HtmlValidationResult validationResult)
    {
        var errors = (await validationResult.GetErrorsAsync())
            .Where(error => !error.ContainsOrdinalIgnoreCase("title text cannot be longer than 70 characters"));
        errors.ShouldBeEmpty();
    }
}
