using Lombiq.Marketing.Tests.UI.Extensions;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

public class BehaviorMarketingTests : UITestBase
{
    public BehaviorMarketingTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task ShortUrlManagementShouldWork() => TestBuilder
        .AddConfiguration(configuration => configuration.SetShortUrlConfiguration())
        .RunAsync(context => context.TestShortUrlManagementAsync());

    [Fact]
    public Task PirschClientSideTrackingShouldBeInjected() => TestBuilder
        .AddConfiguration(configuration => configuration.SetPirschClientTrackerConfiguration())
        .RunAsync(context => context.TestPirschClientSideTrackingAutomaticInjectionAsync());
}
