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
            context => context.TestPirschClientSideTrackingAutomaticInjectionAsync(),
            changeConfiguration: configuration => configuration.SetPirschClientTrackerConfiguration());
}
