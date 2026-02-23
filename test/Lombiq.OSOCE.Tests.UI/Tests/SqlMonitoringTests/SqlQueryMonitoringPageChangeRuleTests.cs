using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.SqlQueryMonitoring;
using Shouldly;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.Tests.UI.Tests.SqlMonitoringTests;

// You can also customize which page changes should be monitored by configuring a predicate.
public class SqlQueryMonitoringPageChangeRuleTests : Lombiq.Tests.UI.Samples.UITestBase
{
    public SqlQueryMonitoringPageChangeRuleTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    // Here we'll only monitor SQL queries on page changes where the URL contains "/categories".
    [Fact]
    public Task SqlQueryMonitoringShouldRespectPageChangeRule()
    {
        // We'll collect the summaries ourselves to assert on them later.
        var summaries = new List<SqlQueryMonitoringSummary>();

        return ExecuteTestAfterSetupAsync(
            async context =>
            {
                await context.GoToRelativeUrlAsync("/categories/travel");
                await context.GoToRelativeUrlAsync("/about");

                // Now we can assert that only /categories page change was monitored.
                summaries.Count.ShouldBe(1);
                summaries[0].RequestPath.ShouldContain("/categories");
                summaries[0].Executions.ShouldNotBeEmpty("SQL query monitoring should capture at least one command.");
            },
            configuration =>
            {
                // This test validates page-change based monitoring rules, so automatic page-change assertions must be
                // enabled explicitly (it's off by default).
                configuration.SqlQueryMonitoringConfiguration.RunSqlQueryMonitoringAssertionOnAllPageChanges = true;

                // Only monitor page changes where the URL contains "/categories".
                configuration.SqlQueryMonitoringConfiguration.SqlQueryMonitoringAndAssertionOnPageChangeRule =
                    context => context.GetCurrentUri().AbsolutePath.Contains("/categories");

                // We'll run assertions ourselves in the test so the captured summaries don't get consumed by the
                // automatic on-page-change assertions.
                configuration.SqlQueryMonitoringConfiguration.AssertSqlQueryMonitoringSummaryAsync = summary =>
                {
                    summaries.Add(summary);
                    return Task.CompletedTask;
                };

                return Task.CompletedTask;
            });
    }
}

// NEXT STATION: Head over to Tests/SqlQueryMonitoringFilteringTests.cs.
