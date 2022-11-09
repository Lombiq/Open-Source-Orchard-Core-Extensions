using Lombiq.ContentEditors.Tests.UI.Extensions;
using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

public class BasicAsyncEditorTests : UITestBase
{
    public BasicAsyncEditorTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Theory, Chrome]
    public Task DemoAsyncEditorShouldLoad(Browser browser) =>
        ExecuteTestAfterSetupAsync(
            async context =>
            {
                await context.SignInDirectlyAsync();
                await context.EnableContentEditorsSamplesFeatureAsync();
                await context.TestDemoAsyncEditorLoadOnAdminAsync();
            },
            browser);
}
