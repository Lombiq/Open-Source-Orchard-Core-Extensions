using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.SqlQueryMonitoring.Extensions;
using Shouldly;
using System;
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
        ExecuteTestAfterSetupAsync(
            async context =>
            {
                await context.EnableFeatureDirectlyAsync("Lombiq.HelpfulLibraries.Samples");

                const string requestPath = "/Lombiq.HelpfulLibraries.Samples/LinqToDbSamples/SimpleQuery";

                // Keep browser navigation on normal HTML pages so HTML validation doesn't fail on non-HTML endpoints.
                await context.GoToHomePageAsync(onlyIfNotAlreadyThere: false);

                using var client = context.CreateHttpClient();
                _ = await client.GetStringAsync(requestPath, context.Configuration.TestCancellationToken);

                await context.AssertSqlQueryMonitoringForRequestAsync(
                    requestPath,
                    requestMethod: "GET",
                    assertSummaryAsync: summary =>
                {
                    summary.Executions.ShouldNotBeEmpty("LINQ to DB calls should be captured by SQL query monitoring.");
                    summary.Executions.ShouldContain(entry =>
                        entry.CommandText.Contains("FROM", StringComparison.OrdinalIgnoreCase));

                    return Task.CompletedTask;
                });
            });
}
