using Lombiq.Tests.UI.Tests.UI.TestCases;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.Tests.UI.Tests.SqlMonitoringTests;

public class SqlQueryMonitoringThresholdsTests : Lombiq.Tests.UI.Samples.UITestBase
{
    public SqlQueryMonitoringThresholdsTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task SqlQueryMonitoringThresholdScenariosShouldWork() =>
        SqlQueryMonitoringTestCases.SqlQueryMonitoringThresholdScenariosShouldWorkAsync(ExecuteTestAfterSetupAsync);
}
