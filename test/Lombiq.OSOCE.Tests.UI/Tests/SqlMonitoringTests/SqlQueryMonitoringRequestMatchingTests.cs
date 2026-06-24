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

#pragma warning disable xUnit1004
    [Fact(Skip = "Method not found: 'System.Threading.Tasks.Task YesSql.IStore.InitializeCollectionAsync(System.String)'.")]
#pragma warning restore xUnit1004
    public Task SqlQueryMonitoringRequestMatchingScenariosShouldWork() =>
        SqlQueryMonitoringTestCases.SqlQueryMonitoringRequestMatchingScenariosShouldWorkAsync(ExecuteTestAfterSetupAsync);
}
