using Lombiq.Hosting.MediaTheme.Tests.UI.Extensions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.ThemeTests;

public class BehaviorMediaThemeTests : UITestBase
{
    public BehaviorMediaThemeTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task MediaThemeShouldRenderTemplatesFromMediaLibrary() =>
        ExecuteTestAfterSetupAsync(context => context.TestMediaThemeTemplateRenderingBehaviorAsync());
}
