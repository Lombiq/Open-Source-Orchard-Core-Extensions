using Lombiq.Hosting.Tenants.EnvironmentRobots.Tests.UI.Extensions;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

public class BehaviorEnvironmentRobotsTests : UITestBase
{
    public BehaviorEnvironmentRobotsTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task RobotsMetaTagShouldBeMissing() =>
        ExecuteTestAfterSetupAsync(
            context => context.TestRobotMetaTagIsMissingAsync(shouldBeMissing: true),
            configuration => configuration.SetEnvironmentRobotsOptionsConfiguration(isProduction: true));

    [Fact]
    public Task RobotsMetaTagShouldBeMissingWithoutConfiguration() =>
        ExecuteTestAfterSetupAsync(
            context => context.TestRobotMetaTagIsMissingAsync(shouldBeMissing: false),
            configuration => configuration.TimeoutConfiguration.TestRunTimeout = TimeSpan.FromMinutes(10));

    [Fact]
    public Task RobotsMetaTagShouldBePresent() =>
        ExecuteTestAfterSetupAsync(
            context => context.TestRobotMetaTagIsMissingAsync(shouldBeMissing: false),
            configuration => configuration.SetEnvironmentRobotsOptionsConfiguration(isProduction: false));
}
