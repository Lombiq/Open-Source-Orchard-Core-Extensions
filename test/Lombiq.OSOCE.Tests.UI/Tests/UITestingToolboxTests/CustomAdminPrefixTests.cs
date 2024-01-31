using Lombiq.Tests.UI.Tests.UI.TestCases;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.UITestingToolboxTests;

public class CustomAdminPrefixTests : UITestBase
{
    public CustomAdminPrefixTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task NavigationWithCustomAdminPrefixShouldWork() =>
        CustomAdminPrefixTestCases.NavigationWithCustomAdminPrefixShouldWorkAsync(ExecuteTestAfterSetupAsync);
}
