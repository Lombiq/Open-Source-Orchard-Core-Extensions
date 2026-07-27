using Lombiq.Hosting.Tenants.FeaturesGuard.Tests.UI.Extensions;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Helpers;
using Lombiq.Tests.UI.Samples.Helpers;
using Lombiq.Tests.UI.Services;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

public class BehaviorFeaturesGuardTests : UITestBase
{
    public BehaviorFeaturesGuardTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    // HTML validation is disabled as OC's login and dashboard pages contain several errors. See:
    // https://github.com/OrchardCMS/OrchardCore/issues/12271.
    [Fact]
    public Task ForbiddenFeaturesShouldNotBeActivatableOnTenants() =>
        ExecuteTestAfterSetupAsync(
            async context =>
            {
                await BeforeTestAsync(context);
                await context.TestForbiddenFeaturesAsync(SetupHelpers.RecipeId);
            },
            ConfigurationHelper.DisableHtmlValidation);

    // HTML validation is disabled as OC's login and dashboard pages contain several errors. See:
    // https://github.com/OrchardCMS/OrchardCore/issues/12271.
    [Fact]
    public Task ConditionallyEnabledFeaturesShouldWorkCorrectlyOnTenants() =>
        ExecuteTestAfterSetupAsync(
            async context =>
            {
                await BeforeTestAsync(context);
                await context.TestConditionallyEnabledFeaturesAsync(SetupHelpers.RecipeId);
            },
            ConfigurationHelper.DisableHtmlValidation);

    private static Task BeforeTestAsync(UITestContext context) =>
        context.ExecuteRecipeDirectlyAsync("Lombiq.OSOCE.Tests.FeaturesGuard");
}
