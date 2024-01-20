using Lombiq.Hosting.BuildVersionDisplay.Tests.UI.Extensions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

public class RetrievalBuildVersionDisplayTests(ITestOutputHelper testOutputHelper) : UITestBase(testOutputHelper)
{
    [Fact]
    public Task BuildVersionShouldBeBeDisplayedCorrectly() =>
        ExecuteTestAfterSetupAsync(context => context.TestBuildVersionDisplayAsync());
}
