# SQL Query Monitoring Test Scenarios

This folder contains end-to-end scenarios for SQL query monitoring in UI tests. The goal is to verify what the
monitoring API captures, how assertions behave, and how to tune or scope checks for real projects.

## Scenario Catalog

| Scenario We Verify | Useful For | Test |
| --- | --- | --- |
| Default monitoring assertions and custom summary assertion after navigation. | Establishing a baseline setup and showing how to add custom validation logic. | `SqlQueryMonitoringBasicsTests.SqlQueryMonitoringShouldCatchDuplicatesAndLargeResults` |
| Monitoring collection can be switched off. | Running UI tests without SQL monitoring overhead when performance checks are out of scope. | `SqlQueryMonitoringDisableCollectionTests.SqlQueryMonitoringShouldAllowDisablingCollection` |
| Duplicate command-text failures are raised and clearly reported. | Catching possible SELECT N+1 style issues. | `SqlQueryMonitoringFailureTests.SqlQueryMonitoringShouldSurfaceDuplicateCommandIssues` |
| Duplicate command+parameters failures are raised and clearly reported. | Catching repeated identical queries that suggest missing caching or repeated call paths. | `SqlQueryMonitoringFailureTests.SqlQueryMonitoringShouldSurfaceDuplicateParameterIssues` |
| Oversized result-set failures are raised and clearly reported. | Catching missing SQL-side filtering or paging. | `SqlQueryMonitoringFailureTests.SqlQueryMonitoringShouldSurfaceOversizedResultSetIssues` |
| All three failure categories can be reported together. | Validating combined diagnostics and error-message quality. | `SqlQueryMonitoringFailureTests.SqlQueryMonitoringShouldSurfaceAllIssues` |
| Monitoring works after tenant switch and on the default tenant too. | Multi-tenant test suites where SQL checks must stay tenant-aware. | `SqlQueryMonitoringTenantTests.SqlQueryMonitoringShouldWorkOnAnotherTenant` |
| Thresholds can be changed dynamically before navigation. | Features with different expected query shapes per page. | `SqlQueryMonitoringThresholdsTests.SqlQueryMonitoringShouldAllowPerPageThresholds` |
| Threshold profiles can be mapped to URL patterns. | Centralized, route-based threshold management. | `SqlQueryMonitoringThresholdsTests.SqlQueryMonitoringShouldAllowRegexBasedPerPageThresholds` |
| Auto page-change monitoring can be filtered by a custom predicate. | Limiting monitoring to selected pages to reduce noise. | `SqlQueryMonitoringPageChangeRuleTests.SqlQueryMonitoringShouldRespectPageChangeRule` |
| Regex-based execution filtering can ignore known noisy queries. | Keeping thresholds strict while excluding known benign queries. | `SqlQueryMonitoringFilteringTests.SqlQueryMonitoringShouldAllowIgnoringKnownQueries` |
| Request path and query string are captured correctly for a navigated page request. | Validating query-string-sensitive diagnostics on page-change assertions. | `SqlQueryMonitoringRequestMatchingTests.SqlQueryMonitoringShouldCaptureRequestPathAndQueryForNavigatedPage` |
| Page-load request and async follow-up API request can be asserted separately by path. | Verifying mixed request flows without summary conflation. | `SqlQueryMonitoringAsyncRequestTests.SqlQueryMonitoringShouldCapturePageLoadAndAsyncApiQuery` |
| Combined follow-up assertion catches duplicates across page and async requests. | Detecting cross-request duplication in a single interaction. | `SqlQueryMonitoringAsyncRequestTests.SqlQueryMonitoringShouldDetectDuplicatesWithoutSpecifyingRequestPath` |
| Follow-up aggregation API can capture late async requests without explicit page-state polling. | Stabilizing tests when async traffic arrives shortly after navigation. | `SqlQueryMonitoringAsyncRequestTests.SqlQueryMonitoringShouldCapturePageLoadAndAsyncApiQueryWithoutPageStateWait` |
| LINQ to DB requests are captured from a navigated endpoint request. | Ensuring monitoring is not limited to one ORM/query entry path while keeping page-change assertion style. | `SqlQueryMonitoringLinqToDbTests.LinqToDbSamplesShouldBeCapturedBySqlMonitoring` |
| `ISession.RawQueryAsync` executions are captured from a navigated page-change request. | Coverage for raw SQL read paths while still validating page-change assertions. | `SqlQueryMonitoringAdditionalQuerySourcesTests.SqlQueryMonitoringShouldCaptureRawQuery` |
| `ISession.RawExecuteNonQueryAsync` executions are captured from a navigated page-change request. | Coverage for raw SQL write/non-query paths while still validating page-change assertions. | `SqlQueryMonitoringAdditionalQuerySourcesTests.SqlQueryMonitoringShouldCaptureRawExecuteNonQuery` |
| Queries from manually created YesSql sessions are captured from a navigated page-change request. | Validating non-default session lifecycle usage while still validating page-change assertions. | `SqlQueryMonitoringAdditionalQuerySourcesTests.SqlQueryMonitoringShouldCaptureCustomSessionQuery` |
| Direct `IDbConnectionAccessor` command execution is captured from a navigated page-change request. | Validating low-level ADO.NET usage while still validating page-change assertions. | `SqlQueryMonitoringAdditionalQuerySourcesTests.SqlQueryMonitoringShouldCaptureDirectConnectionQuery` |
