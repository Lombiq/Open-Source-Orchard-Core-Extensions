using Lombiq.Tests.UI.BasicOrchardFeaturesTesting;
using Lombiq.Tests.UI.Pages;
using System.Threading.Tasks;
using Xunit;

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
            context => context.TestBasicOrchardFeaturesAsync(
                new OrchardCoreSetupParameters(context, "Lombiq.OSOCE.NuGet.BasicOrchardFeaturesTests")
                {
                    SkipRegistration = true,
                }));
}
