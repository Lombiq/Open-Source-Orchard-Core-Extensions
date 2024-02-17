using Lombiq.DataTables.Tests.UI.Extensions;
using Lombiq.OSOCE.NuGet.Tests.UI.Helpers;
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
        context => context.TestDataTableRecipeDataAsync(),
        // Can be removed once  https://github.com/OrchardCMS/OrchardCore/issues/15222 is done.
        changeConfiguration => changeConfiguration.AssertBrowserLog = AssertBrowserLogHelpers.AssertBrowserLogIsEmpty);
}
