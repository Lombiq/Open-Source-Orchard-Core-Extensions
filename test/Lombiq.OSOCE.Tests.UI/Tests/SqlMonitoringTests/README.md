# SQL Query Monitoring Test Scenarios

This folder contains end-to-end scenarios for SQL query monitoring in UI tests. The goal is to verify what the monitoring API captures, how assertions behave, and how to tune or scope checks for real projects.

## Scenario Catalog

Scenario implementations are in [SqlQueryMonitoringTestCases.cs](../../../Lombiq.UITestingToolbox/Lombiq.Tests.UI.Tests.UI/TestCases/SqlQueryMonitoringTestCases.cs).

### Reusable Scenarios (Useful in Your Own Tests)

| Useful For | Scenario We Verify | Test |
| --- | --- | --- |
| Get started with SQL monitoring in a normal page test. | Default assertions + custom summary. | `SqlQueryMonitoringShouldCatchDuplicatesAndLargeResults` |
| Use different thresholds on specific pages. | Thresholds set in `BeforeNavigation`. | `SqlQueryMonitoringShouldAllowPerPageThresholds` |
| Apply thresholds by URL pattern without custom branching code. | Threshold profiles by URL pattern. | `SqlQueryMonitoringShouldAllowRegexBasedPerPageThresholds` |
| Run automatic checks only on selected pages. | Custom page-change rule is respected. | `SqlQueryMonitoringShouldRespectPageChangeRule` |
| Ignore known noisy queries you accept. | Regex execution filter is applied. | `SqlQueryMonitoringShouldAllowIgnoringKnownQueries` |
| Assert page request and async API request separately. | Page-load and async API asserted by path. | `SqlQueryMonitoringShouldCapturePageLoadAndAsyncApiQuery` |
| Catch duplicates across page load and follow-up requests together. | Follow-up-inclusive duplicate detection. | `SqlQueryMonitoringShouldDetectDuplicatesWithoutSpecifyingRequestPath` |

### Feature Verification Scenarios (Guardrails for Toolbox Behavior)

| Useful For | Scenario We Verify | Test |
| --- | --- | --- |
| Confirm monitoring is off unless enabled. | Collection stays disabled when configured off (and is off by default). | `SqlQueryMonitoringShouldNotCollectWhenCollectionIsDisabled` |
| Verify duplicate query-text failures are reported. | Duplicate command-text failure is raised and categorized. | `SqlQueryMonitoringShouldSurfaceDuplicateCommandIssues` |
| Verify duplicate query+parameters failures are reported. | Duplicate command+parameters failure is raised and categorized. | `SqlQueryMonitoringShouldSurfaceDuplicateParameterIssues` |
| Verify large result-set failures are reported. | Oversized result-set failure is raised and categorized. | `SqlQueryMonitoringShouldSurfaceOversizedResultSetIssues` |
| Verify multiple failure categories are reported together. | All failure categories are surfaced together. | `SqlQueryMonitoringShouldSurfaceAllIssues` |
| Verify tenant isolation in captured summaries. | Monitoring works on switched and default tenant, and summaries stay tenant-bound. | `SqlQueryMonitoringShouldWorkOnAnotherTenant` |
| Verify request path and query are captured correctly. | Path + query are captured on navigation. | `SqlQueryMonitoringShouldCaptureRequestPathAndQueryForNavigatedPage` |
| Verify request-specific assertion fails when request is missing. | Missing path/method summary fails instead of falling back. | `SqlQueryMonitoringShouldFailWhenSpecificRequestSummaryIsMissing` |
| Verify request-specific assertion requires query-string match. | Same path with different query does not match. | `SqlQueryMonitoringShouldNotMatchDifferentQueryStringForSpecificRequest` |
| Verify follow-up polling captures late async requests. | Follow-up API is captured without explicit page-state wait. | `SqlQueryMonitoringShouldCapturePageLoadAndAsyncApiQueryWithoutPageStateWait` |
| Verify stale queue entries are ignored during follow-up assertions. | Old summaries are excluded during follow-up aggregation. | `SqlQueryMonitoringShouldIgnoreStaleSummariesWhenAggregatingFollowUpRequests` |
| Verify LINQ to DB SQL is captured by monitoring. | LINQ to DB endpoint SQL is captured. | `LinqToDbSamplesShouldBeCapturedBySqlMonitoring` |
| Verify `ISession.RawQueryAsync` SQL is captured. | `ISession.RawQueryAsync` is captured. | `SqlQueryMonitoringShouldCaptureRawQuery` |
| Verify `ISession.RawExecuteNonQueryAsync` SQL is captured. | `ISession.RawExecuteNonQueryAsync` is captured. | `SqlQueryMonitoringShouldCaptureRawExecuteNonQuery` |
| Verify custom YesSql session SQL is captured. | Manually created session query is captured. | `SqlQueryMonitoringShouldCaptureCustomSessionQuery` |
| Verify direct ADO.NET SQL is captured. | `IDbConnectionAccessor` query is captured. | `SqlQueryMonitoringShouldCaptureDirectConnectionQuery` |
