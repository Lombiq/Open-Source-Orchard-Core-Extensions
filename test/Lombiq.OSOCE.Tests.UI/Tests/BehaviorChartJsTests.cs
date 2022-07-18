using Lombiq.ChartJs.Tests.UI.Extensions;
using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests;

public class BehaviorChartJsTests : UITestBase
{
    public BehaviorChartJsTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Theory, Chrome]
    public Task RecipeDataShouldBeDisplayedCorrectly(Browser browser) =>
        ExecuteTestAfterSetupAsync(
            async context =>
            {
                await context.TestChartJsSampleBehaviorAsync();
                await context.GoToRelativeUrlAsync("UIKitShowcase");
            },
            browser,
            configuration => configuration.HtmlValidationConfiguration.AssertHtmlValidationResultAsync =
                async validationResult =>
                {
                    var errors = (await validationResult.GetErrorsAsync())
                        .Where(error => !error.ContainsOrdinalIgnoreCase("Prefer to use the native <button> element"));
                    errors.ShouldBeEmpty();
                });
}
