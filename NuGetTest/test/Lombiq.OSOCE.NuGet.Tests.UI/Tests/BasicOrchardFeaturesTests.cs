using Lombiq.Tests.UI.BasicOrchardFeaturesTesting;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.NuGet.Tests.UI.Tests;

public class BasicOrchardFeaturesTests : UITestBase
{
    public BasicOrchardFeaturesTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task BasicOrchardFeaturesShouldWork() =>
        ExecuteTestAsync(
            context => context.TestBasicOrchardFeaturesExceptRegistrationAsync("Lombiq.OSOCE.NuGet.BasicOrchardFeaturesTests"));
}
