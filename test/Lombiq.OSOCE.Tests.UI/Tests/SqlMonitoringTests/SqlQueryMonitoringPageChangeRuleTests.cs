using Lombiq.Tests.UI.Tests.UI.TestCases;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.Tests.UI.Tests.SqlMonitoringTests;

public class SqlQueryMonitoringPageChangeRuleTests : Lombiq.Tests.UI.Samples.UITestBase
{
    public SqlQueryMonitoringPageChangeRuleTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task SqlQueryMonitoringShouldRespectPageChangeRule() =>
        SqlQueryMonitoringTestCases.SqlQueryMonitoringShouldRespectPageChangeRuleAsync(ExecuteTestAfterSetupAsync);
}
