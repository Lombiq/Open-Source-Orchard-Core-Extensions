using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using Lombiq.VueJs.Tests.UI.Extensions;
using OpenQA.Selenium;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests;

public class BehaviorVueTests : UITestBase
{
    public BehaviorVueTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Theory, Chrome]
    public Task RecipeDataShouldBeDisplayedCorrectly(Browser browser) =>
        ExecuteTestAfterSetupAsync(
            async context =>
            {
                await context.SignInDirectlyAsync();
                await context.TestVueSampleBehaviorAsync();
            },
            browser);

    [Theory, Chrome]
    public Task ConsentBannerShouldWork(Browser browser) =>
        ExecuteTestAfterSetupAsync(
            async context =>
            {
                var privacyConsentAcceptButton = By.Id("privacy-consent-accept-button");

                await context.EnableFeatureDirectlyAsync("Lombiq.Privacy.ConsentBanner");
                await context.GoToHomePageAsync();
                await context.ClickReliablyOnAsync(privacyConsentAcceptButton);
                context.Missing(privacyConsentAcceptButton);
            },
            browser);
}
