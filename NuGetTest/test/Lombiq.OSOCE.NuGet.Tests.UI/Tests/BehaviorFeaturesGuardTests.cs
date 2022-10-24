using Lombiq.Hosting.Tenants.FeaturesGuard.Tests.UI.Extensions;
using Lombiq.OSOCE.NuGet.Tests.UI.Helpers;
using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Services;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.NuGet.Tests.UI.Tests;

public class BehaviorFeaturesGuardTests : UITestBase
{
    public BehaviorFeaturesGuardTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    // HTML validation is disabled as OC's login and dashboard pages contain several errors. See:
    // https://github.com/OrchardCMS/OrchardCore/issues/12271
    // https://github.com/OrchardCMS/OrchardCore/issues/12271
#pragma warning disable xUnit1004 // Test methods should not be skipped
    [Theory(Skip = "Temporarily skipped while FeaturesGuard is disabled."), Chrome]
#pragma warning restore xUnit1004 // Test methods should not be skipped
    public Task ForbiddenFeaturesShouldNotBeActivatableOnTenants(Browser browser) =>
        ExecuteTestAfterSetupAsync(context => context.TestForbiddenFeaturesAsync(SetupHelpers.RecipeId), browser, configuration =>
            configuration.HtmlValidationConfiguration.RunHtmlValidationAssertionOnAllPageChanges = false);

    // HTML validation is disabled as OC's login and dashboard pages contain several errors. See:
    // https://github.com/OrchardCMS/OrchardCore/issues/12271
    // https://github.com/OrchardCMS/OrchardCore/issues/12271
#pragma warning disable xUnit1004 // Test methods should not be skipped
    [Theory(Skip = "Temporarily skipped while FeaturesGuard is disabled."), Chrome]
#pragma warning restore xUnit1004 // Test methods should not be skipped
    public Task AlwaysEnabledFeaturesShouldNotBeDeactivatableOnTenants(Browser browser) =>
        ExecuteTestAfterSetupAsync(context => context.TestAlwaysEnabledFeaturesAsync(SetupHelpers.RecipeId), browser, configuration =>
            configuration.HtmlValidationConfiguration.RunHtmlValidationAssertionOnAllPageChanges = false);
}
