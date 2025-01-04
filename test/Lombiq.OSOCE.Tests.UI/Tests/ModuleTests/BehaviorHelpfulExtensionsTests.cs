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
    public Task FeatureFlow() => ExecuteTestAfterSetupAsync(context => context.TestFlowsFeatureAsync());

    [Fact]
    public Task FeatureWidgets() => ExecuteTestAfterSetupAsync(context => context.TestWidgetsFeatureAsync());

    [Fact]
    public Task FeatureCodeGeneration() => ExecuteTestAfterSetupAsync(context => context.TestCodeGenerationFeatureAsync());

    [Fact]
    public Task FeatureContentSets() => ExecuteTestAfterSetupAsync(context => context.TestContentSetsFeatureAsync());
}
