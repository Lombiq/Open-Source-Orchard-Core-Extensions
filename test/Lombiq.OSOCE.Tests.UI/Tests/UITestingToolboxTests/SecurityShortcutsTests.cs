using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Services;
using Lombiq.Tests.UI.Tests.UI.TestCases;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.UITestingToolboxTests;

public class SecurityShortcutsTests : UITestBase
{
    public SecurityShortcutsTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Theory, Chrome]
    public Task AddUserToRoleShouldWork(Browser browser) =>
        SecurityShortcutsTestCases.AddUserToRoleShouldWorkAsync(ExecuteTestAfterSetupAsync, browser);

    [Theory, Chrome]
    public Task AddUserToFakeRoleShouldThrow(Browser browser) =>
        SecurityShortcutsTestCases.AddUserToFakeRoleShouldThrowAsync(ExecuteTestAfterSetupAsync, browser);

    [Theory, Chrome]
    public Task AllowFakePermissionToRoleShouldThrow(Browser browser) =>
        SecurityShortcutsTestCases.AllowFakePermissionToRoleShouldThrowAsync(ExecuteTestAfterSetupAsync, browser);
}
