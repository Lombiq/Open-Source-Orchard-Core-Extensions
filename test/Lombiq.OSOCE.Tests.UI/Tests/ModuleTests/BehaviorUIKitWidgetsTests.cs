using Lombiq.UIKit.Widgets.Tests.UI.Extensions;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

public class BehaviorUIKitWidgetsTests : UITestBase
{
    public BehaviorUIKitWidgetsTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper) =>
        AddSyncConfiguration(configuration => configuration.HtmlValidationConfiguration
            .WithRelativeConfigPath("NoUniqueLandmark.htmlvalidate.json")
            .WithOC15222Filter());

    [Fact]
    public Task UIKitCarouselWidgetShouldHaveSlickContainer() =>
        RunAsync(context => context.TestCarouselWidgetBehaviorAsync());

    [Fact]
    public Task CarouselWidgetPartSettingsHasJsonEditorForOptionsAndOptionsAreUsed() =>
        RunAsync(context => context.TestCarouselWidgetOptionsAsync());
}
