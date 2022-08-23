using Lombiq.Hosting.Azure.ApplicationInsights.Tests.UI.Extensions;
using Lombiq.Privacy.Tests.UI.Extensions;
using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
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

                await context.EnablePrivacyConsentBannerFeatureAndAcceptPrivacyConsentAsync();

                context.TestApplicationInsightsTrackingInOfflineOperation();
            },
            browser,
            configuration => configuration.EnableApplicationInsightsOfflineOperation());
}
