# SQL Query Monitoring Test Scenarios

The reusable sample is in the sample project. This folder keeps the feature-verification tests.

## Scenario Catalog

Reusable samples are in [SqlQueryMonitoringTests.cs](../../../Lombiq.UITestingToolbox/Lombiq.Tests.UI.Samples/Tests/SqlQueryMonitoringTests.cs).
Feature-verification implementations are in [SqlQueryMonitoringTestCases.cs](../../../Lombiq.UITestingToolbox/Lombiq.Tests.UI.Tests.UI/TestCases/SqlQueryMonitoringTestCases.cs).

### Reusable Sample

- [`Lombiq.Tests.UI.Samples.Tests.SqlQueryMonitoringTests.SqlQueryMonitoringShouldWork`](../../../Lombiq.UITestingToolbox/Lombiq.Tests.UI.Samples/Tests/SqlQueryMonitoringTests.cs) Covers:
  - Enabling collection
  - Asserting the latest request
  - Asserting a specific request path with query string
  - Asserting the page request and the async follow-up request separately
  - Combining page and follow-up requests
  - Running automatic page-change assertions on selected pages
  - Setting thresholds from `BeforeNavigation`
  - Setting thresholds by URL pattern
  - Filtering known noisy SQL commands

### Feature Verification Scenarios (Guardrails for Toolbox Behavior)

| Useful For | Scenario We Verify | Test |
| --- | --- | --- |
| Confirm monitoring is off unless enabled. | Collection stays disabled when configured off, and it is off by default. | `SqlQueryMonitoringShouldNotCollectWhenCollectionIsDisabled` |
| Verify failure reporting for all built-in SQL monitoring categories. | Duplicate command-text failures, duplicate command+parameters failures, and oversized result-set failures are all raised and categorized. | `SqlQueryMonitoringFailureScenariosShouldWork` |
| Verify tenant isolation in captured summaries. | Monitoring works on both the switched tenant and the default tenant, and summaries stay tenant-bound. | `SqlQueryMonitoringShouldWorkOnAnotherTenant` |
| Verify request matching rejects wrong or missing query strings for specific requests. | A missing path/method summary fails instead of falling back, and the same path with a different query string does not match. | `SqlQueryMonitoringRequestMatchingScenariosShouldWork` |
| Verify follow-up polling and summary retention edge cases. | Page-load and async API requests are captured separately, combined follow-up assertions can work without an explicit page-state wait, old summaries are not pulled into follow-up assertions, empty summaries are evicted first, and combined follow-up assertions can fail without an explicit request path when duplicates are present. | `SqlQueryMonitoringAsyncRequestScenariosShouldWork` |
| Verify LINQ to DB SQL is captured by monitoring. | LINQ to DB endpoint SQL is captured. | `LinqToDbSamplesShouldBeCapturedBySqlMonitoring` |
| Verify non-default SQL execution sources are covered. | `ISession.RawQueryAsync`, `ISession.RawExecuteNonQueryAsync`, a custom YesSql session, and `IDbConnectionAccessor` are all captured. | `SqlQueryMonitoringAdditionalQuerySourcesShouldWork` |
