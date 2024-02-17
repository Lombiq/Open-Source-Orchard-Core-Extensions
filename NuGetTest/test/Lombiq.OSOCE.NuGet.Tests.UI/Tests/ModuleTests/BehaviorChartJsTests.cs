using Lombiq.ChartJs.Tests.UI.Extensions;
using Lombiq.OSOCE.NuGet.Tests.UI.Helpers;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.NuGet.Tests.UI.Tests.ModuleTests;

public class BehaviorChartJsTests : UITestBase
{
    public BehaviorChartJsTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task RecipeDataShouldBeDisplayedCorrectly() =>
        ExecuteTestAfterSetupAsync(
            context => context.TestChartJsSampleBehaviorAsync(),
            // Can be removed once  https://github.com/OrchardCMS/OrchardCore/issues/15222 is done.
            changeConfiguration =>
            {
                changeConfiguration.AssertBrowserLog = AssertHtmlAndBrowserErrorsHelper.AssertBrowserLogIsEmpty;
                changeConfiguration.HtmlValidationConfiguration.AssertHtmlValidationResultAsync =
                    AssertHtmlAndBrowserErrorsHelper.AssertHtmlErrorsAreEmpty;
            });
}
