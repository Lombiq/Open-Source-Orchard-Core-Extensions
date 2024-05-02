using Lombiq.Tests.UI.Tests.UI.TestCases;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.UITestingToolboxTests;

public class TimeoutTests : UITestBase
{
    public TimeoutTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task TestRunTimeoutShouldThrowAsync() =>
        TimeoutTestCases.TestRunTimeoutShouldThrowAsync(ExecuteTestAfterSetupAsync);
}
