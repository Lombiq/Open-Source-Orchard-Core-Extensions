using Lombiq.JsonEditor.Tests.UI.Extensions;
using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Services;
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

    [Theory, Chrome]
    public Task JsonEditorShouldWorkCorrectly(Browser browser) =>
        ExecuteTestAfterSetupAsync(
            async context => await context.TestJsonEditorBehaviorAsync(),
            browser);
}
