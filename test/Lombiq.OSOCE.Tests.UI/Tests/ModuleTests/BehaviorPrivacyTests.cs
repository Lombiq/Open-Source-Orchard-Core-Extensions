using Lombiq.Privacy.Tests.UI.Extensions;
using Lombiq.Tests.UI.Extensions;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

public class BehaviorPrivacyTests : UITestBase
{
    public BehaviorPrivacyTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task ConsentBannerShouldWorkAnonymous() =>
        RunAsync(context => context.TestConsentBannerAsync());

    [Fact]
    public Task ConsentBannerShouldWorkAdmin() =>
        RunAsync(
            async context =>
            {
                await context.SignInDirectlyAsync();
                await context.TestConsentBannerAsync();
            });

    // This test is for https://github.com/Lombiq/Orchard-Privacy/issues/15.
    [Fact]
    public Task ConsentBannerShouldWorkWithRazorBasedThemes() =>
        AddTest(context => context.TestConsentBannerWithThemeAsync("TheTheme"))
            .AddSyncConfiguration(configuration => configuration.HtmlValidationConfiguration.WithOC15222Filter())
            .RunAsync();

    // This test is for https://github.com/Lombiq/Orchard-Privacy/issues/15.
    [Fact]
    public Task ConsentBannerShouldWorkWithLiquidBasedThemes() =>
        RunAsync(context => context.TestConsentBannerWithThemeAsync("TheBlogTheme"));

    [Fact]
    public Task RegistrationConsentCheckboxShouldWork() =>
        RunAsync(context => context.TestRegistrationConsentCheckboxAsync());

    [Fact]
    public Task FormConsentCheckboxShouldWork() =>
        RunAsync(context => context.TestPrivacySampleBehaviorAsync());
}
