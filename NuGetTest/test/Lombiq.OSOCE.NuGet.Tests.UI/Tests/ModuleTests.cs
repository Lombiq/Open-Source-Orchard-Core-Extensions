using Lombiq.DataTables.Tests.UI.Extensions;
using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Services;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.NuGet.Tests.UI.Tests
{
    public class ModuleTests : UITestBase
    {
        public ModuleTests(ITestOutputHelper testOutputHelper)
            : base(testOutputHelper)
        {
        }

        [Theory, Chrome]
        public Task DataTableShouldWork(Browser browser) =>
            ExecuteTestAfterSetupAsync(
                context => context.TestDataTableRecipeDataAsync(),
                browser);
    }
}
