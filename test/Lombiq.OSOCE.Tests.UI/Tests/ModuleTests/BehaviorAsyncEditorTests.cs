using Lombiq.ContentEditors.Tests.UI.Extensions;
using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Helpers;
using Lombiq.Tests.UI.Services;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

public class BehaviorAsyncEditorTests : UITestBase
{
    public BehaviorAsyncEditorTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Theory, Chrome]
    public Task DemoContentItemAsyncEditorShouldWork(Browser browser) =>
        ExecuteTestAfterSetupAsync(
            async context =>
            {
                await context.SignInDirectlyAsync();
                await context.TestDemoContentItemAsyncEditorAsync();
            },
            browser);

    [Theory, Chrome]
    public Task DemoFrontEndAsyncEditorShouldWork(Browser browser) =>
        ExecuteTestAfterSetupAsync(
            async context =>
            {
                await context.SignInDirectlyAsync();
                await context.TestDemoFrontEndAsyncEditorAsync();
            },
            browser,
            ConfigurationHelper.DisableHtmlValidation);
}
