using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Pages;
using Lombiq.Walkthroughs.Tests.UI.Extensions;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

public class BehaviorWalkthroughsTests : UITestBase
{
    public BehaviorWalkthroughsTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task WalkthroughsShouldWorkCorrectly() =>
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
            // Could be removed if https://github.com/shepherd-pro/shepherd/issues/2555 is fixed.
            // Or could be made simpler if this is fixed https://github.com/atata-framework/atata-htmlvalidation/issues/8.
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
