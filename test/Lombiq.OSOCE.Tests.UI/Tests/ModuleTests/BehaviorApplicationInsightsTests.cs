using Lombiq.Hosting.Azure.ApplicationInsights.Tests.UI.TestCases;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

public class BehaviorApplicationInsightsTests : UITestBase
{
    public BehaviorApplicationInsightsTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task ApplicationInsightsTrackingInOfflineOperationShouldWork() =>
        ApplicationInsightsTestCases.ApplicationInsightsTrackingInOfflineOperationShouldWorkAsync(ExecuteTestAfterSetupAsync);
}
