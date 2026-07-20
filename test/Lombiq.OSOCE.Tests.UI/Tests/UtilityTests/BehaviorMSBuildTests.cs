using Lombiq.MSBuild.Targets.Tests.UI.Extensions;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.Tests.UI.Tests.UtilityTests;

public class BehaviorMSBuildTests : UITestBase
{
    public BehaviorMSBuildTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task SdkUsingThemeShouldBeRecognizedByOrchardCore() =>
        ExecuteTestAfterSetupAsync(
            context => context.TestSdkUsingThemeAsync(),
            configuration => configuration.HtmlValidationConfiguration.RunHtmlValidationAssertionOnAllPageChanges = false);
}
