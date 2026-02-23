using Lombiq.Tests.UI.Tests.UI.TestCases;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.Tests.UI.Tests.SqlMonitoringTests;

public class SqlQueryMonitoringDisableCollectionTests : Lombiq.Tests.UI.Samples.UITestBase
{
    public SqlQueryMonitoringDisableCollectionTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task SqlQueryMonitoringShouldAllowDisablingCollection() =>
        SqlQueryMonitoringTestCases.SqlQueryMonitoringShouldAllowDisablingCollectionAsync(ExecuteTestAfterSetupAsync);
}
