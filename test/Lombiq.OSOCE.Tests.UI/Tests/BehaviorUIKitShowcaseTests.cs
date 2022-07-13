using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Services;
using Lombiq.UIKit.Tests.UI.Extensions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests;

public class BehaviorUIKitShowcaseTests : UITestBase
{
    public BehaviorUIKitShowcaseTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Theory, Chrome]
    public Task UIKitShowcasePageShouldBeCorrect(Browser browser)
        => ExecuteTestAfterSetupAsync(
            context => context.TestUIKitShowcaseBehaviorAsync(),
            browser);
}
