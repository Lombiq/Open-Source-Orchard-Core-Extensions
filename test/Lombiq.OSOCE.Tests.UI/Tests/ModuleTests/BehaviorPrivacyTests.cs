using Lombiq.Privacy.Tests.UI.Extensions;
using Lombiq.Tests.UI.Extensions;
using Shouldly;
using System;
using System.Linq;
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

    // This test is for https://github.com/Lombiq/Orchard-Privacy/issues/15.
    [Fact]
    public async Task ConsentBannerShouldWorkWithRazorAndLiquidBasedThemes()
    {
        // First should work with a Liquid-based theme.
        await ExecuteTestAfterSetupAsync(
            context => context.TestConsentBannerWithThemeAsync("TheBlogTheme"));

        // Then should work with a Razor-based theme.
        await ExecuteTestAfterSetupAsync(
            context => context.TestConsentBannerWithThemeAsync("TheTheme"),
            configuration => configuration.HtmlValidationConfiguration.AssertHtmlValidationResultAsync =
                validationResult =>
                {
                    // Error filtering due to https://github.com/OrchardCMS/OrchardCore/issues/15222,
                    // can be removed once it is resolved.
                    var errors = validationResult.GetParsedErrors()
                        .Where(error => error.RuleId is not "prefer-native-element");
                    errors.ShouldBeEmpty(string.Join('\n', errors.Select(error => error.Message)));
                    return Task.CompletedTask;
                });
    }

    [Fact]
    public Task RegistrationConsentCheckboxShouldWork() =>
        ExecuteTestAfterSetupAsync(context => context.TestRegistrationConsentCheckboxAsync());

    [Fact]
    public Task FormConsentCheckboxShouldWork() =>
        ExecuteTestAfterSetupAsync(context => context.TestPrivacySampleBehaviorAsync());
}
