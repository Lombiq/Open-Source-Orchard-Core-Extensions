using Lombiq.Marketing.Tests.UI.Extensions;
using Lombiq.Tests.UI.Extensions;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.NuGet.Tests.UI.Tests.ModuleTests;

public class BehaviorMarketingTests : UITestBase
{
    public BehaviorMarketingTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task ShortUrlManagementShouldWork() =>
        ExecuteTestAfterSetupAsync(
            context => context.TestShortUrlManagementAsync(),
            changeConfiguration: configuration => configuration.SetShortUrlConfiguration());

    [Fact]
    public Task PirschClientSideTrackingShouldBeInjected() =>
        ExecuteTestAfterSetupAsync(
            async context =>
            {
                // In the NuGet test we need to switch to the Base Theme to test the automatic zone rendering.
                await context.SignInDirectlyAsync();
                await context.ExecuteRecipeDirectlyAsync("Lombiq.OSOCE.NuGet.BaseTheme");
                await context.GoToHomePageAsync();

                await context.TestPirschClientSideTrackingAutomaticInjectionAsync();
            },
            changeConfiguration: configuration => configuration.SetPirschClientTrackerConfiguration());
}
