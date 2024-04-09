using Lombiq.JsonEditor.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

public class BehaviorJsonEditorTests : UITestBase
{
    public BehaviorJsonEditorTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task JsonEditorShouldWorkCorrectly() =>
        ExecuteTestAfterSetupAsync(
            context =>
            context.TestJsonEditorBehaviorAsync(),
            configuration => configuration.AssertBrowserLog = logEntries =>
            {
                // This is to not fail on a browser error caused by jQuery missing. Can be removed after this issue is
                // resolved and released: https://github.com/OrchardCMS/OrchardCore/issues/15181.
                var messageWithoutJqueryError = logEntries.Where(logEntry =>
                    !logEntry.Message.ContainsOrdinalIgnoreCase(
                        "Uncaught ReferenceError: $ is not defined"));

                OrchardCoreUITestExecutorConfiguration.AssertBrowserLogIsEmpty(messageWithoutJqueryError);
            });
}
