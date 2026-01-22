using Lombiq.Tests.UI.Extensions;
using Lombiq.UIKit.Widgets.Tests.UI.Extensions;
using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

public class BehaviorUIKitWidgetsTests : UITestBase
{
    public BehaviorUIKitWidgetsTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task UIKitCarouselWidgetShouldHaveSlickContainer() =>
        ExecuteTestAfterSetupAsync(
            context => context.TestCarouselWidgetBehaviorAsync(),
            configuration => configuration.HtmlValidationConfiguration
                .WithRelativeConfigPath("NoUniqueLandmark.htmlvalidate.json")
                .WithOC15222Filter());

    [Fact]
    public Task CarouselWidgetPartSettingsHasJsonEditorForOptionsAndOptionsAreUsed() =>
        ExecuteTestAfterSetupAsync(
            context => context.TestCarouselWidgetOptionsAsync(),
            configuration => configuration.HtmlValidationConfiguration
                .WithRelativeConfigPath("NoUniqueLandmark.htmlvalidate.json")
                .WithOC15222Filter());
}
