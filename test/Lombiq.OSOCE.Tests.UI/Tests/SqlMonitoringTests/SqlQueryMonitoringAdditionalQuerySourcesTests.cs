using Lombiq.Tests.UI.Tests.UI.TestCases;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.Tests.UI.Tests.SqlMonitoringTests;

public class SqlQueryMonitoringAdditionalQuerySourcesTests : Lombiq.Tests.UI.Samples.UITestBase
{
    public SqlQueryMonitoringAdditionalQuerySourcesTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task SqlQueryMonitoringShouldCaptureRawQuery() =>
        SqlQueryMonitoringTestCases.SqlQueryMonitoringShouldCaptureRawQueryAsync(ExecuteTestAfterSetupAsync);

    [Fact]
    public Task SqlQueryMonitoringShouldCaptureRawExecuteNonQuery() =>
        SqlQueryMonitoringTestCases.SqlQueryMonitoringShouldCaptureRawExecuteNonQueryAsync(ExecuteTestAfterSetupAsync);

    [Fact]
    public Task SqlQueryMonitoringShouldCaptureCustomSessionQuery() =>
        SqlQueryMonitoringTestCases.SqlQueryMonitoringShouldCaptureCustomSessionQueryAsync(ExecuteTestAfterSetupAsync);

    [Fact]
    public Task SqlQueryMonitoringShouldCaptureDirectConnectionQuery() =>
        SqlQueryMonitoringTestCases.SqlQueryMonitoringShouldCaptureDirectConnectionQueryAsync(ExecuteTestAfterSetupAsync);
}
