using Lombiq.Walkthroughs.Tests.UI.Extensions;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

public class BehaviorWalkthroughsTests : UITestBase
{
    public BehaviorWalkthroughsTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact(Skip = "Temporarily disabled.")]
    [SuppressMessage("Usage", "xUnit1004:Test methods should not be skipped", Justification = "See above.")]
    public Task WalkthroughsShouldWorkCorrectly() =>
        ExecuteTestAsync(context => context.RunSetupAndTestWalkthroughsBehaviorAsync());
}
