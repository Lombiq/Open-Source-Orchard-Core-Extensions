using Lombiq.Hosting.Tenants.FeaturesGuard.Tests.UI.Extensions;
using Lombiq.Tests.UI.Helpers;
using Lombiq.Tests.UI.Samples.Helpers;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

[SuppressMessage("Usage", "xUnit1004:Test methods should not be skipped", Justification = "As explained in the Skip.")]
public class BehaviorFeaturesGuardTests : UITestBase
{
    public BehaviorFeaturesGuardTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    // HTML validation is disabled as OC's login and dashboard pages contain several errors. See:
    // https://github.com/OrchardCMS/OrchardCore/issues/12271.
    [Fact(Skip = "Feature profiles are bugged, see https://github.com/OrchardCMS/OrchardCore/issues/15451")]
    public Task ForbiddenFeaturesShouldNotBeActivatableOnTenants() =>
        ExecuteTestAfterSetupAsync(
            context => context.TestForbiddenFeaturesAsync(SetupHelpers.RecipeId),
            ConfigurationHelper.DisableHtmlValidation);

    // HTML validation is disabled as OC's login and dashboard pages contain several errors. See:
    // https://github.com/OrchardCMS/OrchardCore/issues/12271.
    [Fact(Skip = "Feature profiles are bugged, see https://github.com/OrchardCMS/OrchardCore/issues/15451")]
    public Task ConditionallyEnabledFeaturesShouldWorkCorrectlyOnTenants() =>
        ExecuteTestAfterSetupAsync(
            context => context.TestConditionallyEnabledFeaturesAsync(SetupHelpers.RecipeId),
            ConfigurationHelper.DisableHtmlValidation);
}
