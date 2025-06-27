using Lombiq.Tests.UI.Extensions;
using OpenQA.Selenium.Internal.Logging;
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

                Log.SetLevel(LogEventLevel.Trace);
                Log.Handlers.Add(new FileLogHandler(context.Id + ".log"));

                // driver is an IWebDriver instance, baseUri is the base URL of the app being tested.

                for (int i = 0; i < 99; i++)
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

                File.Copy(context.Id + ".log", context.Id + "2.log");
                context.AppendTestDump(context.Id + "2.log");
            },
            configuration =>
            {
                configuration.AccessibilityCheckingConfiguration.RunAccessibilityCheckingAssertionOnAllPageChanges = false;
                configuration.HtmlValidationConfiguration.RunHtmlValidationAssertionOnAllPageChanges = false;
            });
}
