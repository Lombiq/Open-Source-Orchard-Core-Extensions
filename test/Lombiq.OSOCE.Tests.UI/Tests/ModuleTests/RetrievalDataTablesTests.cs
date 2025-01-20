using Lombiq.DataTables.Tests.UI;
using Lombiq.DataTables.Tests.UI.Extensions;
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

    [Theory]
    [InlineData(TestDataTableRecipeDataSections.MainMenu)]
    [InlineData(TestDataTableRecipeDataSections.TagHelper)]
    [InlineData(TestDataTableRecipeDataSections.ProviderWithShape)]
    [InlineData(TestDataTableRecipeDataSections.JsonBasedProvider)]
    [InlineData(TestDataTableRecipeDataSections.IndexBasedProvider)]
    public Task RecipeDataShouldBeDisplayedCorrectly(TestDataTableRecipeDataSections sections) =>
        ExecuteTestAfterSetupAsync(context => context.TestDataTableRecipeDataAsync(sections));
}
