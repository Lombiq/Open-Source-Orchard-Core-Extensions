using Lombiq.Hosting.Tenants.HealthChecks.Tests.UI.Extensions;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

public class BehaviorHealthCheckTests : UITestBase
{
    public BehaviorHealthCheckTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper) =>
        AddConfiguration(configuration =>
        {
            configuration.ConfigureTenantHealthCheckTests();
            configuration.AppLogFilters[nameof(ApplicationLogEntryExtensions.IsNotHealthCheckError)] =
                ApplicationLogEntryExtensions.IsNotHealthCheckError;
        });

    [Fact]
    public Task UnhealthyTenantsShouldBeListed() =>
        RunAsync(context => context.TestHealthChecksAsync("Lombiq.OSOCE.BasicOrchardFeaturesTests"));
}
