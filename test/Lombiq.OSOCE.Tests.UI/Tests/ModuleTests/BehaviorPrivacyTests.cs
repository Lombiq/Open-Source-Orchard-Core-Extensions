using Lombiq.Privacy.Tests.UI.Extensions;
using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

public class BehaviorPrivacyTests : UITestBase
{
    public BehaviorPrivacyTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Theory, Chrome]
    public Task ConsentBannerShouldWorkAnonymous(Browser browser) =>
        ExecuteTestAfterSetupAsync(context => context.TestConsentBannerAsync(), browser);

    [Theory, Chrome]
    public Task ConsentBannerShouldWorkAdmin(Browser browser) =>
        ExecuteTestAfterSetupAsync(
            async context =>
            {
                await context.SignInDirectlyAsync();
                await context.TestConsentBannerAsync();
            },
            browser);

    // This test is for https://github.com/Lombiq/Orchard-Privacy/issues/15
    [Theory, Chrome]
    public async Task ConsentBannerShouldWorkWithRazorAndLiquidBasedThemes(Browser browser)
    {
        // First should work with Liquid-based theme
        await ExecuteTestAfterSetupAsync(
            context => context.TestConsentBannerWithThemeAsync("TheBlogTheme"),
            browser);
        // Then should work with Razor-based theme
        await ExecuteTestAfterSetupAsync(
            context => context.TestConsentBannerWithThemeAsync("TheTheme"),
            browser);
    }

    [Theory, Chrome]
    public Task RegistrationConsentCheckboxShouldWork(Browser browser) =>
        ExecuteTestAfterSetupAsync(context => context.TestRegistrationConsentCheckboxAsync(), browser);

    [Theory, Chrome]
    public Task FormConsentCheckboxShouldWork(Browser browser) =>
        ExecuteTestAfterSetupAsync(context => context.TestPrivacySampleBehaviorAsync(), browser);
}
