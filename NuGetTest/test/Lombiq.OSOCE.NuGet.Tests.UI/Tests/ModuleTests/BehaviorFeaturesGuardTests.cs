using Lombiq.Hosting.Tenants.FeaturesGuard.Tests.UI.Extensions;
using Lombiq.OSOCE.NuGet.Tests.UI.Helpers;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.NuGet.Tests.UI.Tests.ModuleTests;

public class BehaviorFeaturesGuardTests : UITestBase
{
    public BehaviorFeaturesGuardTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    // HTML validation is disabled as OC's login and dashboard pages contain several errors. See:
    // https://github.com/OrchardCMS/OrchardCore/issues/12271
    // https://github.com/OrchardCMS/OrchardCore/issues/12271
    [Fact]
    public Task ForbiddenFeaturesShouldNotBeActivatableOnTenants() =>
        ExecuteTestAfterSetupAsync(
            context => context.TestForbiddenFeaturesAsync(SetupHelpers.RecipeId),
            configuration => configuration.HtmlValidationConfiguration.RunHtmlValidationAssertionOnAllPageChanges = false);

    // HTML validation is disabled as OC's login and dashboard pages contain several errors. See:
    // https://github.com/OrchardCMS/OrchardCore/issues/12271
    // https://github.com/OrchardCMS/OrchardCore/issues/12271
    [Fact]
    public Task ConditionallyEnabledFeaturesShouldWorkCorrectlyOnTenants() =>
        ExecuteTestAfterSetupAsync(
            context => context.TestConditionallyEnabledFeaturesAsync(SetupHelpers.RecipeId),
            configuration => configuration.HtmlValidationConfiguration.RunHtmlValidationAssertionOnAllPageChanges = false);
}
