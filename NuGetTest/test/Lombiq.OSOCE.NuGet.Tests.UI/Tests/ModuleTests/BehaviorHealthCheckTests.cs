using Lombiq.Hosting.Tenants.HealthChecks.Tests.UI.Extensions;
using Lombiq.OSOCE.NuGet.Tests.UI;
using Lombiq.Tests.UI.Extensions;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.NuGet.Tests.UI.Tests.ModuleTests;

public class BehaviorHealthCheckTests : UITestBase
{
    public BehaviorHealthCheckTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task UnhealthyTenantsShouldBeListed() =>
        ExecuteTestAfterSetupAsync(
            context => context.TestHealthChecksAsync("Lombiq.OSOCE.NuGet.BasicOrchardFeaturesTests"),
            configuration =>
                configuration.AssertAppLogsAsync = app =>
                    app.LogsShouldNotContainAsync(
                        logEntry =>
                            logEntry.Level >= LogLevel.Error &&
                            !logEntry.Message.Contains("Health check TestHealthCheck with status Unhealthy") &&
                            !logEntry.Message.Contains("Tenant is Unhealthy:"),
                        configuration.TestCancellationToken));
}
