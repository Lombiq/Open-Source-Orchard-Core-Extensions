using Lombiq.OrchardCoreApiClient.Tests.UI.Extensions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.LibraryTests;

public class BehaviorOrchardCoreApiClientTests(ITestOutputHelper testOutputHelper) : UITestBase(testOutputHelper)
{
    [Fact]
    public Task OrchardCoreApiClientShouldWork() =>
        ExecuteTestAfterSetupAsync(context => context.TestOrchardCoreApiClientBehaviorAsync());
}
