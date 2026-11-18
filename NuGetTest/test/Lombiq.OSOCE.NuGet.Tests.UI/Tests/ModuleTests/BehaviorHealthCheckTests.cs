using Lombiq.Hosting.Tenants.HealthChecks.Tests.UI.Extensions;
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
    public Task UnhealthyTenantsShouldBeListed() => TestBuilder
        .AddConfiguration(configuration =>
        {
            configuration.ConfigureTenantHealthCheckTests();
            configuration.AssertAppLogsAsync = app =>
                app.LogsShouldNotContainAsync(
                    logEntry => logEntry.Level >= LogLevel.Error && logEntry.IsNotHealthCheckError(),
                    configuration.TestCancellationToken);
        })
        .RunAsync(context => context.TestHealthChecksAsync("Lombiq.OSOCE.NuGet.BasicOrchardFeaturesTests"));
}
