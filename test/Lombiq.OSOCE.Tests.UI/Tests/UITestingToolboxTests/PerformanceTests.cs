using Lombiq.Tests.UI.Extensions;
using System;
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
                    await context.ExecuteLoggedAsync(
                        "GoToUrlAsync",
                        "blog/post-1",
                        async () =>
                        {
                            await context.DoWithRetriesUntilNavigationHasOccurredOrFailAsync(
                                () => context.Driver.Navigate().GoToUrlAsync(new Uri(context.Scope.BaseUri, "blog/post-1")));

                            context.AppendTestDump("post-1 PageSource " + i + ".html", content: context.Driver.PageSource);
                        });

                    await context.ExecuteLoggedAsync(
                        "GoToUrlAsync",
                        "about",
                        async () =>
                        {
                            await context.DoWithRetriesUntilNavigationHasOccurredOrFailAsync(
                                () => context.Driver.Navigate().GoToUrlAsync(new Uri(context.Scope.BaseUri, "about")));

                            context.AppendTestDump("about PageSource " + i + ".html", content: context.Driver.PageSource);
                        });

                    await context.ExecuteLoggedAsync(
                        "GoToUrlAsync",
                        "/",
                        async () =>
                        {
                            await context.DoWithRetriesUntilNavigationHasOccurredOrFailAsync(
                                () => context.Driver.Navigate().GoToUrlAsync(context.Scope.BaseUri));

                            context.AppendTestDump("homepage PageSource " + i + ".html", content: context.Driver.PageSource);
                        });

                    //if (true)
                    //{
                    //    await context.GoToRelativeUrlAsync("/blog/post-1");
                    //    await context.GoToRelativeUrlAsync("/about");
                    //    await context.GoToHomePageAsync();
                    //}

                    _testOutputHelper.WriteLine($"Iteration {i + 1} completed.");
                }
            },
            configuration =>
            {
                configuration.AccessibilityCheckingConfiguration.RunAccessibilityCheckingAssertionOnAllPageChanges = false;
                configuration.HtmlValidationConfiguration.RunHtmlValidationAssertionOnAllPageChanges = false;
            });
}
