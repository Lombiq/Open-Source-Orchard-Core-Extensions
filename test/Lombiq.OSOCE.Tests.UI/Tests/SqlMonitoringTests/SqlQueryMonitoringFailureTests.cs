using Lombiq.Tests.UI.SqlQueryMonitoring.Exceptions;
using Lombiq.Tests.UI.SqlQueryMonitoring.Extensions;
using Lombiq.Tests.UI.SqlQueryMonitoring.Services;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.Tests.UI.Tests.SqlMonitoringTests;

// It's useful to demonstrate how SQL monitoring failures get surfaced. These tests set aggressive thresholds and
// then verify that the expected failure category is reported.
public class SqlQueryMonitoringFailureTests : Lombiq.Tests.UI.Samples.UITestBase
{
    public SqlQueryMonitoringFailureTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task SqlQueryMonitoringShouldSurfaceDuplicateCommandIssues() =>
        ExecuteTestAfterSetupAsync(
            async context =>
            {
                try
                {
                    // The test loads the home page by default. We assert against that request.
                    await context.AssertSqlQueryMonitoringAsync();

                    // If we reach this point, the assertion did not fail as expected.
                    throw new InvalidOperationException("The SQL monitoring assertion did not fail as expected.");
                }
                catch (SqlQueryMonitoringAssertionException exception)
                {
                    // This is expected because the duplicate command text threshold is set to one.
                    exception.InnerException.ShouldNotBeNull();
                    exception.InnerException.Message.ShouldContain(
                        $"[{SqlQueryMonitoringConfiguration.DuplicateCommandFailureCategory}]");
                    exception.InnerException.Message.ShouldContain(
                        SqlQueryMonitoringConfiguration.DuplicateCommandFailureCategory);
                    exception.InnerException.Message.ShouldContain("Command text executed");
                    exception.InnerException.Message.ShouldContain("threshold: 1");
                    _testOutputHelper.WriteLineTimestampedAndDebug(
                        "Caught SqlQueryMonitoringAssertionException as expected for the failure demo.");
                }
            },
            configuration =>
            {
                // Set a low threshold so duplicate command text detection triggers.
                configuration.SqlQueryMonitoringConfiguration.DuplicateCommandThreshold = 1;

                return Task.CompletedTask;
            });

    [Fact]
    public Task SqlQueryMonitoringShouldSurfaceDuplicateParameterIssues() =>
        ExecuteTestAfterSetupAsync(
            async context =>
            {
                try
                {
                    await context.AssertSqlQueryMonitoringAsync();
                    throw new InvalidOperationException("The SQL monitoring assertion did not fail as expected.");
                }
                catch (SqlQueryMonitoringAssertionException exception)
                {
                    // Expect the duplicate command with parameters category.
                    exception.InnerException.ShouldNotBeNull();
                    exception.InnerException.Message.ShouldContain(
                        $"[{SqlQueryMonitoringConfiguration.DuplicateCommandWithParametersFailureCategory}]");
                    exception.InnerException.Message.ShouldContain(
                        SqlQueryMonitoringConfiguration.DuplicateCommandWithParametersFailureCategory);
                    exception.InnerException.Message.ShouldContain("Command text with same parameters executed");
                    exception.InnerException.Message.ShouldContain("threshold: 1");
                    _testOutputHelper.WriteLineTimestampedAndDebug(
                        "Caught SqlQueryMonitoringAssertionException as expected for the login page.");
                }
            },
            configuration =>
            {
                // Leave other thresholds as defaults and tighten only the parameters based one.
                configuration.SqlQueryMonitoringConfiguration.DuplicateCommandWithParametersThreshold = 1;
                return Task.CompletedTask;
            });

    [Fact]
    public Task SqlQueryMonitoringShouldSurfaceOversizedResultSetIssues() =>
        ExecuteTestAfterSetupAsync(
            async context =>
            {
                try
                {
                    await context.AssertSqlQueryMonitoringAsync();
                    throw new InvalidOperationException("The SQL monitoring assertion did not fail as expected.");
                }
                catch (SqlQueryMonitoringAssertionException exception)
                {
                    // Expect the oversized result set category.
                    exception.InnerException.ShouldNotBeNull();
                    exception.InnerException.Message.ShouldContain(
                        $"[{SqlQueryMonitoringConfiguration.ResultSetRowCountFailureCategory}]");
                    exception.InnerException.Message.ShouldContain(
                        SqlQueryMonitoringConfiguration.ResultSetRowCountFailureCategory);
                    exception.InnerException.Message.ShouldContain("Command result set had");
                    exception.InnerException.Message.ShouldContain("threshold: 0");
                    _testOutputHelper.WriteLineTimestampedAndDebug(
                        "Caught SqlQueryMonitoringAssertionException as expected for oversized result sets.");
                }
            },
            configuration =>
            {
                // Any query that returns rows should fail with a zero threshold.
                configuration.SqlQueryMonitoringConfiguration.ResultSetRowCountThreshold = 0;
                return Task.CompletedTask;
            });

    [Fact]
    public Task SqlQueryMonitoringShouldSurfaceAllIssues() =>
        ExecuteTestAfterSetupAsync(
            async context =>
            {
                try
                {
                    await context.AssertSqlQueryMonitoringAsync();
                    throw new InvalidOperationException("The SQL monitoring assertion did not fail as expected.");
                }
                catch (SqlQueryMonitoringAssertionException exception)
                {
                    // Expect all failure categories.
                    exception.InnerException.ShouldNotBeNull();
                    exception.InnerException.Message.ShouldContain(
                        SqlQueryMonitoringConfiguration.DuplicateCommandFailureCategory);
                    exception.InnerException.Message.ShouldContain(
                        SqlQueryMonitoringConfiguration.DuplicateCommandWithParametersFailureCategory);
                    exception.InnerException.Message.ShouldContain(
                        SqlQueryMonitoringConfiguration.ResultSetRowCountFailureCategory);
                    exception.InnerException.Message.ShouldContain("Command text executed");
                    exception.InnerException.Message.ShouldContain("Command text with same parameters executed");
                    exception.InnerException.Message.ShouldContain("Command result set had");
                    exception.InnerException.Message.ShouldContain("threshold: 1");
                    exception.InnerException.Message.ShouldContain("threshold: 0");
                    _testOutputHelper.WriteLineTimestampedAndDebug(
                        "Caught SqlQueryMonitoringAssertionException as expected for oversized result sets.");
                }
            },
            configuration =>
            {
                // Set all thresholds to trigger all categories.
                configuration.SqlQueryMonitoringConfiguration.DuplicateCommandThreshold = 1;
                configuration.SqlQueryMonitoringConfiguration.DuplicateCommandWithParametersThreshold = 1;
                configuration.SqlQueryMonitoringConfiguration.ResultSetRowCountThreshold = 0;
                return Task.CompletedTask;
            });
}

// NEXT STATION: Head over to Tests/SqlQueryMonitoringTenantTests.cs.
