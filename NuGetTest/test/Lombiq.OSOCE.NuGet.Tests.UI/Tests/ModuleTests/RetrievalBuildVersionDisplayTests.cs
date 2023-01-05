using Lombiq.Hosting.BuildVersionDisplay.Tests.UI.Extensions;
using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Services;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.NuGet.Tests.UI.Tests.ModuleTests;

public class RetrievalBuildVersionDisplayTests : UITestBase
{
    public RetrievalBuildVersionDisplayTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    // The build link won't be displayed, since the UI test project doesn't (shouldn't) reference the module
    // package directly and thus won't use its targets file either, breaking the BuildVersionDisplay_BuildUrl
    // property. This is not an error, the link is still displayed when the web app is run directly.
    [Theory, Chrome]
    public Task BuildVersionShouldBeBeDisplayedCorrectly(Browser browser) =>
        ExecuteTestAfterSetupAsync(
            context => context.TestBuildVersionDisplayAsync(checkBuildLink: false),
            browser);
}
