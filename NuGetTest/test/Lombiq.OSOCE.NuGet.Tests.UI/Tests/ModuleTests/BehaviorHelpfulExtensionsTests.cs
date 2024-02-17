using Lombiq.HelpfulExtensions.Tests.UI.Extensions;
using Lombiq.OSOCE.NuGet.Tests.UI.Helpers;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.NuGet.Tests.UI.Tests.ModuleTests;

public class BehaviorHelpfulExtensionsTests : UITestBase
{
    public BehaviorHelpfulExtensionsTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task FeatureFlow() =>
        ExecuteTestAfterSetupAsync(
            context => context.TestFlowAdditionalStylingPartAsync(),
            // Can be removed once  https://github.com/OrchardCMS/OrchardCore/issues/15222 is done.
            changeConfiguration =>
            {
                changeConfiguration.AssertBrowserLog = AssertHtmlAndBrowserErrorsHelper.AssertBrowserLogIsEmpty;
                changeConfiguration.HtmlValidationConfiguration.AssertHtmlValidationResultAsync =
                    AssertHtmlAndBrowserErrorsHelper.AssertHtmlErrorsAreEmpty;
            });

    [Fact]
    public Task FeatureWidgets() =>
        ExecuteTestAfterSetupAsync(context => context.TestFeatureWidgetsAsync());

    [Fact]
    public Task FeatureCodeGeneration() =>
        ExecuteTestAfterSetupAsync(context => context.TestFeatureCodeGenerationsAsync());
}
