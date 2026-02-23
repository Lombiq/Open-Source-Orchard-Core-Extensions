using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.SqlQueryMonitoring.Extensions;
using Shouldly;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.Tests.UI.Tests.SqlMonitoringTests;

// Demonstrates asserting SQL monitoring for a request that happens separately from page navigation (for example, an
// AJAX/background call).
public class SqlQueryMonitoringRequestMatchingTests : Lombiq.Tests.UI.Samples.UITestBase
{
    public SqlQueryMonitoringRequestMatchingTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task SqlQueryMonitoringShouldAllowAssertingSeparateRequestsByPath() =>
        ExecuteTestAfterSetupAsync(
            async context =>
            {
                // Keep an explicit page load first, then trigger the monitored request separately.
                await context.GoToRelativeUrlAsync("/");

                const string requestPath = "/categories/travel?sqlMonitoringRequestCheck=1";

                using var client = context.CreateHttpClient();
                _ = await client.GetStringAsync(requestPath, context.Configuration.TestCancellationToken);

                await context.AssertSqlQueryMonitoringForRequestAsync(
                    requestPath,
                    HttpMethod.Get.Method,
                    summary =>
                    {
                        summary.Executions.ShouldNotBeEmpty("Separate requests should also be captured.");
                        summary.RequestPath.ShouldStartWith(
                            "/categories/travel",
                            Case.Insensitive,
                            "The request path should be captured in the summary and match the asserted path.");
                        return Task.CompletedTask;
                    });
            });
}
