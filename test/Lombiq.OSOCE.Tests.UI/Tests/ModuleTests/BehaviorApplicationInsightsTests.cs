using Lombiq.Hosting.Azure.ApplicationInsights.Tests.UI.TestCases;
using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Services;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

public class BehaviorApplicationInsightsTests : UITestBase
{
    public BehaviorApplicationInsightsTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Theory, Chrome]
    public Task ApplicationInsightsTrackingInOfflineOperationShouldWork(Browser browser) =>
        ApplicationInsightsTestCases.ApplicationInsightsTrackingInOfflineOperationShouldWorkAsync(ExecuteTestAfterSetupAsync, browser);
}
