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

#pragma warning disable xUnit1004 // Test methods should not be skipped
    [Fact(Skip = "Debugging test hangs.")]
#pragma warning restore xUnit1004 // Test methods should not be skipped
    public Task TestRunTimeoutShouldThrowAsync() =>
        TimeoutTestCases.TestRunTimeoutShouldThrowAsync(ExecuteTestAfterSetupAsync);
}
