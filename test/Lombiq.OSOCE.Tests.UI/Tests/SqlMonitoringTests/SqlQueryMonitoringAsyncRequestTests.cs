using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.SqlQueryMonitoring.Exceptions;
using Lombiq.Tests.UI.SqlQueryMonitoring.Extensions;
using Lombiq.Tests.UI.SqlQueryMonitoring.Services;
using OpenQA.Selenium;
using Shouldly;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.Tests.UI.Tests.SqlMonitoringTests;

// Demonstrates SQL monitoring with a page load query plus an async browser-triggered API call.
public class SqlQueryMonitoringAsyncRequestTests : Lombiq.Tests.UI.Samples.UITestBase
{
    public SqlQueryMonitoringAsyncRequestTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task SqlQueryMonitoringShouldCapturePageLoadAndAsyncApiQuery() =>
        ExecuteTestAfterSetupAsync(
            async context =>
            {
                const string pagePath = "/Lombiq.Tests.UI.Shortcuts/SqlQueryMonitoringScenario/Index";
                const string asyncApiPath = "/Lombiq.Tests.UI.Shortcuts/SqlQueryMonitoringScenario/AsyncQuery";

                await context.GoToRelativeUrlAsync(pagePath);

                context.DoWithRetriesOrFail(() =>
                    string.Equals(context.GetText(By.Id("async-query-status")), "Completed", StringComparison.Ordinal));

                await context.AssertSqlQueryMonitoringForRequestAsync(
                    pagePath,
                    HttpMethod.Get.Method,
                    summary =>
                    {
                        summary.Executions.ShouldNotBeEmpty(
                            "The initial page request should execute at least one SQL command.");
                        return Task.CompletedTask;
                    });

                await context.AssertSqlQueryMonitoringForRequestAsync(
                    asyncApiPath,
                    HttpMethod.Get.Method,
                    summary =>
                    {
                        summary.Executions.ShouldNotBeEmpty(
                            "The async API request should execute at least one SQL command.");
                        return Task.CompletedTask;
                    });
            });

    [Fact]
    public Task SqlQueryMonitoringShouldDetectDuplicatesWithoutSpecifyingRequestPath() =>
        ExecuteTestAfterSetupAsync(
            async context =>
            {
                const string pagePath = "/Lombiq.Tests.UI.Shortcuts/SqlQueryMonitoringScenario/Index";

                await context.GoToRelativeUrlAsync(pagePath);

                var exception = await Should.ThrowAsync<SqlQueryMonitoringAssertionException>(
                    () => context.AssertSqlQueryMonitoringIncludingFollowUpRequestsAsync());

                exception.SqlQueryMonitoringSummary.RequestPath.ShouldContain(pagePath);
                exception.InnerException.ShouldNotBeNull();
                exception.InnerException.Message.ShouldContain(
                    SqlQueryMonitoringConfiguration.DuplicateCommandFailureCategory);
                exception.InnerException.Message.ShouldContain("Command text executed");
                exception.InnerException.Message.ShouldContain("2 times");
                exception.InnerException.Message.ShouldContain("threshold: 2");
            },
            configuration => configuration.SqlQueryMonitoringConfiguration.DuplicateCommandThreshold = 2);

    [Fact]
    public Task SqlQueryMonitoringShouldCapturePageLoadAndAsyncApiQueryWithoutPageStateWait() =>
        ExecuteTestAfterSetupAsync(
            async context =>
            {
                const string pagePath = "/Lombiq.Tests.UI.Shortcuts/SqlQueryMonitoringScenario/Index";

                await context.GoToRelativeUrlAsync(pagePath);

                await context.AssertSqlQueryMonitoringIncludingFollowUpRequestsAsync(summary =>
                {
                    summary.Executions.Count.ShouldBeGreaterThanOrEqualTo(
                        2,
                        "The combined assertion should capture both page-load and async-request SQL executions.");

                    summary.Executions.Count(entry =>
                            entry.CommandText.Contains("ContentItemIndex", StringComparison.OrdinalIgnoreCase))
                        .ShouldBeGreaterThanOrEqualTo(2);

                    return Task.CompletedTask;
                });
            });
}
