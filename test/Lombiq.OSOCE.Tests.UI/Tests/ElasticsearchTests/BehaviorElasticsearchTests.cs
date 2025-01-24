using Atata;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Helpers;
using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium;
using OrchardCore.Search.Elasticsearch.Core.Services;
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
    public Task ElasticsearchShouldWork() =>
        ExecuteTestAsync(
            async context =>
            {
                await context.GoToSetupPageAndSetupOrchardCoreAsync("Lombiq.OSOCE.Tests.Elasticsearch");

                try
                {
                    await context.SignInDirectlyAndGoToRelativeUrlAsync("/search");

                    await context.ClickAndFillInWithRetriesAsync(By.Name("Terms"), "man");
                    await context.ClickReliablyOnAsync(By.XPath("//button[@class='btn btn-primary btn-sm']"));

                    context.Exists(By.XPath("//h2[contains(., 'Man must explore, and this is exploration at its greatest')]"));
                }
                finally
                {
                    await context.Application.UsingScopeAsync(async shellScope =>
                    {
                        var elasticsearchIndexManager = shellScope.ServiceProvider.GetRequiredService<ElasticIndexManager>();

                        if (!await elasticsearchIndexManager.ExistsAsync("elasticsearchshouldwork")) return; // #spell-check-ignore-line

                        await elasticsearchIndexManager.DeleteIndex("elasticsearchshouldwork"); // #spell-check-ignore-line
                    });
                }
            },
            changeConfigurationAsync: ConfigurationHelper.DisableHtmlValidation);
}
