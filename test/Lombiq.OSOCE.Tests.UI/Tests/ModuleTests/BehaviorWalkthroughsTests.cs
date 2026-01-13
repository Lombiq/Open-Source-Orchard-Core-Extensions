using Lombiq.Walkthroughs.Tests.UI.Extensions;
using OpenQA.Selenium.BiDi.Log;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

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
            context => context.RunSetupAndTestWalkthroughsBehaviorAsync(),
            changeConfiguration: configuration =>
            {
                configuration
                    .HtmlValidationConfiguration
                    .WithRelativeConfigPath("NoUniqueLandmark.htmlvalidate.json");

                // There are some false positives of this error, because of page navigation.
                configuration.AssertBrowserLog = logEntries =>
                    logEntries.ShouldNotContain(entry =>
                        entry.Level > Level.Info &&
                        !entry.Text.Contains("The element for this Shepherd step was not found"));
            });
}
