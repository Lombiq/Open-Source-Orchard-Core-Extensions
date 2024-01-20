using Lombiq.JsonEditor.Tests.UI.Extensions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

public class BehaviorJsonEditorTests(ITestOutputHelper testOutputHelper) : UITestBase(testOutputHelper)
{
    [Fact]
    public Task JsonEditorShouldWorkCorrectly() =>
        ExecuteTestAfterSetupAsync(context => context.TestJsonEditorBehaviorAsync());
}
