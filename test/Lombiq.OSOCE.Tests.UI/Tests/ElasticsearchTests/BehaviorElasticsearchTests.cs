using Atata;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Helpers;
using Lombiq.Tests.UI.Pages;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;
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
        ExecuteTestAsync(
            async context =>
            {
                await context.SignInDirectlyAndGoToRelativeUrlAsync("/search");

                await context.ClickAndFillInWithRetriesAsync(By.Name("Terms"), "man");
                await context.ClickReliablyOnAsync(By.XPath("//button[@class='btn btn-primary btn-sm']"));

                context.Exists(By.XPath("//h2[contains(., 'Man must explore, and this is exploration at its greatest')]"));
            },
            setupOperation: async context =>
            {
                var homepageUri = await context.GoToSetupPageAndSetupOrchardCoreAsync(
                    new OrchardCoreSetupParameters(context)
                    {
                        SiteName = "Lombiq's OSOCE - UI Testing - Elasticsearch",
                        RecipeId = "Lombiq.OSOCE.Tests.Elasticsearch",
                        TablePrefix = "OSOCE",
                        SiteTimeZoneValue = "Europe/Budapest",
                    });

                try
                {
                    context.Exists(By.Id("navbar"));
                }
                catch (NoSuchElementException)
                {
                    var validationErrors = context.GetAll(By.ClassName("field-validation-error"));

                    if (validationErrors.Count == 0) throw;

                    var errors = "\n- " + validationErrors.Select(element => element.Text.Trim()).Join("\n- ");
                    throw new AssertionException($"Setup has failed with the following validation errors:{errors}");
                }

                return homepageUri;
            },
            changeConfigurationAsync: ConfigurationHelper.DisableHtmlValidation);
}
