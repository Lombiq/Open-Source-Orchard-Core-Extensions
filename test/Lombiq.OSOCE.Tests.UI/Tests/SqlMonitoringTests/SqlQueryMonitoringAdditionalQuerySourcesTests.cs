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
    public Task SqlQueryMonitoringAdditionalQuerySourcesShouldWork() =>
        SqlQueryMonitoringTestCases.SqlQueryMonitoringAdditionalQuerySourcesShouldWorkAsync(ExecuteTestAfterSetupAsync);
}
