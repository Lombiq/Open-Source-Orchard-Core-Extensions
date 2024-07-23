using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Helpers;
using OpenQA.Selenium;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

public class BehaviorElasticsearchTests : UITestBase
{
    public BehaviorElasticsearchTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task ElasticsearchSearchingShouldWork() =>
        ExecuteTestAfterSetupAsync(
            async context =>
            {
                await context.SignInDirectlyAsync();
                await context.GoToRelativeUrlAsync("/search");

                await context.ClickAndFillInWithRetriesAsync(By.Name("Terms"), "man");
                await context.ClickReliablyOnAsync(By.XPath("//button[@class='btn btn-primary btn-sm']"));

                await context.SwitchToInteractiveAsync();
                context.Exists(By.XPath("//h2[contains(., 'Man must explore, and this is exploration at its greatest')]"));
            },
            ConfigurationHelper.DisableHtmlValidation);
}
