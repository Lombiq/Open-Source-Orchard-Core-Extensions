using Lombiq.HelpfulLibraries.Samples.Tests.UI.Extensions;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

public class BehaviorHelpfulLibrariesSamplesTests : UITestBase
{
    public BehaviorHelpfulLibrariesSamplesTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task LinqToDbSamplesShouldWorkCorrectly() =>
        RunAsync(context => context.TestLinqToDbSamplesAsync());
}
