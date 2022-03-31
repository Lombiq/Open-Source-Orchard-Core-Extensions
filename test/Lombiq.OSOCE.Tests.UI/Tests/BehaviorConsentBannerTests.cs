using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using OpenQA.Selenium;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests;

public class BehaviorConsentBannerTests : UITestBase
{
    public BehaviorConsentBannerTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Theory, Chrome]
    public Task ConsentBannerShouldWork(Browser browser) =>
        ExecuteTestAfterSetupAsync(
            async context =>
            {
                var privacyConsentAcceptButton = By.Id("privacy-consent-accept-button");

                // Enable consent banner and interact with it.
                await context.EnableFeatureDirectlyAsync("Lombiq.Privacy.ConsentBanner");
                await context.GoToHomePageAsync();
                await context.ClickReliablyOnAsync(privacyConsentAcceptButton);
                context.Missing(privacyConsentAcceptButton);

                // Verify persistence.
                context.Refresh();
                context.Missing(privacyConsentAcceptButton);
            },
            browser);
}
