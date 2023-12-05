using Lombiq.Hosting.Tenants.Maintenance.Tests.UI.Extensions;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Pages;
using OpenQA.Selenium;
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

    [Fact]
    public Task MaintenanceTaskShouldBeExecutedSuccessfully() =>
        ExecuteTestAfterSetupAsync(
            context => context.TestSiteUrlMaintenanceExecutionAsync(),
            configuration => configuration.SetUpdateSiteUrlMaintenanceConfiguration());

    [Fact]
    public Task AddSiteOwnerPermissionToRoleMaintenanceTaskShouldBeExecutedSuccessfully() =>
        ExecuteTestAfterSetupAsync(
            context => context.TestSiteOwnerPermissionToRoleMaintenanceExecutionAsync(),
            configuration => configuration.SetAddSiteOwnerPermissionToRoleMaintenanceConfiguration());

    // This test employs ExecuteTestAsync with a distinct setup delegate instead of ExecuteTestAfterSetupAsync.
    // This choice is prompted by the modifications it applies to the appsettings.json file, occasionally leading
    // to failures in subsequent tests within the NuGet solution in the CI environment
    [Fact]
    public Task ChangeUserSensitiveContentMaintenanceTaskShouldBeExecutedSuccessfully() =>
        ExecuteTestAsync(
            context => context.ChangeUserSensitiveContentMaintenanceExecutionAsync(),
            async context =>
            {
                var homepageUri = await context.GoToSetupPageAndSetupOrchardCoreAsync(
                    new OrchardCoreSetupParameters(context)
                    {
                        SiteName = "Lombiq's OSOCE - UI Testing",
                        RecipeId = "Lombiq.OSOCE.NuGet.Tests",
                        TablePrefix = "OSOCE",
                        SiteTimeZoneValue = "Europe/Budapest",
                    });

                context.Exists(By.Id("navbar"));

                return homepageUri;
            },
            configuration => configuration.ChangeUserSensitiveContentMaintenanceConfiguration());
}
