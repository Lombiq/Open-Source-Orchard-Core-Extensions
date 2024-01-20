using Lombiq.Tests.UI.Tests.UI.TestCases;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.UITestingToolboxTests;

public class SecurityShortcutsTests(ITestOutputHelper testOutputHelper) : UITestBase(testOutputHelper)
{
    [Fact]
    public Task AddUserToRoleShouldWork() =>
        SecurityShortcutsTestCases.AddUserToRoleShouldWorkAsync(ExecuteTestAfterSetupAsync);

    [Fact]
    public Task AddUserToFakeRoleShouldThrow() =>
        SecurityShortcutsTestCases.AddUserToFakeRoleShouldThrowAsync(ExecuteTestAfterSetupAsync);

    [Fact]
    public Task AllowFakePermissionToRoleShouldThrow() =>
        SecurityShortcutsTestCases.AllowFakePermissionToRoleShouldThrowAsync(ExecuteTestAfterSetupAsync);
}
