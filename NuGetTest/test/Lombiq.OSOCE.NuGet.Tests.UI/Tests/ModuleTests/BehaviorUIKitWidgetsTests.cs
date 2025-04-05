using Lombiq.UIKit.Widgets.Tests.UI.Extensions;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.NuGet.Tests.UI.Tests.ModuleTests;

public class BehaviorUIKitWidgetsTests : UITestBase
{
    public BehaviorUIKitWidgetsTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task CarouselWidgetDisplaySlidesCorrectly() =>
        ExecuteTestAfterSetupAsync(async context =>
            await context.TestCarouselWidgetBehaviorAsync()
        );

    [Fact]
    public Task JsonOptionsForCarouselWidgetWork() =>
        ExecuteTestAfterSetupAsync(async context => await context.TestCarouselWidgetOptionsAsync());
}
