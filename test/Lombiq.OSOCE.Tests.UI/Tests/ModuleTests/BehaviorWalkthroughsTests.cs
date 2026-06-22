using Lombiq.Tests.UI.Extensions;
using Lombiq.Walkthroughs.Tests.UI.Extensions;
using System.Net;
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
                    .WithRelativeConfigPath("WalkthroughsShouldWorkCorrectly.htmlvalidate.json");

                // The preview page sends these requests prematurely, which fails validation and perhaps incorrectly
                // returns an error response. See https://github.com/OrchardCMS/OrchardCore/issues/19440 for details.
                configuration.WithIgnoreExpectedStatusResponseFilter("/Preview/Draft", HttpStatusCode.InternalServerError);
            });
}
