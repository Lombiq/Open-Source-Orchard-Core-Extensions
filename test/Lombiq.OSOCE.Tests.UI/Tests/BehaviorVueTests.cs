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
                    await context.SignInDirectlyAndGoToDashboardAsync();
                    context.ClickReliablyOn(By.Id("privacy-consent-accept-button"));
                    context.Refresh();

                    await context.TestVueSampleBehaviorAsync();
                },
                browser);
    }
}
