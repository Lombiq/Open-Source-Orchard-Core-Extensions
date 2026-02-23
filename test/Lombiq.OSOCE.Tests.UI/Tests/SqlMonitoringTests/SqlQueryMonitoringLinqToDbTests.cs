using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Helpers;
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

                await context.GoToRelativeUrlAsync(requestPath);

                await context.AssertSqlQueryMonitoringAsync(summary =>
                {
                    summary.RequestPath.ShouldStartWith(
                        requestPath,
                        Case.Insensitive,
                        "The monitored summary should belong to the navigated LINQ to DB endpoint request.");

                    summary.Executions.ShouldNotBeEmpty("LINQ to DB calls should be captured by SQL query monitoring.");
                    summary.Executions.ShouldContain(entry =>
                        entry.CommandText.Contains("FROM", StringComparison.OrdinalIgnoreCase));

                    return Task.CompletedTask;
                });
            },
            ConfigurationHelper.DisableHtmlValidation);
}
