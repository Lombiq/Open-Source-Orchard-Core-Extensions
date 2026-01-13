using Lombiq.Walkthroughs.Tests.UI.Extensions;
using OpenQA.Selenium.BiDi.Log;
using Shouldly;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

[SuppressMessage("Usage", "xUnit1004:Test methods should not be skipped", Justification = "Awaiting new OC preview version.")]
public class BehaviorWalkthroughsTests : UITestBase
{
    public BehaviorWalkthroughsTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact(Skip = "Blocked by https://github.com/OrchardCMS/OrchardCore/pull/18699.")]
    public Task WalkthroughsShouldWorkCorrectly() =>
        ExecuteTestAsync(
            context => context.RunSetupAndTestWalkthroughsBehaviorAsync(),
            changeConfiguration: configuration =>
            {
                configuration
                    .HtmlValidationConfiguration
                    .WithRelativeConfigPath("NoUniqueLandmark.htmlvalidate.json");

                // There are some false positives of this error, because of page navigation.
                configuration.AssertBrowserLog = logEntries => logEntries
                    .Where(entry =>
                        entry.Level > Level.Info &&
                        entry.Text?.Contains("The element for this Shepherd step was not found") != true)
                    .ShouldBeEmpty();
            });
}
