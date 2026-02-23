using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.SqlQueryMonitoring.Extensions;
using System.Threading.Tasks;
using Xunit;
using static Lombiq.Tests.UI.SqlQueryMonitoring.Services.SqlQueryMonitoringConfiguration;

namespace Lombiq.OSOCE.Tests.UI.Tests.SqlMonitoringTests;

// You can tune thresholds per page to match the expected behavior of each feature.
public class SqlQueryMonitoringThresholdsTests : Lombiq.Tests.UI.Samples.UITestBase
{
    public SqlQueryMonitoringThresholdsTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    // You can tune thresholds per page. Here we tighten them for the categories page while keeping others looser.
    [Fact]
    public Task SqlQueryMonitoringShouldAllowPerPageThresholds() =>
        ExecuteTestAfterSetupAsync(
            async context =>
            {
                await context.GoToRelativeUrlAsync("/categories/travel");
                await context.GoToRelativeUrlAsync("/about");
            },
            configuration =>
            {
                configuration.Events.BeforeNavigation += (_, targetUri) =>
                {
                    var thresholds = configuration.SqlQueryMonitoringConfiguration;

                    if (targetUri.AbsolutePath.Contains("/categories"))
                    {
                        thresholds.DuplicateCommandThreshold = 20;
                        thresholds.DuplicateCommandWithParametersThreshold = 10;
                        thresholds.ResultSetRowCountThreshold = 100;
                    }
                    else
                    {
                        thresholds.DuplicateCommandThreshold = 30;
                        thresholds.DuplicateCommandWithParametersThreshold = 15;
                        thresholds.ResultSetRowCountThreshold = 200;
                    }

                    return Task.CompletedTask;
                };

                return Task.CompletedTask;
            });

    // You can also configure per-page thresholds using regex rules for the URL.
    [Fact]
    public Task SqlQueryMonitoringShouldAllowRegexBasedPerPageThresholds() =>
        ExecuteTestAfterSetupAsync(
            async context =>
            {
                // Each navigation will apply a matching rule (or the default thresholds if no rule matches).
                await context.GoToRelativeUrlAsync("/categories/travel");
                await context.GoToRelativeUrlAsync("/about");
                await context.GoToRelativeUrlAsync("/");
            },
            configuration =>
            {
                // Configure defaults first, then override them for specific routes via regex patterns.
                // The first matching pattern wins, and patterns are evaluated against the request path.
                configuration.ConfigureSqlQueryMonitoringThresholdsForPages(
                    new SqlQueryMonitoringThresholds(
                        DuplicateCommandThreshold: 30,
                        DuplicateCommandWithParametersThreshold: 15,
                        ResultSetRowCountThreshold: 200),
                    (Pattern: @"^/categories/.*", Thresholds: new SqlQueryMonitoringThresholds(
                        DuplicateCommandThreshold: 20,
                        DuplicateCommandWithParametersThreshold: 10,
                        ResultSetRowCountThreshold: 100)),
                    (Pattern: @"^/about$", Thresholds: new SqlQueryMonitoringThresholds(
                        DuplicateCommandThreshold: 25,
                        DuplicateCommandWithParametersThreshold: 12,
                        ResultSetRowCountThreshold: 150)));
                return Task.CompletedTask;
            });
}

// NEXT STATION: Head over to Tests/SqlQueryMonitoringPageChangeRuleTests.cs.
