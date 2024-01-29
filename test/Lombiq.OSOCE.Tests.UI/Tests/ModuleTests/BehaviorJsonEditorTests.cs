using Lombiq.JsonEditor.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

public class BehaviorJsonEditorTests(ITestOutputHelper testOutputHelper) : UITestBase(testOutputHelper)
{
    [Fact]
    public Task JsonEditorShouldWorkCorrectly() =>
        ExecuteTestAfterSetupAsync(
            context =>
            context.TestJsonEditorBehaviorAsync(),
            configuration => configuration.AssertBrowserLog = logEntries =>
            {
                // This is needed to avoid missing jQuery
                // Can be removed after this issue is resolved and deployed https://github.com/OrchardCMS/OrchardCore/issues/15181
                var messageWithoutJqueryError = logEntries.Where(logEntry =>
                !logEntry.Message.ContainsOrdinalIgnoreCase(
                    "Uncaught ReferenceError: $ is not defined"));

                OrchardCoreUITestExecutorConfiguration.AssertBrowserLogIsEmpty(messageWithoutJqueryError);
            });
}
