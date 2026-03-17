using Lombiq.Tests.UI.Tests.UI.TestCases;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.Tests.UI.Tests.SqlMonitoringTests;

public class SqlQueryMonitoringLinqToDbTests : Lombiq.Tests.UI.Samples.UITestBase
{
    public SqlQueryMonitoringLinqToDbTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task LinqToDbSamplesShouldBeCapturedBySqlMonitoring() =>
        SqlQueryMonitoringTestCases.LinqToDbSamplesShouldBeCapturedBySqlMonitoringAsync(ExecuteTestAfterSetupAsync);
}
