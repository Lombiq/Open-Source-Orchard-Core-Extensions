using Atata.HtmlValidation;
using Lombiq.DataTables.Controllers.Api;
using Lombiq.DataTables.Samples.Controllers;
using Lombiq.DataTables.Tests.UI.Extensions;
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

public class BehaviorDataTablesTests : UITestBase
{
    public BehaviorDataTablesTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Theory, Chrome]
    public Task RecipeDataShouldBeDisplayedCorrectly(Browser browser) =>
        ExecuteTestAfterSetupAsync(
            context => context.TestDataTableRecipeDataAsync(),
            browser,
            configuration =>
            {
                configuration.HtmlValidationConfiguration.AssertHtmlValidationResultAsync = AssertHtmValidationResultAsync;
                configuration.CounterConfiguration.Running.ConfigureForRelativeUrl<SampleController>(
                    configuration =>
                    {
                        configuration.NavigationThreshold.DbReaderReadThreshold = 51;
                        configuration.PageLoadThreshold.DbReaderReadThreshold = 51;
                        configuration.SessionThreshold.DbReaderReadThreshold = 51;
                    },
                    controller => controller.DataTableTagHelper(),
                    exactMatch: true);
                configuration.CounterConfiguration.Running.ConfigureForRelativeUrl<RowsController>(
                    configuration =>
                    {
                        configuration.PageLoadThreshold.DbReaderReadThreshold = 57;
                        configuration.SessionThreshold.DbReaderReadThreshold = 57;
                    },
                    controller => controller.Get(null),
                    exactMatch: false);
                configuration.CounterConfiguration.Running.PhaseThreshold.DbReaderReadThreshold = 57;
                configuration.CounterConfiguration.Running.PhaseThreshold.DbReaderReadThreshold = 171;
                configuration.CounterConfiguration.Running.PhaseThreshold.DbCommandTextExecutionThreshold = 171;
                return Task.CompletedTask;
            });

    private static async Task AssertHtmValidationResultAsync(HtmlValidationResult validationResult)
    {
        var errors = (await validationResult.GetErrorsAsync())
            .Where(error => !error.ContainsOrdinalIgnoreCase("title text cannot be longer than 70 characters"));
        errors.ShouldBeEmpty();
    }
}
