using Lombiq.OSOCE.Tests.UI;
using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

public class PrefixTests : UITestBase
{
    public PrefixTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Theory, Chrome]
    public Task TestTest(Browser browser) =>
        ExecuteTestAfterSetupAsync(
            async context =>
            {
                context.AdminUrlPrefix = "event-admin";

                await context.SignInDirectlyAsync();
                await context.GoToFeaturesPageAsync();
                await context.GoToContentItemListAsync();
            },
            browser);
}
