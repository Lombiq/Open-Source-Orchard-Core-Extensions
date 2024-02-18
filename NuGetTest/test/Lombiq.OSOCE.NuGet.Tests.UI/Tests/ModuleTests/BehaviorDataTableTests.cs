using Lombiq.DataTables.Tests.UI.Extensions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.NuGet.Tests.UI.Tests.ModuleTests;

public class BehaviorDataTableTests : UITestBase
{
    public BehaviorDataTableTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task DataTableShouldWork() => ExecuteTestAfterSetupAsync(
        context => context.TestDataTableRecipeDataAsync());
}
