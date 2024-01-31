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
