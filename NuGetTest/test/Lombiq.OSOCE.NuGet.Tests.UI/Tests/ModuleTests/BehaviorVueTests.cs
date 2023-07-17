using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using Lombiq.VueJs.Tests.UI.Extensions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.NuGet.Tests.UI.Tests.ModuleTests;

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
                await context.SignInDirectlyAsync();
                await context.TestVueSampleBehaviorAsync();
            },
            browser);
}
