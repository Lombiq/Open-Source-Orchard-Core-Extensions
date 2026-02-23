# SQL Query Monitoring Test Scenarios

This folder contains end-to-end scenarios for SQL query monitoring in UI tests. The goal is to verify what the
monitoring API captures, how assertions behave, and how to tune or scope checks for real projects.

## Scenario Catalog

| Useful For | Scenario We Verify | Test |
| --- | --- | --- |
| Baseline setup and customization. | Default monitoring assertions and custom summary assertion after navigation. | [`SqlQueryMonitoringBasicsTests.SqlQueryMonitoringShouldCatchDuplicatesAndLargeResults`](SqlQueryMonitoringBasicsTests.cs) |
| No-overhead UI runs. | Monitoring collection can be switched off. | [`SqlQueryMonitoringDisableCollectionTests.SqlQueryMonitoringShouldAllowDisablingCollection`](SqlQueryMonitoringDisableCollectionTests.cs) |
| SELECT N+1 detection. | Duplicate command-text failures are raised and clearly reported. | [`SqlQueryMonitoringFailureTests.SqlQueryMonitoringShouldSurfaceDuplicateCommandIssues`](SqlQueryMonitoringFailureTests.cs) |
| Missing-cache detection. | Duplicate command+parameters failures are raised and clearly reported. | [`SqlQueryMonitoringFailureTests.SqlQueryMonitoringShouldSurfaceDuplicateParameterIssues`](SqlQueryMonitoringFailureTests.cs) |
| Missing filter/paging detection. | Oversized result-set failures are raised and clearly reported. | [`SqlQueryMonitoringFailureTests.SqlQueryMonitoringShouldSurfaceOversizedResultSetIssues`](SqlQueryMonitoringFailureTests.cs) |
| Combined diagnostics validation. | All three failure categories can be reported together. | [`SqlQueryMonitoringFailureTests.SqlQueryMonitoringShouldSurfaceAllIssues`](SqlQueryMonitoringFailureTests.cs) |
| Tenant-aware monitoring. | Monitoring works after tenant switch and on the default tenant too. | [`SqlQueryMonitoringTenantTests.SqlQueryMonitoringShouldWorkOnAnotherTenant`](SqlQueryMonitoringTenantTests.cs) |
| Per-page dynamic tuning. | Thresholds can be changed dynamically before navigation. | [`SqlQueryMonitoringThresholdsTests.SqlQueryMonitoringShouldAllowPerPageThresholds`](SqlQueryMonitoringThresholdsTests.cs) |
| Regex-based threshold tuning. | Threshold profiles can be mapped to URL patterns. | [`SqlQueryMonitoringThresholdsTests.SqlQueryMonitoringShouldAllowRegexBasedPerPageThresholds`](SqlQueryMonitoringThresholdsTests.cs) |
| Scope monitoring to selected pages. | Auto page-change monitoring can be filtered by a custom predicate. | [`SqlQueryMonitoringPageChangeRuleTests.SqlQueryMonitoringShouldRespectPageChangeRule`](SqlQueryMonitoringPageChangeRuleTests.cs) |
| Suppress known benign noise. | Regex-based execution filtering can ignore known noisy queries. | [`SqlQueryMonitoringFilteringTests.SqlQueryMonitoringShouldAllowIgnoringKnownQueries`](SqlQueryMonitoringFilteringTests.cs) |
| Query-string-sensitive diagnostics. | Request path and query string are captured correctly for a navigated page request. | [`SqlQueryMonitoringRequestMatchingTests.SqlQueryMonitoringShouldCaptureRequestPathAndQueryForNavigatedPage`](SqlQueryMonitoringRequestMatchingTests.cs) |
| Mixed-flow request matching. | Page-load request and async follow-up API request can be asserted separately by path. | [`SqlQueryMonitoringAsyncRequestTests.SqlQueryMonitoringShouldCapturePageLoadAndAsyncApiQuery`](SqlQueryMonitoringAsyncRequestTests.cs) |
| Cross-request duplicate detection. | Combined follow-up assertion catches duplicates across page and async requests. | [`SqlQueryMonitoringAsyncRequestTests.SqlQueryMonitoringShouldDetectDuplicatesWithoutSpecifyingRequestPath`](SqlQueryMonitoringAsyncRequestTests.cs) |
| Stable async follow-up capture. | Follow-up aggregation API can capture late async requests without explicit page-state polling. | [`SqlQueryMonitoringAsyncRequestTests.SqlQueryMonitoringShouldCapturePageLoadAndAsyncApiQueryWithoutPageStateWait`](SqlQueryMonitoringAsyncRequestTests.cs) |
| ORM-path coverage. | LINQ to DB requests are captured from a navigated endpoint request. | [`SqlQueryMonitoringLinqToDbTests.LinqToDbSamplesShouldBeCapturedBySqlMonitoring`](SqlQueryMonitoringLinqToDbTests.cs) |
| Raw SQL read-path coverage. | `ISession.RawQueryAsync` executions are captured from a navigated page-change request. | [`SqlQueryMonitoringAdditionalQuerySourcesTests.SqlQueryMonitoringShouldCaptureRawQuery`](SqlQueryMonitoringAdditionalQuerySourcesTests.cs) |
| Raw SQL write-path coverage. | `ISession.RawExecuteNonQueryAsync` executions are captured from a navigated page-change request. | [`SqlQueryMonitoringAdditionalQuerySourcesTests.SqlQueryMonitoringShouldCaptureRawExecuteNonQuery`](SqlQueryMonitoringAdditionalQuerySourcesTests.cs) |
| Custom session-path coverage. | Queries from manually created YesSql sessions are captured from a navigated page-change request. | [`SqlQueryMonitoringAdditionalQuerySourcesTests.SqlQueryMonitoringShouldCaptureCustomSessionQuery`](SqlQueryMonitoringAdditionalQuerySourcesTests.cs) |
| Low-level ADO.NET coverage. | Direct `IDbConnectionAccessor` command execution is captured from a navigated page-change request. | [`SqlQueryMonitoringAdditionalQuerySourcesTests.SqlQueryMonitoringShouldCaptureDirectConnectionQuery`](SqlQueryMonitoringAdditionalQuerySourcesTests.cs) |
