# SQL Query Monitoring Test Scenarios

This folder contains end-to-end scenarios for SQL query monitoring in UI tests. The goal is to verify what the monitoring API captures, how assertions behave, and how to tune or scope checks for real projects.

## Scenario Catalog

Scenario implementations are in [SqlQueryMonitoringTestCases.cs](../../../Lombiq.UITestingToolbox/Lombiq.Tests.UI.Tests.UI/TestCases/SqlQueryMonitoringTestCases.cs).

### Reusable Scenarios (Useful in Your Own Tests)

| Useful For | Scenario We Verify | Test |
| --- | --- | --- |
| Get started with SQL monitoring in a normal page test. | Default assertions + custom summary. | `SqlQueryMonitoringShouldCatchDuplicatesAndLargeResults` |
| Tune thresholds per page, both explicitly and by URL pattern. | Thresholds set in `BeforeNavigation`; threshold profiles by URL pattern. | `SqlQueryMonitoringThresholdScenariosShouldWork` |
| Run automatic checks only on selected pages. | Custom page-change rule is respected. | `SqlQueryMonitoringShouldRespectPageChangeRule` |
| Ignore known noisy queries you accept. | Regex execution filter is applied. | `SqlQueryMonitoringShouldAllowIgnoringKnownQueries` |
| Assert page request and async request behavior, including combined follow-up assertions. | Page-load and async API asserted by path; follow-up-inclusive duplicate detection; follow-up API captured without explicit page-state wait. | `SqlQueryMonitoringAsyncRequestScenariosShouldWork` |

### Feature Verification Scenarios (Guardrails for Toolbox Behavior)

| Useful For | Scenario We Verify | Test |
| --- | --- | --- |
| Confirm monitoring is off unless enabled. | Collection stays disabled when configured off (and is off by default). | `SqlQueryMonitoringShouldNotCollectWhenCollectionIsDisabled` |
| Verify failure reporting for all built-in SQL monitoring categories. | Duplicate command-text failure is raised and categorized; duplicate command+parameters failure is raised and categorized; oversized result-set failure is raised and categorized; all failure categories are surfaced together. | `SqlQueryMonitoringFailureScenariosShouldWork` |
| Verify tenant isolation in captured summaries. | Monitoring works on switched and default tenant, and summaries stay tenant-bound. | `SqlQueryMonitoringShouldWorkOnAnotherTenant` |
| Verify request-path/query matching and missing-summary behavior. | Path + query are captured on navigation; missing path/method summary fails instead of falling back; same path with different query does not match. | `SqlQueryMonitoringRequestMatchingScenariosShouldWork` |
| Verify follow-up polling and summary retention edge cases. | Old summaries are excluded during follow-up aggregation; store retention favors removing empty summaries first. | `SqlQueryMonitoringAsyncRequestScenariosShouldWork` |
| Verify LINQ to DB SQL is captured by monitoring. | LINQ to DB endpoint SQL is captured. | `LinqToDbSamplesShouldBeCapturedBySqlMonitoring` |
| Verify non-default SQL execution sources are covered. | `ISession.RawQueryAsync` is captured; `ISession.RawExecuteNonQueryAsync` is captured; manually created YesSql session query is captured; `IDbConnectionAccessor` query is captured. | `SqlQueryMonitoringAdditionalQuerySourcesShouldWork` |
