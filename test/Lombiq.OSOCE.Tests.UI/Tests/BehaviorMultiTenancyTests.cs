using Lombiq.Hosting.MultiTenancy.Tenants.Tests.UI.Extensions;
using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Services;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests;

public class BehaviorMultiTenancyTests : UITestBase
{
    public BehaviorMultiTenancyTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Theory, Chrome]
    public Task ForbiddenFeaturesShouldNotBeActivatableOnTenants(Browser browser) =>
        ExecuteTestAfterSetupAsync(context => context.TestForbiddenFeaturesAsync(), browser, configuration =>
            configuration.HtmlValidationConfiguration.RunHtmlValidationAssertionOnAllPageChanges = false);

    [Theory, Chrome]
    public Task AlwaysEnabledFeaturesShouldNotBeDeactivatableOnTenants(Browser browser) =>
        ExecuteTestAfterSetupAsync(context => context.TestAlwaysEnabledFeaturesAsync(), browser, configuration =>
            configuration.HtmlValidationConfiguration.RunHtmlValidationAssertionOnAllPageChanges = false);
}
