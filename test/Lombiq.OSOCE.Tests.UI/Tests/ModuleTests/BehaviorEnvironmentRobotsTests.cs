using Lombiq.Hosting.Tenants.EnvironmentRobots.Tests.UI.Extensions;
using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Services;
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

    [Theory, Chrome]
    public Task RobotsMetaTagShouldBeMissing(Browser browser) =>
        ExecuteTestAfterSetupAsync(
            async context => await context.TestRobotMetaTagIsMissingAsync(shouldBeMissing: true),
            browser,
            configuration => configuration.SetEnvironmentRobotsOptionsConfiguration(isProduction: true));

    [Theory, Chrome]
    public Task RobotsMetaTagShouldBeMissingWithoutConfiguration(Browser browser) =>
        ExecuteTestAfterSetupAsync(
            async context => await context.TestRobotMetaTagIsMissingAsync(shouldBeMissing: false),
            browser);

    [Theory, Chrome]
    public Task RobotsMetaTagShouldBePresent(Browser browser) =>
        ExecuteTestAfterSetupAsync(
            async context => await context.TestRobotMetaTagIsMissingAsync(shouldBeMissing: false),
            browser,
            configuration => configuration.SetEnvironmentRobotsOptionsConfiguration(isProduction: false));
}
