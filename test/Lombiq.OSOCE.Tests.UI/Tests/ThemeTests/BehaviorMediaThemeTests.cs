using Lombiq.Hosting.MediaTheme.Tests.UI.Extensions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.ThemeTests;

public class BehaviorMediaThemeTests(ITestOutputHelper testOutputHelper) : UITestBase(testOutputHelper)
{
    [Fact]
    public Task MediaThemeShouldRenderTemplatesFromMediaLibrary() =>
        ExecuteTestAfterSetupAsync(context => context.TestMediaThemeTemplateRenderingBehaviorAsync());
}
