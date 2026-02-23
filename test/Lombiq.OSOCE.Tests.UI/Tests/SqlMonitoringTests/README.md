# SQL Query Monitoring Test Scenarios

This folder contains end-to-end scenarios for SQL query monitoring in UI tests. The goal is to verify what the monitoring API captures, how assertions behave, and how to tune or scope checks for real projects.

## Scenario Catalog

| Useful For | Scenario We Verify | Test |
| --- | --- | --- |
| Baseline setup and customization. | Default assertions + custom summary. | [`SqlQueryMonitoringShouldCatchDuplicatesAndLargeResults`](SqlQueryMonitoringBasicsTests.cs) |
| No-overhead UI runs. | Collection can be disabled. | [`SqlQueryMonitoringShouldAllowDisablingCollection`](SqlQueryMonitoringDisableCollectionTests.cs) |
| SELECT N+1 detection. | Duplicate command-text failure. | [`SqlQueryMonitoringShouldSurfaceDuplicateCommandIssues`](SqlQueryMonitoringFailureTests.cs) |
| Missing-cache detection. | Duplicate command+parameters failure. | [`SqlQueryMonitoringShouldSurfaceDuplicateParameterIssues`](SqlQueryMonitoringFailureTests.cs) |
| Missing filter/paging detection. | Oversized result-set failure. | [`SqlQueryMonitoringShouldSurfaceOversizedResultSetIssues`](SqlQueryMonitoringFailureTests.cs) |
| Combined diagnostics validation. | All failure categories together. | [`SqlQueryMonitoringShouldSurfaceAllIssues`](SqlQueryMonitoringFailureTests.cs) |
| Tenant-aware monitoring. | Works on switched and default tenant. | [`SqlQueryMonitoringShouldWorkOnAnotherTenant`](SqlQueryMonitoringTenantTests.cs) |
| Per-page dynamic tuning. | Thresholds set in `BeforeNavigation`. | [`SqlQueryMonitoringShouldAllowPerPageThresholds`](SqlQueryMonitoringThresholdsTests.cs) |
| Regex-based threshold tuning. | Threshold profiles by URL pattern. | [`SqlQueryMonitoringShouldAllowRegexBasedPerPageThresholds`](SqlQueryMonitoringThresholdsTests.cs) |
| Scope monitoring to selected pages. | Custom page-change rule is respected. | [`SqlQueryMonitoringShouldRespectPageChangeRule`](SqlQueryMonitoringPageChangeRuleTests.cs) |
| Suppress known benign noise. | Regex execution filter is applied. | [`SqlQueryMonitoringShouldAllowIgnoringKnownQueries`](SqlQueryMonitoringFilteringTests.cs) |
| Query-string-sensitive diagnostics. | Path + query are captured on navigation. | [`SqlQueryMonitoringShouldCaptureRequestPathAndQueryForNavigatedPage`](SqlQueryMonitoringRequestMatchingTests.cs) |
| Mixed-flow request matching. | Page-load and async API asserted by path. | [`SqlQueryMonitoringShouldCapturePageLoadAndAsyncApiQuery`](SqlQueryMonitoringAsyncRequestTests.cs) |
| Cross-request duplicate detection. | Follow-up-inclusive duplicate detection. | [`SqlQueryMonitoringShouldDetectDuplicatesWithoutSpecifyingRequestPath`](SqlQueryMonitoringAsyncRequestTests.cs) |
| Stable async follow-up capture. | Follow-up API captured without page-state wait. | [`SqlQueryMonitoringShouldCapturePageLoadAndAsyncApiQueryWithoutPageStateWait`](SqlQueryMonitoringAsyncRequestTests.cs) |
| ORM-path coverage. | LINQ to DB endpoint SQL is captured. | [`LinqToDbSamplesShouldBeCapturedBySqlMonitoring`](SqlQueryMonitoringLinqToDbTests.cs) |
| Raw SQL read-path coverage. | `ISession.RawQueryAsync` is captured. | [`SqlQueryMonitoringShouldCaptureRawQuery`](SqlQueryMonitoringAdditionalQuerySourcesTests.cs) |
| Raw SQL write-path coverage. | `ISession.RawExecuteNonQueryAsync` is captured. | [`SqlQueryMonitoringShouldCaptureRawExecuteNonQuery`](SqlQueryMonitoringAdditionalQuerySourcesTests.cs) |
| Custom session-path coverage. | Manually created session query is captured. | [`SqlQueryMonitoringShouldCaptureCustomSessionQuery`](SqlQueryMonitoringAdditionalQuerySourcesTests.cs) |
| Low-level ADO.NET coverage. | `IDbConnectionAccessor` query is captured. | [`SqlQueryMonitoringShouldCaptureDirectConnectionQuery`](SqlQueryMonitoringAdditionalQuerySourcesTests.cs) |
