using Lombiq.LoginAsAnybody.Tests.UI.Extensions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

public class BehaviorLoginAsAnybodyTests(ITestOutputHelper testOutputHelper) : UITestBase(testOutputHelper)
{
    [Fact]
    public Task SwitchingUserShouldWorkCorrectly() =>
        ExecuteTestAfterSetupAsync(context => context.TestLoginAsAnybodyAsync());

    [Fact]
    public Task PermissionCheckShouldWorkCorrectly() =>
        ExecuteTestAfterSetupAsync(
            context => context.TestLoginAsAnybodyAuthorizationAsync(),
            changeConfiguration: Configurations.IgnoreUnauthorizedBrowserLogEntries);
}
