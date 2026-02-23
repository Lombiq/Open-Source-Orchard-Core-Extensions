using Lombiq.Tests.UI.Tests.UI.TestCases;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.Tests.UI.Tests.SqlMonitoringTests;

public class SqlQueryMonitoringBasicsTests : Lombiq.Tests.UI.Samples.UITestBase
{
    public SqlQueryMonitoringBasicsTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task SqlQueryMonitoringShouldCatchDuplicatesAndLargeResults() =>
        SqlQueryMonitoringTestCases.SqlQueryMonitoringShouldCatchDuplicatesAndLargeResultsAsync(ExecuteTestAfterSetupAsync);
}
