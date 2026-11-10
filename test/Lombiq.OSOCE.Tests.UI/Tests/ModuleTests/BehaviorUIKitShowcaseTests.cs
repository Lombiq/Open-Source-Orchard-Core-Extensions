using Lombiq.Tests.UI.Extensions;
using Lombiq.UIKit.Tests.UI.Extensions;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

public class BehaviorUIKitShowcaseTests : UITestBase
{
    public BehaviorUIKitShowcaseTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper) =>
        AddSyncConfiguration(configuration => configuration.HtmlValidationConfiguration
            .WithRelativeConfigPath("NoUniqueLandmark.htmlvalidate.json")
            .WithOC15222Filter());

#pragma warning disable xUnit1004
    [Fact(Skip = "Temp")]
#pragma warning restore xUnit1004
    public Task UIKitShowcasePageShouldBeCorrect() =>
        RunAsync(async context =>
        {
            await context.SignInDirectlyAndGoToDashboardAsync();
            await context.TestUIKitShowcaseBehaviorAsync();
        });
}
