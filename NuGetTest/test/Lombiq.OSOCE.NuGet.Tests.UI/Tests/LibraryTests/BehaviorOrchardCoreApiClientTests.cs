using Lombiq.OrchardCoreApiClient.Tests.UI.Extensions;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.NuGet.Tests.UI.Tests.LibraryTests;

public class BehaviorOrchardCoreApiClientTests : UITestBase
{
    public BehaviorOrchardCoreApiClientTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task OrchardCoreApiClientShouldWork() =>
        ExecuteTestAfterSetupAsync(
            context => context.TestOrchardCoreApiClientBehaviorAsync(),
            configuration =>
            {
                // Workaround for long paths in Windows.
                if (OperatingSystem.IsWindows())
                {
                    configuration.TempDirectoryPath = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                        "oc-api");
                }
            });
}
