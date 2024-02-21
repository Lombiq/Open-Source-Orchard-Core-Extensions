using Lombiq.Hosting.Tenants.Management.Tests.UI.Extensions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.NuGet.Tests.UI.Tests.ModuleTests;

public class BehaviorTenantManagementTests : UITestBase
{
    public BehaviorTenantManagementTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task TenantShellSettingsEditorShouldSaveSettings() =>
        ExecuteTestAfterSetupAsync(context => context.TestShellSettingsEditorFeatureAsync());
}
