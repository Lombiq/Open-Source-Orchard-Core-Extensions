using Lombiq.ChartJs.Tests.UI.Extensions;
using Lombiq.OSOCE.NuGet.Tests.UI.Helpers;
using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Services;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.NuGet.Tests.UI.Tests;

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
            changeConfigurationAsync: config =>
            {
                config.AssertAppLogsAsync = AssertAppLogsHelpers.AssertOsoceAppLogsAreEmptyAsync;
                return Task.CompletedTask;
            });
}
