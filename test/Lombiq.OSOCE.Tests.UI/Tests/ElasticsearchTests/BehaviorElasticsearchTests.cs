using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Samples.Helpers;
using Lombiq.Tests.UI.Services;
using OpenQA.Selenium;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

public class BehaviorElasticsearchTests : UITestBase
{
    public BehaviorElasticsearchTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    protected override Task ExecuteTestAfterSetupAsync(
        Func<UITestContext, Task> testAsync,
        Browser browser,
        Func<OrchardCoreUITestExecutorConfiguration, Task> changeConfigurationAsync) =>
        ExecuteTestAsync(testAsync, browser, SetupHelpers.RunElasticsearchSetupAsync, configuration =>
        {
            configuration.UseElasticsearch = true;
            configuration.HtmlValidationConfiguration.RunHtmlValidationAssertionOnAllPageChanges = false;
            return changeConfigurationAsync(configuration);
        });

    [Fact]
    public Task ElasticsearchShouldWork() =>
        ExecuteTestAfterSetupAsync(
            async context =>
            {
                await context.SignInDirectlyAndGoToRelativeUrlAsync("/search");

                await context.ClickAndFillInWithRetriesAsync(By.Name("Terms"), "man");
                await context.ClickReliablyOnAsync(By.XPath("//button[@class='btn btn-primary btn-sm']"));

                context.Exists(By.XPath("//h2[contains(., 'Man must explore, and this is exploration at its greatest')]"));
            });
}
