using Lombiq.Walkthroughs.Tests.UI.Extensions;
using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Services;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Pages;
using System;
using Shouldly;
using Lombiq.Tests.UI.Services;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

public class BehaviorWalkthroughsTests : UITestBase
{
    public BehaviorWalkthroughsTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Theory, Chrome]
    public Task WalkthroughsShouldWorkCorrectly(Browser browser) =>
        ExecuteTestAsync(
            async context =>
            {
                await context.GoToSetupPageAndSetupOrchardCoreAsync(
                    new OrchardCoreSetupParameters(context)
                    {
                        SiteName = "Lombiq's OSOCE - UI Testing",
                        RecipeId = "Lombiq.Walkthroughs",
                        TablePrefix = "OSOCE",
                        SiteTimeZoneValue = "Europe/Budapest",
                    });

                await context.TestWalkthroughsBehaviorAsync();
            },
            browser,
            // Could be removed if https://github.com/shepherd-pro/shepherd/issues/2555 is fixed.
            changeConfiguration: configuration => configuration.HtmlValidationConfiguration.AssertHtmlValidationResultAsync =
                validationResult =>
                {
                    var validationResultOutput = validationResult.Output;

                    if (!validationResultOutput.Contains(
                        "1 problem (1 error, 0 warnings)",
                        StringComparison.InvariantCultureIgnoreCase) ||
                    !validationResultOutput.Contains(
                        "error  <button> is missing required \"type\" attribute  element-required-attributes",
                        StringComparison.InvariantCultureIgnoreCase))
                    {
                        validationResult.Output.ShouldBeEmpty();
                    }

                    return Task.CompletedTask;
                });
}
