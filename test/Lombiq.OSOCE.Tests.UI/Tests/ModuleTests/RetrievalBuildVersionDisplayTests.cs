using Lombiq.Hosting.BuildVersionDisplay.Tests.UI.Extensions;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

public class RetrievalBuildVersionDisplayTests : UITestBase
{
    public RetrievalBuildVersionDisplayTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task BuildVersionShouldBeBeDisplayedCorrectly() =>
        ExecuteTestAfterSetupAsync(context => context.TestBuildVersionDisplayAsync());
}
