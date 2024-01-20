using Lombiq.Hosting.Azure.ApplicationInsights.Tests.UI.TestCases;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

public class BehaviorApplicationInsightsTests(ITestOutputHelper testOutputHelper) : UITestBase(testOutputHelper)
{
    [Fact]
    public Task ApplicationInsightsTrackingInOfflineOperationShouldWork() =>
        ApplicationInsightsTestCases.ApplicationInsightsTrackingInOfflineOperationShouldWorkAsync(ExecuteTestAfterSetupAsync);
}
