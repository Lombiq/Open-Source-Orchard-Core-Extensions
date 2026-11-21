using Lombiq.Hosting.Tenants.EnvironmentRobots.Tests.UI.Extensions;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.NuGet.Tests.UI.Tests.ModuleTests;

public class BehaviorEnvironmentRobotsTests : UITestBase
{
    public BehaviorEnvironmentRobotsTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task RobotsMetaTagShouldBeMissing() => TestBuilder
        .AddConfiguration(configuration => configuration.SetEnvironmentRobotsOptionsConfiguration(isProduction: true))
        .RunAsync(context => context.TestRobotMetaTagIsMissingAsync(shouldBeMissing: true));

    [Fact]
    public Task RobotsMetaTagShouldBeMissingWithoutConfiguration() =>
        RunAsync(context => context.TestRobotMetaTagIsMissingAsync(shouldBeMissing: false));

    [Fact]
    public Task RobotsMetaTagShouldBePresent() => TestBuilder
        .AddConfiguration(configuration => configuration.SetEnvironmentRobotsOptionsConfiguration(isProduction: false))
        .RunAsync(context => context.TestRobotMetaTagIsMissingAsync(shouldBeMissing: false));
}
