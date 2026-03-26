using Lombiq.Tests.UI.Tests.UI.TestCases;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.Tests.UI.Tests.SqlMonitoringTests;

public class SqlQueryMonitoringRequestMatchingTests : Lombiq.Tests.UI.Samples.UITestBase
{
    public SqlQueryMonitoringRequestMatchingTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task SqlQueryMonitoringRequestMatchingScenariosShouldWork() =>
        SqlQueryMonitoringTestCases.SqlQueryMonitoringRequestMatchingScenariosShouldWorkAsync(ExecuteTestAfterSetupAsync);
}
