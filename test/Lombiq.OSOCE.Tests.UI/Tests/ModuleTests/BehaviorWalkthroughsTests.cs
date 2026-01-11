using Lombiq.Walkthroughs.Tests.UI.Extensions;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

[SuppressMessage("Usage", "xUnit1004:Test methods should not be skipped", Justification = "Temporarily disabled.")]
public class BehaviorWalkthroughsTests : UITestBase
{
    public BehaviorWalkthroughsTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact(Skip = "Temporarily disabled.")]
    public Task WalkthroughsShouldWorkCorrectly() =>
        ExecuteTestAsync(
            context => context.RunSetupAndTestWalkthroughsBehaviorAsync(),
            changeConfiguration: configuration => configuration
                .HtmlValidationConfiguration
                .WithRelativeConfigPath("NoUniqueLandmark.htmlvalidate.json"));
}
