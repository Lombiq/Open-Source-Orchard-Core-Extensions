using Lombiq.LoginAsAnybody.Tests.UI.Extensions;
using Lombiq.OSOCE.NuGet.Tests.UI.Helpers;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.NuGet.Tests.UI.Tests.ModuleTests;

public class BehaviorLoginAsAnybodyTests : UITestBase
{
    public BehaviorLoginAsAnybodyTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task SwitchingUserShouldWorkCorrectly() =>
        ExecuteTestAfterSetupAsync(
            context => context.TestLoginAsAnybodyAsync(),
            // Can be removed once  https://github.com/OrchardCMS/OrchardCore/issues/15222 is done.
            changeConfiguration => changeConfiguration.AssertBrowserLog = AssertBrowserLogHelpers.AssertBrowserLogIsEmpty);

    [Fact]
    public Task PermissionCheckShouldWorkCorrectly() =>
        ExecuteTestAfterSetupAsync(
            context => context.TestLoginAsAnybodyAuthorizationAsync(),
            changeConfiguration: Configurations.IgnoreUnauthorizedBrowserLogEntries);
}
