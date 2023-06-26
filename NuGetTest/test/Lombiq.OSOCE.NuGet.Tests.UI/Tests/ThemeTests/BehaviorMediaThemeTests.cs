using Lombiq.Hosting.MediaTheme.Tests.UI.Extensions;
using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Services;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.NuGet.Tests.UI.Tests.ThemeTests;

public class BehaviorMediaThemeTests : UITestBase
{
    public BehaviorMediaThemeTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Theory, Chrome]
    public Task MediaThemeShouldRenderTemplatesFromMediaLibrary(Browser browser) =>
        ExecuteTestAfterSetupAsync(
            context => context.TestMediaThemeTemplateRenderingBehaviorAsync(),
            browser);
}
