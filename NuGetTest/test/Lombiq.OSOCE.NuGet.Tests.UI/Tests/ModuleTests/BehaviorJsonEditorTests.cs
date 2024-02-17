using Lombiq.JsonEditor.Tests.UI.Extensions;
using Lombiq.OSOCE.NuGet.Tests.UI.Helpers;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.NuGet.Tests.UI.Tests.ModuleTests;

public class BehaviorJsonEditorTests : UITestBase
{
    public BehaviorJsonEditorTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task JsonEditorShouldWorkCorrectly() =>
        ExecuteTestAfterSetupAsync(
            context => context.TestJsonEditorBehaviorAsync(),
            // Can be removed once  https://github.com/OrchardCMS/OrchardCore/issues/15222 is done.
            changeConfiguration =>
            {
                changeConfiguration.AssertBrowserLog = AssertHtmlAndBrowserErrorsHelper.AssertBrowserLogIsEmpty;
                changeConfiguration.HtmlValidationConfiguration.AssertHtmlValidationResultAsync =
                    AssertHtmlAndBrowserErrorsHelper.AssertHtmlErrorsAreEmpty;
            });
}
