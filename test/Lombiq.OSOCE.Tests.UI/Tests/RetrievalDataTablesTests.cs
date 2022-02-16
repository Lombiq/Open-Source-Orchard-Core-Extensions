using Lombiq.DataTables.Tests.UI.Extensions;
using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests
{
    public class BehaviorDataTablesTests : UITestBase
    {
        public BehaviorDataTablesTests(ITestOutputHelper testOutputHelper)
            : base(testOutputHelper)
        {
        }

        [Theory, Chrome]
        public Task RecipeDataShouldBeDisplayedCorrectly(Browser browser) =>
            ExecuteTestAfterSetupAsync(
                async context =>
                {
                    await context.SignInDirectlyAsync();
                    await context.ExecuteDataTablesSampleRecipeDirectlyAsync();

                    await context.TestDataTableTagHelperAsync();
                    await context.TestDataTableProviderWithShapeAsync();
                    await context.TestDataTableIndexBasedProviderAsync();
                },
                browser);
    }
}
