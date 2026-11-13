using Lombiq.Walkthroughs.Tests.UI.Extensions;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

public class BehaviorWalkthroughsTests : UITestBase
{
    public BehaviorWalkthroughsTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task WalkthroughsShouldWorkCorrectly() =>
        AddSyncConfiguration(configuration => configuration
            .HtmlValidationConfiguration
            .WithRelativeConfigPath("WalkthroughsShouldWorkCorrectly.htmlvalidate.json"))
            .RunAsync(context => context.RunSetupAndTestWalkthroughsBehaviorAsync());
}
