using Lombiq.Tests.UI.Tests.UI.TestCases;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.UITestingToolboxTests;

public class WorkflowShortcutsTests : UITestBase
{
    public WorkflowShortcutsTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task GenerateHttpEventUrlShouldWork() =>
        WorkflowShortcutsTestCases.GenerateHttpEventUrlShouldWorkAsync(ExecuteTestAfterSetupAsync);
}
