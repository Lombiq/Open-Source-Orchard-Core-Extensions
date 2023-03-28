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

    [Theory, Chrome]
    public Task BuildVersionShouldBeBeDisplayedCorrectly(Browser browser) =>
        ExecuteTestAfterSetupAsync(
            context => context.TestBuildVersionDisplayAsync(),
            browser);
}
