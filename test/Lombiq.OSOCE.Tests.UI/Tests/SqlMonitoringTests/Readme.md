# SQL Query Monitoring Test Scenarios

This folder contains end-to-end scenarios for SQL query monitoring in UI tests. Reusable usage samples live in the sample project, while this folder keeps the feature-verification coverage.

## Scenario Catalog

Reusable samples are in [SqlQueryMonitoringTests.cs](../../../Lombiq.UITestingToolbox/Lombiq.Tests.UI.Samples/Tests/SqlQueryMonitoringTests.cs).
Feature-verification implementations are in [SqlQueryMonitoringTestCases.cs](../../../Lombiq.UITestingToolbox/Lombiq.Tests.UI.Tests.UI/TestCases/SqlQueryMonitoringTestCases.cs).

### Reusable Sample

- `SqlQueryMonitoringShouldWorkAllSample`
Shows the basic SQL monitoring API the same way you'd use it in a real project. It demonstrates:
  - enabling collection
  - asserting the latest request
  - asserting a specific request path with query string
  - asserting the page request and the async follow-up request separately
  - combining page and follow-up requests
  - running automatic page-change assertions on selected pages
  - setting thresholds from `BeforeNavigation`
  - setting thresholds by URL pattern
  - filtering known noisy SQL commands

### Feature Verification Scenarios (Guardrails for Toolbox Behavior)

| Useful For | Scenario We Verify | Test |
| --- | --- | --- |
| Confirm monitoring is off unless enabled. | Collection stays disabled when configured off (and is off by default). | `SqlQueryMonitoringShouldNotCollectWhenCollectionIsDisabled` |
| Verify failure reporting for all built-in SQL monitoring categories. | Duplicate command-text failure is raised and categorized; duplicate command+parameters failure is raised and categorized; oversized result-set failure is raised and categorized; all failure categories are surfaced together. | `SqlQueryMonitoringFailureScenariosShouldWork` |
| Verify tenant isolation in captured summaries. | Monitoring works on switched and default tenant, and summaries stay tenant-bound. | `SqlQueryMonitoringShouldWorkOnAnotherTenant` |
| Verify request matching rejects wrong or missing query strings for specific requests. | Missing path/method summary fails instead of falling back; same path with different query does not match. | `SqlQueryMonitoringRequestMatchingScenariosShouldWork` |
| Verify follow-up polling and summary retention edge cases. | Page-load and async API requests are captured separately; combined follow-up assertions can work without an explicit page-state wait; old summaries are excluded during follow-up aggregation; store retention favors removing empty summaries first; combined follow-up assertions can fail without an explicit request path when duplicates are present. | `SqlQueryMonitoringAsyncRequestScenariosShouldWork` |
| Verify LINQ to DB SQL is captured by monitoring. | LINQ to DB endpoint SQL is captured. | `LinqToDbSamplesShouldBeCapturedBySqlMonitoring` |
| Verify non-default SQL execution sources are covered. | `ISession.RawQueryAsync` is captured; `ISession.RawExecuteNonQueryAsync` is captured; manually created YesSql session query is captured; `IDbConnectionAccessor` query is captured. | `SqlQueryMonitoringAdditionalQuerySourcesShouldWork` |
