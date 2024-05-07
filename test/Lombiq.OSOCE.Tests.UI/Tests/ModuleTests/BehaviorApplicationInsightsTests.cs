using Lombiq.Hosting.Azure.ApplicationInsights.Tests.UI.TestCases;
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

#pragma warning disable xUnit1004 // Test methods should not be skipped
    [Fact(Skip = "Debugging test hangs.")]
#pragma warning restore xUnit1004 // Test methods should not be skipped
    public Task ApplicationInsightsTrackingInOfflineOperationShouldWork() =>
        ApplicationInsightsTestCases.ApplicationInsightsTrackingInOfflineOperationShouldWorkAsync(ExecuteTestAfterSetupAsync);
}
