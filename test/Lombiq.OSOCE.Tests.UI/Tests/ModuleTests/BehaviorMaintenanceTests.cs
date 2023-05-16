using Lombiq.Hosting.Tenants.IdleTenantManagement.Tests.UI.Extensions;
using Lombiq.Hosting.Tenants.Maintenance.Tests.UI.Extensions;
using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Services;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

public class BehaviorMaintenanceTests : UITestBase
{
    public BehaviorMaintenanceTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Theory, Chrome]
    public Task NecessaryMaintenanceTasksShouldBeExecutedSuccessfullyAndOthersSkipped(Browser browser) =>
        ExecuteTestAfterSetupAsync(
            async context =>
            {
                await context.TestMaintenanceExecutionAsync();

                context.Configuration.AssertAppLogsAsync = async webApplicationInstance =>
                {
                    await AssertAppLogsDefaultOSOCEAsync(webApplicationInstance);
                    await MaintenanceExtensions.AssertAppLogsWithMaintenanceExecutionStartAsync(webApplicationInstance);
                    await MaintenanceExtensions.AssertAppLogsWithSuccessfulUpdateSiteUrlExecutionAsync(webApplicationInstance);
                    await MaintenanceExtensions.AssertAppLogsWithSkippedUpdateShellRequestUrlExecutionAsync(webApplicationInstance);
                };
            },
            browser,
            configuration => configuration.SetUpdateSiteUrlMaintenanceConfiguration());
}
