using Lombiq.HelpfulExtensions.Tests.UI.Extensions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

public class BehaviorHelpfulExtensionsTests : UITestBase
{
    public BehaviorHelpfulExtensionsTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task FeatureFlow() => ExecuteTestAfterSetupAsync(context => context.TestFlowAdditionalStylingPartAsync());

    [Fact]
    public Task FeatureWidgets() => ExecuteTestAfterSetupAsync(context => context.TestFeatureWidgetsAsync());

    [Fact]
    public Task FeatureCodeGeneration() => ExecuteTestAfterSetupAsync(context => context.TestFeatureCodeGenerationsAsync());

    [Fact]
    public Task FeatureContentSets() => ExecuteTestAfterSetupAsync(context => context.TestFeatureContentSetsAsync());
}
