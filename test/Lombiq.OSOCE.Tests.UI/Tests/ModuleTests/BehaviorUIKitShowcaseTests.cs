using Lombiq.UIKit.Tests.UI.Extensions;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

public class BehaviorUIKitShowcaseTests : UITestBase
{
    public BehaviorUIKitShowcaseTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task UIKitShowcasePageShouldBeCorrect()
        => ExecuteTestAfterSetupAsync(
            context => context.TestUIKitShowcaseBehaviorAsync(),
            configuration => configuration.HtmlValidationConfiguration
                .WithRelativeConfigPath("NoUniqueLandmark.htmlvalidate.json")
                .WithOC15222Filter()
                // This is necessary until https://github.com/OrchardCMS/OrchardCore/pull/18730 is merged.
                .WithFilters("OC-18730", error => error.RuleId is not "aria-label-misuse"));
}
