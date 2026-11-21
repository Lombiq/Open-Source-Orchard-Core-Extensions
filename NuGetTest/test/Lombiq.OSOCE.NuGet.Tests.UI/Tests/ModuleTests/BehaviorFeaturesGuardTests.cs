using Lombiq.Hosting.Tenants.FeaturesGuard.Tests.UI.Extensions;
using Lombiq.OSOCE.NuGet.Tests.UI.Helpers;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.NuGet.Tests.UI.Tests.ModuleTests;

public class BehaviorFeaturesGuardTests : UITestBase
{
    // HTML validation is disabled as OC's login and dashboard pages contain several errors. See:
    // https://github.com/OrchardCMS/OrchardCore/issues/12271.
    public BehaviorFeaturesGuardTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper) =>
        AddConfiguration(configuration => configuration
            .HtmlValidationConfiguration
            .RunHtmlValidationAssertionOnAllPageChanges = false);

    [Fact]
    public Task ForbiddenFeaturesShouldNotBeActivatableOnTenants() =>
        RunAsync(context => context.TestForbiddenFeaturesAsync(SetupHelpers.RecipeId));

    [Fact]
    public Task ConditionallyEnabledFeaturesShouldWorkCorrectlyOnTenants() =>
        RunAsync(context => context.TestConditionallyEnabledFeaturesAsync(SetupHelpers.RecipeId));
}
