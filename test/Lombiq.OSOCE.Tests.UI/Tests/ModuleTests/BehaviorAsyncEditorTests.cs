using Lombiq.ContentEditors.Tests.UI.Extensions;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Helpers;
using System.Text.Json;
using System.Text.Json.Serialization;
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

    [Fact]
    public Task DemoContentItemAsyncEditorShouldWork() =>
        ExecuteTestAfterSetupAsync(
            async context =>
            {
                await context.SignInDirectlyAsync();
                await context.TestDemoContentItemAsyncEditorAsync();
            });

    [Fact]
    public Task DemoFrontEndAsyncEditorShouldWork() =>
        ExecuteTestAfterSetupAsync(
            async context =>
            {
                await context.SignInDirectlyAsync();
                await context.TestDemoFrontEndAsyncEditorAsync();
            },
            ConfigurationHelper.DisableHtmlValidation);
}
