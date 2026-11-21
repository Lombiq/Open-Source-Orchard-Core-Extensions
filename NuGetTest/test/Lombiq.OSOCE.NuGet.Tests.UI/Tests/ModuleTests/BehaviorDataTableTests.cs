using Lombiq.DataTables.Tests.UI.Extensions;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.NuGet.Tests.UI.Tests.ModuleTests;

public class BehaviorDataTableTests : UITestBase
{
    public BehaviorDataTableTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper) =>
        AddConfiguration(configuration => configuration
            .HtmlValidationConfiguration
            .WithRelativeConfigPath($"{nameof(BehaviorDataTableTests)}.htmlvalidate.json"));

    [Fact]
    public Task DataTableShouldWork() =>
        RunAsync(context => context.TestDataTableRecipeDataAsync(checkMainMenu: false));
}
