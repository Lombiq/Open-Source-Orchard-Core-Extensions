using Lombiq.Tests.UI.Extensions;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.Tests.UI.Tests.SqlMonitoringTests;

// SQL monitoring can be disabled when you only want the UI test helpers without the collection overhead.
public class SqlQueryMonitoringDisableCollectionTests : Lombiq.Tests.UI.Samples.UITestBase
{
    public SqlQueryMonitoringDisableCollectionTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task SqlQueryMonitoringShouldAllowDisablingCollection() =>
        ExecuteTestAfterSetupAsync(
            context => context.GoToHomePageAsync(onlyIfNotAlreadyThere: false),
            configuration =>
            {
                // Disable SQL query monitoring collection entirely.
                configuration.SqlQueryMonitoringConfiguration.EnableSqlQueryMonitoringCollection = false;
                return Task.CompletedTask;
            });
}

// NEXT STATION: Head over to Tests/SqlQueryMonitoringFailureTests.cs.
