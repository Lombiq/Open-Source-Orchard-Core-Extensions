using Lombiq.Tests.UI.Extensions;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.Tests.UI.Tests.UITestingToolboxTests;

public class PerformanceTests : UITestBase
{
    public PerformanceTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task TestSeleniumPerformance() =>
        ExecuteTestAfterSetupAsync(
            async context =>
            {
                for (int i = 0; i < 99; i++)
                {
                    await context.GoToRelativeUrlAsync("/blog/post-1");
                    await context.GoToRelativeUrlAsync("/about");
                    await context.GoToHomePageAsync();

                    _testOutputHelper.WriteLine($"Iteration {i + 1} completed.");
                }
            },
            configuration =>
            {
                configuration.AccessibilityCheckingConfiguration.RunAccessibilityCheckingAssertionOnAllPageChanges = false;
                configuration.HtmlValidationConfiguration.RunHtmlValidationAssertionOnAllPageChanges = false;
            });
}
