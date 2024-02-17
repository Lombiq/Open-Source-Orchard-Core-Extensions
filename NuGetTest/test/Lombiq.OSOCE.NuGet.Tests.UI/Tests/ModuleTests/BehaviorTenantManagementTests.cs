using Lombiq.Hosting.Tenants.Management.Tests.UI.Extensions;
using Lombiq.OSOCE.NuGet.Tests.UI.Helpers;
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
        ExecuteTestAfterSetupAsync(
            context => context.TestShellSettingsEditorFeatureAsync(),
            // Can be removed once  https://github.com/OrchardCMS/OrchardCore/issues/15222 is done.
            changeConfiguration =>
            {
                changeConfiguration.AssertBrowserLog = AssertHtmlAndBrowserErrorsHelper.AssertBrowserLogIsEmpty;
                changeConfiguration.HtmlValidationConfiguration.AssertHtmlValidationResultAsync =
                    AssertHtmlAndBrowserErrorsHelper.AssertHtmlErrorsAreEmpty;
            });
}
