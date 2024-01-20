using Lombiq.Tests.UI.Tests.UI.TestCases;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.UITestingToolboxTests;

public class CustomAdminPrefixTests(ITestOutputHelper testOutputHelper) : UITestBase(testOutputHelper)
{
    [Fact]
    public Task NavigationWithCustomAdminPrefixShouldWork() =>
        CustomAdminPrefixTestCases.NavigationWithCustomAdminPrefixShouldWorkAsync(ExecuteTestAfterSetupAsync);
}
