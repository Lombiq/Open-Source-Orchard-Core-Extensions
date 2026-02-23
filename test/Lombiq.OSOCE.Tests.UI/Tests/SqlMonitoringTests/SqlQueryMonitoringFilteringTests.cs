using Lombiq.Tests.UI.Tests.UI.TestCases;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.Tests.UI.Tests.SqlMonitoringTests;

public class SqlQueryMonitoringFilteringTests : Lombiq.Tests.UI.Samples.UITestBase
{
    public SqlQueryMonitoringFilteringTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task SqlQueryMonitoringShouldAllowIgnoringKnownQueries() =>
        SqlQueryMonitoringTestCases.SqlQueryMonitoringShouldAllowIgnoringKnownQueriesAsync(ExecuteTestAfterSetupAsync);
}
