using Lombiq.Hosting.Tenants.MediaStorageManagement.Tests.UI.Extensions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

public class BehaviorMediaStorageManagementTests : UITestBase
{
    public BehaviorMediaStorageManagementTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task MediaQuotaShouldWork() =>
        ExecuteTestAfterSetupAsync(
            context => context.TestMediaStorageManagementBehaviorAsync(),
            // Setting maximum storage quota for 50 000 bytes to see if it fails with the sample png file.
            configuration => configuration.SetMediaStorageManagementOptionsForUITest(50_000));
}
