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
    public Task CodeGenerationFeatureShouldWork() => ExecuteTestAfterSetupAsync(context => context.TestCodeGenerationFeatureAsync());

    [Fact]
    public Task ContentSetsFeatureShouldWork() => ExecuteTestAfterSetupAsync(context => context.TestContentSetsFeatureAsync());

    [Fact]
    public Task WidgetsFeatureShouldWork() => ExecuteTestAfterSetupAsync(context => context.TestWidgetsFeatureAsync());

    [Fact]
    public Task FlowFeatureShouldWork() => ExecuteTestAfterSetupAsync(context => context.TestFlowsFeatureAsync());
}
