using Lombiq.Tests.UI.Tests.UI.TestCases;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.Tests.UI.Tests.SqlMonitoringTests;

public class SqlQueryMonitoringTenantTests : Lombiq.Tests.UI.Samples.UITestBase
{
    public SqlQueryMonitoringTenantTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task SqlQueryMonitoringShouldWorkOnAnotherTenant() =>
        SqlQueryMonitoringTestCases.SqlQueryMonitoringShouldWorkOnAnotherTenantAsync(ExecuteTestAfterSetupAsync);
}
