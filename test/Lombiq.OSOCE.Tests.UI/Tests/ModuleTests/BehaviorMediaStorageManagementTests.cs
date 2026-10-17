using Lombiq.Hosting.Tenants.MediaStorageManagement.Tests.UI.Extensions;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

public class BehaviorMediaStorageManagementTests : UITestBase
{
    private const int LargeFileSizeInMegabytes = 10;
    private const int LargeFileSizeInBytes = LargeFileSizeInMegabytes * 1024 * 1024;

    public BehaviorMediaStorageManagementTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    [SuppressMessage("Style", "IDE0055:Fix formatting", Justification = "Needed for inline comments.")]
    public Task MediaQuotaShouldWork() =>
        ExecuteTestAfterSetupAsync(
            context => context.TestMediaStorageManagementBehaviorAsync(LargeFileSizeInMegabytes),
            // Setting maximum storage quota to see if it fails without the need of very large files.
            configuration => configuration
                .SetMediaStorageManagementOptionsForUITest(LargeFileSizeInBytes));
}
