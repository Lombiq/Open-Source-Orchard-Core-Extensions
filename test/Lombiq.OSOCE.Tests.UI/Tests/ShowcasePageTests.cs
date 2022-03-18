using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Services;
using Lombiq.UIKit.Tests.UI.Extensions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests
{
    public class ShowcasePageTests : UITestBase
    {
        public ShowcasePageTests(ITestOutputHelper testOutputHelper)
            : base(testOutputHelper)
        {
        }

        [Theory, Chrome]
        public Task TestShowcasePage(Browser browser) =>
            ExecuteTestAfterSetupAsync(
                context => context.TestShowCasePageAsync(),
                browser);
    }
}
