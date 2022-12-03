using Lombiq.DataTables.Tests.UI.Extensions;
using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Services;
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
                configuration.CounterConfiguration.Running.DbReaderReadThreshold = 57;
                configuration.CounterConfiguration.Running.DbReaderReadPerNavigationThreshold = 57;
                return Task.CompletedTask;
            });
}
