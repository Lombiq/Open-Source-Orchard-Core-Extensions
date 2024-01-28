using Lombiq.Privacy.Tests.UI.Extensions;
using Lombiq.Tests.UI.Extensions;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

public class BehaviorPrivacyTests(ITestOutputHelper testOutputHelper) : UITestBase(testOutputHelper)
{
    [Fact]
    public Task ConsentBannerShouldWorkAnonymous() =>
        ExecuteTestAfterSetupAsync(context => context.TestConsentBannerAsync());

    [Fact]
    public Task ConsentBannerShouldWorkAdmin() =>
        ExecuteTestAfterSetupAsync(
            async context =>
            {
                await context.SignInDirectlyAsync();
                await context.TestConsentBannerAsync();
            });

    // This test is for https://github.com/Lombiq/Orchard-Privacy/issues/15
    [Fact]
    public async Task ConsentBannerShouldWorkWithRazorAndLiquidBasedThemes()
    {
        // First should work with Liquid-based theme
        await ExecuteTestAfterSetupAsync(
            context => context.TestConsentBannerWithThemeAsync("TheBlogTheme"));
        // Then should work with Razor-based theme
        await ExecuteTestAfterSetupAsync(
            context => context.TestConsentBannerWithThemeAsync("TheTheme"),
            configuration => configuration.HtmlValidationConfiguration.AssertHtmlValidationResultAsync =
                async validationResult =>
                {
                    var errors = (await validationResult.GetErrorsAsync())
                        .Where(error => !error.ContainsOrdinalIgnoreCase("Prefer to use the native <button> element"));
                    errors.ShouldBeEmpty();
                });
    }

    [Fact]
    public Task RegistrationConsentCheckboxShouldWork() =>
        ExecuteTestAfterSetupAsync(context => context.TestRegistrationConsentCheckboxAsync());

    [Fact]
    public Task FormConsentCheckboxShouldWork() =>
        ExecuteTestAfterSetupAsync(context => context.TestPrivacySampleBehaviorAsync());
}
