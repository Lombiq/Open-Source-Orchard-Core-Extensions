using Lombiq.Tests.UI.Tests.UI.TestCases;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.UITestingToolboxTests;

public class WorkflowShortcutsTests(ITestOutputHelper testOutputHelper) : UITestBase(testOutputHelper)
{
    [Fact]
    public Task GenerateHttpEventUrlShouldWork() =>
        WorkflowShortcutsTestCases.GenerateHttpEventUrlShouldWorkAsync(ExecuteTestAfterSetupAsync);
}
