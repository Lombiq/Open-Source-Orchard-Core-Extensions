using Lombiq.Privacy.Tests.UI.Extensions;
using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests;
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
    public Task ConsentBannerShouldWorkOsoe108(Browser browser) =>
        // First should work with liquid based theme
        Task.WhenAll(
            ExecuteTestAfterSetupAsync(
                async context =>
                {
                    await context.SelectThemeAsync("TheBlogTheme");
                    await context.SignInDirectlyAsync();
                    await context.TestConsentBannerAsync();
                },
                browser),
            // Then should work with razor based theme
            ExecuteTestAfterSetupAsync(
                async context =>
                {
                    await context.SelectThemeAsync("TheTheme");
                    await context.SignInDirectlyAsync();
                    await context.TestConsentBannerAsync();
                },
                browser)
            );

    [Theory, Chrome]
    public Task RegistrationConsentCheckboxShouldWork(Browser browser) =>
        ExecuteTestAfterSetupAsync(context => context.TestRegistrationConsentCheckboxAsync(), browser);

    [Theory, Chrome]
    public Task FormConsentCheckboxShouldWork(Browser browser) =>
        ExecuteTestAfterSetupAsync(context => context.TestPrivacySampleBehaviorAsync(), browser);
}
