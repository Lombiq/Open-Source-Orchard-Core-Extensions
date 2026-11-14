using Lombiq.OrchardCoreApiClient.Tests.UI.Extensions;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.Tests.UI.Tests.LibraryTests;

public class BehaviorOrchardCoreApiClientTests : UITestBase
{
    public BehaviorOrchardCoreApiClientTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task OrchardCoreApiClientShouldWork() =>
        AddTest(context => context.TestOrchardCoreApiClientBehaviorAsync())
            .AddSyncConfiguration(configuration =>
            {
                // Workaround for long paths in Windows.
                if (OperatingSystem.IsWindows())
                {
                    configuration.TempDirectoryPath = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                        "oc-api");
                }
            })
            .RunAsync();
}
