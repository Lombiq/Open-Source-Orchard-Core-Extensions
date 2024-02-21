using Lombiq.HelpfulExtensions.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

public class BehaviorHelpfulExtensionsTests : UITestBase
{
    public BehaviorHelpfulExtensionsTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task FeatureFlow() => ExecuteTestAfterSetupAsync(context => context.TestFlowAdditionalStylingPartAsync());

    [Fact]
    public Task FeatureWidgets() => ExecuteTestAfterSetupAsync(context => context.TestFeatureWidgetsAsync());

    [Fact]
    public Task FeatureCodeGeneration() => ExecuteTestAfterSetupAsync(context => context.TestFeatureCodeGenerationsAsync());

    [Fact]
    public Task FeatureContentSets() =>
        ExecuteTestAfterSetupAsync(
            context => context.TestFeatureContentSetsAsync(),
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
