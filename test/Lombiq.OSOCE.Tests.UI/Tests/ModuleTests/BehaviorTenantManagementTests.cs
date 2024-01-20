using Lombiq.Hosting.Tenants.Management.Tests.UI.Extensions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

public class BehaviorTenantManagementTests(ITestOutputHelper testOutputHelper) : UITestBase(testOutputHelper)
{
    [Fact]
    public Task TenantShellSettingsEditorShouldSaveSettings() =>
        ExecuteTestAfterSetupAsync(context => context.TestShellSettingsEditorFeatureAsync());
}
