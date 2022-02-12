using Lombiq.Tests.UI.Attributes;
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
        public Task Test(Browser browser) =>
            ExecuteTestAfterSetupAsync(
                async context =>
                {
                },
                browser);
    }
}
