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
    public Task MediaThemeShouldWorkWhenDeployed() =>
        ExecuteTestAfterSetupAsync(context => context.TestMediaThemeDeployedBehaviorAsync());

    [Fact]
    public Task MediaThemeShouldWorkLocally() =>
        ExecuteTestAfterSetupAsync(context => context.TestMediaThemeLocalBehaviorAsync());
}
