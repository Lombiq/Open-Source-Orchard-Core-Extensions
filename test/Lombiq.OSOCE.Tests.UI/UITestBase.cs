using Lombiq.Tests.UI;
using Lombiq.Tests.UI.Constants;
using Lombiq.Tests.UI.Services;
using System;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI;

public abstract class UITestBase : OrchardCoreUITestBase<Program>
{
    protected UITestBase(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    protected override Task ExecuteTestAfterSetupAsync(
        Func<UITestContext, Task> testAsync,
        Browser browser,
        Func<OrchardCoreUITestExecutorConfiguration, Task> changeConfigurationAsync) =>
        ExecuteTestAsync(testAsync, browser, Lombiq.Tests.UI.Samples.Helpers.SetupHelpers.RunSetupAsync, changeConfigurationAsync);

    protected override Task ExecuteTestAsync(
        Func<UITestContext, Task> testAsync,
        Browser browser,
        Func<UITestContext, Task<Uri>> setupOperation,
        Func<OrchardCoreUITestExecutorConfiguration, Task> changeConfigurationAsync) =>
        base.ExecuteTestAsync(
            testAsync,
            browser,
            setupOperation,
            async configuration =>
            {
                ChangeConfiguration(configuration);
                if (changeConfigurationAsync != null) await changeConfigurationAsync(configuration);
            });

    protected static void ChangeConfiguration(OrchardCoreUITestExecutorConfiguration configuration)
    {
        configuration.BrowserConfiguration.DefaultBrowserSize = CommonDisplayResolutions.HdPlus;

        configuration.BrowserConfiguration.Headless =
            TestConfigurationManager.GetBoolConfiguration("BrowserConfiguration:Headless", defaultValue: false);

        configuration.AssertAppLogsAsync = OrchardCoreUITestExecutorConfiguration.AssertAppLogsCanContainWarningsAndCacheFolderErrorsAsync;
    }
}
