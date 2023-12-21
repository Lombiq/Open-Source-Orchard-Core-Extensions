using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Pages;
using Lombiq.Walkthroughs.Tests.UI.Extensions;
using System;
using System.IO;
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
            changeConfiguration: configuration => configuration.HtmlValidationConfiguration.HtmlValidationOptions =
                configuration.HtmlValidationConfiguration.HtmlValidationOptions
                    .CloneWith(validationOptions => validationOptions.ConfigPath =
                        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BehaviorWalkthroughsTests.htmlvalidate.json")));
}
