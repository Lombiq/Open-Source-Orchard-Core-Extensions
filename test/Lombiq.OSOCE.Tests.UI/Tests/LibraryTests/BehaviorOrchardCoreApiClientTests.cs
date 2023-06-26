using Lombiq.OrchardCoreApiClient.Tests.UI.Extensions;
using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Services;
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

    [Theory, Chrome]
    public Task OrchardCoreApiClientShouldWork(Browser browser) =>
        ExecuteTestAfterSetupAsync(
            async context => await context.TestOrchardCoreApiClientBehaviorAsync(),
            browser);
}
