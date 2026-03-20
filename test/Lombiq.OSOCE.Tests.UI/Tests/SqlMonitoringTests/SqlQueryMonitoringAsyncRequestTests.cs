using Lombiq.Tests.UI.Tests.UI.TestCases;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.Tests.UI.Tests.SqlMonitoringTests;

public class SqlQueryMonitoringAsyncRequestTests : Lombiq.Tests.UI.Samples.UITestBase
{
    public SqlQueryMonitoringAsyncRequestTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task SqlQueryMonitoringAsyncRequestScenariosShouldWork() =>
        SqlQueryMonitoringTestCases.SqlQueryMonitoringAsyncRequestScenariosShouldWorkAsync(ExecuteTestAfterSetupAsync);
}
