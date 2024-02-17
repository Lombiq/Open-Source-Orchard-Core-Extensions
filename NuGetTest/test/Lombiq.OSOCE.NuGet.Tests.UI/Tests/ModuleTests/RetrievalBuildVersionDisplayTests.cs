using Lombiq.Hosting.BuildVersionDisplay.Tests.UI.Extensions;
using Lombiq.OSOCE.NuGet.Tests.UI.Helpers;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.NuGet.Tests.UI.Tests.ModuleTests;

public class RetrievalBuildVersionDisplayTests : UITestBase
{
    public RetrievalBuildVersionDisplayTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task BuildVersionShouldBeBeDisplayedCorrectly() =>
        ExecuteTestAfterSetupAsync(
            context => context.TestBuildVersionDisplayAsync(),
            // Can be removed once  https://github.com/OrchardCMS/OrchardCore/issues/15222 is done.
            changeConfiguration =>
            {
                changeConfiguration.AssertBrowserLog = AssertHtmlAndBrowserErrorsHelper.AssertBrowserLogIsEmpty;
                changeConfiguration.HtmlValidationConfiguration.AssertHtmlValidationResultAsync =
                    AssertHtmlAndBrowserErrorsHelper.AssertHtmlErrorsAreEmpty;
            });
}
