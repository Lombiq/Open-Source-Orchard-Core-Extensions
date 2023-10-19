using Lombiq.Hosting.Tenants.Management.Tests.UI.Extensions;
using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Services;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

public class BehaviorTenantManagementTests : UITestBase
{
    public BehaviorTenantManagementTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Theory, Chrome]
    public Task TenantShellSettingsEditorShouldSaveSettings(Browser browser) =>
        ExecuteTestAfterSetupAsync(
            async context => await context.TestShellSettingsEditorFeatureAsync(),
            browser);
}
