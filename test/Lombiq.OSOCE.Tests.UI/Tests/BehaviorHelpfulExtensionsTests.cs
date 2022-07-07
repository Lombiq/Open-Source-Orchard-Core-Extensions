using Lombiq.HelpfulExtensions.Tests.UI.Extensions;
using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Services;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests;

public class BehaviorHelpfulExtensionsTests : UITestBase
{
    public BehaviorHelpfulExtensionsTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Theory, Chrome]
    public Task FlowAdditionalStylingPartShouldActivateCorrectly(Browser browser) =>
        ExecuteTestAfterSetupAsync(
            context => context.TestFlowAdditionalStylingPartNotActivatingGh76Async(),
            browser);
}
