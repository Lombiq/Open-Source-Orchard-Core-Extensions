using Lombiq.ChartJs.Tests.UI.Extensions;
using Lombiq.DataTables.Tests.UI.Extensions;
using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Services;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.NuGet.Tests.UI.Tests;

public class BehaviorModuleTests : UITestBase
{
    public BehaviorModuleTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Theory, Chrome]
    public Task DataTableShouldWork(Browser browser) =>
        ExecuteTestAfterSetupAsync(
            context => context.TestDataTableRecipeDataAsync(),
            browser);

    [Theory, Chrome]
    public Task ChartJsShouldWork(Browser browser) =>
        ExecuteTestAfterSetupAsync(
            context => context.TestChartJsSampleBehaviorAsync(),
            browser);
}
