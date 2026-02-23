using Lombiq.Tests.UI.SqlQueryMonitoring.Extensions;
using Lombiq.Tests.UI.SqlQueryMonitoring.Services;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.Tests.UI.Tests.SqlMonitoringTests;

// This test shows how to filter out known noisy queries while still using the default threshold assertions.
public class SqlQueryMonitoringFilteringTests : Lombiq.Tests.UI.Samples.UITestBase
{
    public SqlQueryMonitoringFilteringTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task SqlQueryMonitoringShouldAllowIgnoringKnownQueries() =>
        ExecuteTestAfterSetupAsync(
            context => context.AssertSqlQueryMonitoringAsync(),
            configuration =>
            {
                // Keep thresholds low to make filtering behavior visible, but still high enough for stable tests.
                configuration.SqlQueryMonitoringConfiguration.DuplicateCommandThreshold = 5;
                configuration.SqlQueryMonitoringConfiguration.DuplicateCommandWithParametersThreshold = 3;
                configuration.SqlQueryMonitoringConfiguration.ResultSetRowCountThreshold = 5;

                // You can ignore specific commands by their command text using regex patterns. Here we ignore
                // common queries that are known to be noisy in Orchard Core, e.g. queries loading content items of
                // specific types or common indexes.
                configuration.SqlQueryMonitoringConfiguration.ExecutionFilter =
                    SqlQueryMonitoringConfiguration.BuildIgnoreCommandTextPatternFilter(
                        @"FROM\s+\[Document\].*\[Type\]\s*=\s*@Type",
                        @"FROM\s+\[Document\].*ContentDefinitionRecord",
                        @"FROM\s+\[Document\].*RolesDocument",
                        @"FROM\s+\[Document\].*PlacementsDocument",
                        @"FROM\s+\[Document\].*LayersDocument",
                        @"FROM\s+\[Document\].*TemplatesDocument",
                        @"FROM\s+\[ContentItemIndex\]",
                        @"FROM\s+\[AutoroutePartIndex\]");
            });
}

// END OF TRAINING SECTION: SQL query monitoring.
// NEXT STATION: Head over to Tests/SqlServerTests.cs.
