using Lombiq.Hosting.Azure.ApplicationInsights.Tests.UI.Extensions;
using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using OpenQA.Selenium;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests;

public class BehaviorApplicationInsightsTests : UITestBase
{
    public BehaviorApplicationInsightsTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Theory, Chrome]
    public Task ApplicationInsightsTrackingInOfflineOperationShouldWork(Browser browser) =>
        ExecuteTestAfterSetupAsync(
            async context =>
            {
                await context.EnableFeatureDirectlyAsync("Lombiq.Hosting.Azure.ApplicationInsights");
                await context.EnableFeatureDirectlyAsync("Lombiq.Privacy.ConsentBanner");

                await context.GoToHomePageAsync();

                // For tracking to be enabled, even in offline mode, the user needs to give consent.
                await context.ClickReliablyOnAsync(By.Id("privacy-consent-accept-button"));
                context.Refresh();

                context.TestApplicationInsightsTrackingInOfflineOperation();
            },
            browser,
            configuration => configuration.EnableApplicationInsightsOfflineOperation());
}
