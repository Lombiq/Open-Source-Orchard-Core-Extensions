using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.SqlQueryMonitoring.Extensions;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.Tests.UI.Tests.SqlMonitoringTests;

// Demonstrates asserting SQL monitoring for a page request with query-string based request matching.
public class SqlQueryMonitoringRequestMatchingTests : Lombiq.Tests.UI.Samples.UITestBase
{
    public SqlQueryMonitoringRequestMatchingTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task SqlQueryMonitoringShouldCaptureRequestPathAndQueryForNavigatedPage() =>
        ExecuteTestAfterSetupAsync(
            async context =>
            {
                const string requestPath = "/categories/travel?sqlMonitoringRequestCheck=1";

                await context.GoToRelativeUrlAsync(requestPath);

                await context.AssertSqlQueryMonitoringAsync(summary =>
                {
                    summary.Executions.ShouldNotBeEmpty("Page requests should be captured.");
                    summary.RequestPath.ShouldStartWith(
                        requestPath,
                        Case.Insensitive,
                        "The request path and query should be captured in the summary and match the navigated path.");
                    return Task.CompletedTask;
                });
            });
}
