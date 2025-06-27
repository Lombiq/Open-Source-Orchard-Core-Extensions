using System;
using System.IO;
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
                var driver = context.Driver;
                var baseUri = context.Scope.BaseUri;

                for (int i = 0; i < 3; i++)
                {
                    await driver.Navigate().GoToUrlAsync(new Uri(baseUri, "blog/post-1"));
                    // Writing to file to check the contents and to make sure that the page source is indeed fully loaded.
                    await File.WriteAllTextAsync("post-1 PageSource " + i + ".html", driver.PageSource);

                    await driver.Navigate().GoToUrlAsync(new Uri(baseUri, "about"));
                    await File.WriteAllTextAsync("about PageSource " + i + ".html", driver.PageSource);

                    await driver.Navigate().GoToUrlAsync(baseUri);
                    await File.WriteAllTextAsync("homepage PageSource " + i + ".html", driver.PageSource);

                    _testOutputHelper.WriteLine($"Iteration {i + 1} completed.");
                }
            },
            configuration =>
            {
                configuration.AccessibilityCheckingConfiguration.RunAccessibilityCheckingAssertionOnAllPageChanges = false;
                configuration.HtmlValidationConfiguration.RunHtmlValidationAssertionOnAllPageChanges = false;
            });
}
