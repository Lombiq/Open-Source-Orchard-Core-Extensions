using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using Lombiq.VueJs.Tests.UI.Extensions;
using OpenQA.Selenium;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests
{
    public class BehaviorVueTests : UITestBase
    {
        public BehaviorVueTests(ITestOutputHelper testOutputHelper)
            : base(testOutputHelper)
        {
        }

        [Theory, Chrome]
        public Task RecipeDataShouldBeDisplayedCorrectly(Browser browser) =>
            ExecuteTestAfterSetupAsync(
                async context =>
                {
                    // We want to click off the modal from Lombiq.Privacy immediately because it overlaps the Next
                    // button while testing the enhanced list. This requirement is specific to this solution, which is
                    // why it's not included in the extension method.
                    await context.SignInDirectlyAndGoToDashboardAsync();
                    await context.ClickReliablyOnAsync(By.Id("privacy-consent-accept-button"));
                    context.Refresh();

                    await context.TestVueSampleBehaviorAsync();
                },
                browser);
    }
}
