using Lombiq.Tests.UI.Tests.UI.TestCases;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.Tests.UI.Tests.SqlMonitoringTests;

public class SqlQueryMonitoringFailureTests : Lombiq.Tests.UI.Samples.UITestBase
{
    public SqlQueryMonitoringFailureTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task SqlQueryMonitoringFailureScenariosShouldWork() =>
        SqlQueryMonitoringTestCases.SqlQueryMonitoringFailureScenariosShouldWorkAsync(ExecuteTestAfterSetupAsync);
}
