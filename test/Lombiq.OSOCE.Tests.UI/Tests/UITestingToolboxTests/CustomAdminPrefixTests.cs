using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Services;
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

    [Theory, Chrome]
    public Task NavigationWithCustomAdminPrefixShouldWork(Browser browser) =>
        CustomAdminPrefixTestCases.NavigationWithCustomAdminPrefixShouldWorkAsync(ExecuteTestAfterSetupAsync, browser);
}
