using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Helpers;
using Lombiq.Tests.UI.SqlQueryMonitoring;
using Lombiq.Tests.UI.SqlQueryMonitoring.Extensions;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.Tests.UI.Tests.SqlMonitoringTests;

// Verifies that SQL monitoring captures the main query execution paths beyond "normal" page queries.
public class SqlQueryMonitoringAdditionalQuerySourcesTests : Lombiq.Tests.UI.Samples.UITestBase
{
    public SqlQueryMonitoringAdditionalQuerySourcesTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task SqlQueryMonitoringShouldCaptureRawQuery() =>
        ExecuteSqlMonitoringScenarioAsync(
            requestPath: "/Lombiq.Tests.UI.Shortcuts/SqlQueryMonitoringScenario/RawQuery",
            entry =>
                entry.CommandText.Contains("SELECT", StringComparison.OrdinalIgnoreCase) &&
                entry.CommandText.Contains("ContentItemIndex", StringComparison.OrdinalIgnoreCase),
            "The raw SQL query should be captured.");

    [Fact]
    public Task SqlQueryMonitoringShouldCaptureRawExecuteNonQuery() =>
        ExecuteSqlMonitoringScenarioAsync(
            requestPath: "/Lombiq.Tests.UI.Shortcuts/SqlQueryMonitoringScenario/RawExecuteNonQuery",
            entry =>
                entry.CommandText.Contains("DELETE", StringComparison.OrdinalIgnoreCase) &&
                entry.CommandText.Contains("ContentItemIndex", StringComparison.OrdinalIgnoreCase),
            "The raw SQL non-query command should be captured.");

    [Fact]
    public Task SqlQueryMonitoringShouldCaptureCustomSessionQuery() =>
        ExecuteSqlMonitoringScenarioAsync(
            requestPath: "/Lombiq.Tests.UI.Shortcuts/SqlQueryMonitoringScenario/CustomSessionQuery",
            entry => entry.CommandText.Contains("ContentItemIndex", StringComparison.OrdinalIgnoreCase),
            "Queries executed through a manually created YesSql session should be captured.");

    [Fact]
    public Task SqlQueryMonitoringShouldCaptureDirectConnectionQuery() =>
        ExecuteSqlMonitoringScenarioAsync(
            requestPath: "/Lombiq.Tests.UI.Shortcuts/SqlQueryMonitoringScenario/DirectConnectionQuery",
            entry =>
                entry.CommandText.Contains("SELECT", StringComparison.OrdinalIgnoreCase) &&
                entry.CommandText.Contains("ContentItemIndex", StringComparison.OrdinalIgnoreCase),
            "Queries executed through IDbConnectionAccessor should be captured.");

    private Task ExecuteSqlMonitoringScenarioAsync(
        string requestPath,
        Predicate<SqlQueryExecutionEntry> executionPredicate,
        string assertionMessage) =>
        ExecuteTestAfterSetupAsync(
            async context =>
            {
                await context.GoToRelativeUrlAsync(requestPath);

                await context.AssertSqlQueryMonitoringAsync(
                    summary =>
                    {
                        summary.RequestPath.ShouldStartWith(
                            requestPath,
                            Case.Insensitive,
                            "The monitored summary should belong to the navigated page request.");

                        summary.Executions.ShouldContain(
                            execution => executionPredicate(execution),
                            assertionMessage);

                        return Task.CompletedTask;
                    });
            },
            ConfigurationHelper.DisableHtmlValidation);
}
