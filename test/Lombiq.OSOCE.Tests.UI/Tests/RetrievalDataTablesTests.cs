using Lombiq.DataTables.Tests.UI.Extensions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

public class BehaviorDataTablesTests(ITestOutputHelper testOutputHelper) : UITestBase(testOutputHelper)
{
    [Fact]
    public Task RecipeDataShouldBeDisplayedCorrectly() =>
        ExecuteTestAfterSetupAsync(context => context.TestDataTableRecipeDataAsync());
}
