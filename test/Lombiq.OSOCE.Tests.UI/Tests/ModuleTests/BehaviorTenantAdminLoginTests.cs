using Lombiq.Hosting.Tenants.EmailQuotaManagement.Tests.UI.Extensions;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

public class BehaviorTenantAdminLoginTests : UITestBase
{
    public BehaviorTenantAdminLoginTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task TenantEditorShouldHaveLoginAsAdminUserButton() =>
        ExecuteTestAfterSetupAsync(
            context => context.TestTenantAdminLoginBehaviorAsync(),
            changeConfiguration: configuration => configuration.HtmlValidationConfiguration
                .WithOC15222Filter());
}
