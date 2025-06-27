using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
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
                    if (false)
                    {
                        await context.Driver.Navigate().GoToUrlAsync(new Uri(context.Scope.BaseUri, "blog/post-1"));
                        await context.Driver.Navigate().GoToUrlAsync(new Uri(context.Scope.BaseUri, "about"));
                        await context.Driver.Navigate().GoToUrlAsync(context.Scope.BaseUri);
                    }

                    if (false)
                    {
                        await context.DoWithRetriesUntilNavigationHasOccurredOrFailAsync(
                            () => context.Driver.Navigate().GoToUrlAsync(new Uri(context.Scope.BaseUri, "blog/post-1")));

                        await context.DoWithRetriesUntilNavigationHasOccurredOrFailAsync(
                            () => context.Driver.Navigate().GoToUrlAsync(new Uri(context.Scope.BaseUri, "about")));

                        await context.DoWithRetriesUntilNavigationHasOccurredOrFailAsync(
                            () => context.Driver.Navigate().GoToUrlAsync(context.Scope.BaseUri));
                    }

                    if (false)
                    {
                        await context.ExecuteLoggedAsync(
                            "GoToUrlAsync",
                            "blog/post-1",
                            async () =>
                            {
                                await context.DoWithRetriesUntilNavigationHasOccurredOrFailAsync(
                                    () => context.Driver.Navigate().GoToUrlAsync(new Uri(context.Scope.BaseUri, "blog/post-1")));
                            });

                        await context.ExecuteLoggedAsync(
                            "GoToUrlAsync",
                            "about",
                            async () =>
                            {
                                await context.DoWithRetriesUntilNavigationHasOccurredOrFailAsync(
                                    () => context.Driver.Navigate().GoToUrlAsync(new Uri(context.Scope.BaseUri, "about")));
                            });

                        await context.ExecuteLoggedAsync(
                            "GoToUrlAsync",
                            "/",
                            async () =>
                            {
                                await context.DoWithRetriesUntilNavigationHasOccurredOrFailAsync(
                                    () => context.Driver.Navigate().GoToUrlAsync(context.Scope.BaseUri));
                            });
                    }

                    if (false)
                    {
                        await context.ExecuteLoggedAsync(
                            "GoToUrlAsync",
                            "blog/post-1",
                            async () =>
                            {
                                await context.Configuration.Events.BeforeNavigation
                                    .InvokeAsync<NavigationEventHandler>(eventHandler => eventHandler(context, new Uri(context.Scope.BaseUri, "blog/post-1")));

                                await context.DoWithRetriesUntilNavigationHasOccurredOrFailAsync(
                                    () => context.Driver.Navigate().GoToUrlAsync(new Uri(context.Scope.BaseUri, "blog/post-1")));

                                await context.Configuration.Events.AfterNavigation
                                    .InvokeAsync<NavigationEventHandler>(eventHandler => eventHandler(context, new Uri(context.Scope.BaseUri, "blog/post-1")));
                            });

                        await context.ExecuteLoggedAsync(
                            "GoToUrlAsync",
                            "about",
                            async () =>
                            {
                                await context.Configuration.Events.BeforeNavigation
                                    .InvokeAsync<NavigationEventHandler>(eventHandler => eventHandler(context, new Uri(context.Scope.BaseUri, "about")));

                                await context.DoWithRetriesUntilNavigationHasOccurredOrFailAsync(
                                    () => context.Driver.Navigate().GoToUrlAsync(new Uri(context.Scope.BaseUri, "about")));

                                await context.Configuration.Events.AfterNavigation
                                    .InvokeAsync<NavigationEventHandler>(eventHandler => eventHandler(context, new Uri(context.Scope.BaseUri, "about")));
                            });

                        await context.ExecuteLoggedAsync(
                            "GoToUrlAsync",
                            "/",
                            async () =>
                            {
                                await context.Configuration.Events.BeforeNavigation
                                    .InvokeAsync<NavigationEventHandler>(eventHandler => eventHandler(context, context.Scope.BaseUri));

                                await context.DoWithRetriesUntilNavigationHasOccurredOrFailAsync(
                                    () => context.Driver.Navigate().GoToUrlAsync(context.Scope.BaseUri));

                                await context.Configuration.Events.AfterNavigation
                                    .InvokeAsync<NavigationEventHandler>(eventHandler => eventHandler(context, context.Scope.BaseUri));
                            });
                    }

                    if (true)
                    {
                        await context.GoToRelativeUrlAsync("/blog/post-1");
                        await context.GoToRelativeUrlAsync("/about");
                        await context.GoToHomePageAsync();
                    }

                    _testOutputHelper.WriteLine($"Iteration {i + 1} completed.");
                }
            },
            configuration =>
            {
                configuration.AccessibilityCheckingConfiguration.RunAccessibilityCheckingAssertionOnAllPageChanges = false;
                configuration.HtmlValidationConfiguration.RunHtmlValidationAssertionOnAllPageChanges = false;
            });
}
