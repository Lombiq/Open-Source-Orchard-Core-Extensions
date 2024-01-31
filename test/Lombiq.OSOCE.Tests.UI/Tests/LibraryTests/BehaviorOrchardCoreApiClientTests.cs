using Lombiq.OrchardCoreApiClient.Tests.UI.Extensions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.LibraryTests;

public class BehaviorOrchardCoreApiClientTests : UITestBase
{
    public BehaviorOrchardCoreApiClientTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task OrchardCoreApiClientShouldWork() =>
        ExecuteTestAfterSetupAsync(context => context.TestOrchardCoreApiClientBehaviorAsync());
}
