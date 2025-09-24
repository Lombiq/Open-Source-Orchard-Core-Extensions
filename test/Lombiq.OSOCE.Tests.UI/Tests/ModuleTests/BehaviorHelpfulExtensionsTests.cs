using Lombiq.HelpfulExtensions.Tests.UI.Extensions;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

public class BehaviorHelpfulExtensionsTests : UITestBase
{
    public BehaviorHelpfulExtensionsTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task CodeGenerationFeatureShouldWork() => ExecuteTestAfterSetupAsync(context => context.TestCodeGenerationFeatureAsync());

    [Fact]
    public Task ContentSetsFeatureShouldWork() => ExecuteTestAfterSetupAsync(context => context.TestContentSetsFeatureAsync());

    [Fact]
    public Task FlowsFeatureShouldWork() => ExecuteTestAfterSetupAsync(context => context.TestFlowsFeatureAsync());

    [Fact]
    public Task LiquidFeatureShouldWork() => ExecuteTestAfterSetupAsync(context => context.TestLiquidFeatureAsync());

    [Fact]
    public Task TrumbowygBlogPostsFeatureShouldWork() =>
        ExecuteTestAfterSetupAsync(context => context.TestTrumbowygBlogPostsFeatureAsync());

    [Fact]
    public Task WidgetsFeatureShouldWork() => ExecuteTestAfterSetupAsync(context => context.TestWidgetsFeatureAsync());
}
