using Lombiq.BaseTheme.Tests.UI.Extensions;
using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests;

public class BehaviorBaseThemeTests : UITestBase
{
    public BehaviorBaseThemeTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Theory, Chrome]
    public Task ThemeFeaturesShouldWork(Browser browser) =>
        ExecuteTestAfterSetupAsync(
            async context =>
            {
                await context.SignInDirectlyAndGoToHomepageAsync();
                await context.TestBaseThemeFeaturesAsync();
            },
            browser);

    [Theory, Chrome]
    public Task IntentionalTestError1(Browser browser) =>
        ExecuteTestAfterSetupAsync(
            _ => throw new InvalidOperationException("I want to fail."),
            browser);

    [Theory, Chrome]
    public Task IntentionalTestError2(Browser browser) =>
        ExecuteTestAfterSetupAsync(
            _ => Throw("Losing is fun."),
            browser);

    [Theory, Chrome]
    public Task IntentionalTestError3(Browser browser) =>
        ExecuteTestAfterSetupAsync(
            _ => Throw(),
            browser);

    private static void Throw(string message) => throw new InvalidOperationException(message);
}
