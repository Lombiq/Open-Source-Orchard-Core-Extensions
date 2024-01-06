using Lombiq.BaseTheme.Tests.UI.Extensions;
using Lombiq.Tests.UI.Extensions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.NuGet.Tests.UI.Tests.ThemeTests;

public class BehaviorThemeTests : UITestBase
{
    public BehaviorThemeTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task BaseThemeShouldWork() =>
        ExecuteTestAfterSetupAsync(
            async context =>
            {
                await context.SignInDirectlyAsync();
                await context.ExecuteRecipeDirectlyAsync("Lombiq.OSOCE.NuGet.BaseTheme");

                await context.GoToHomePageAsync(onlyIfNotAlreadyThere: false);
                await context.TestBaseThemeFeaturesAsync();
            });
}
