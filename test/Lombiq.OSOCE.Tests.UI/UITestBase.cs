using Lombiq.Tests.UI;
using Lombiq.Tests.UI.Constants;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Helpers;
using Lombiq.Tests.UI.Samples.Helpers;
using Lombiq.Tests.UI.Services;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI;

public class UITestBase : OrchardCoreUITestBase
{
    protected override string AppAssemblyPath => WebAppConfigHelper
        .GetAbsoluteApplicationAssemblyPath("Lombiq.OSOCE.Web", "net6.0");

    protected UITestBase(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    protected override Task ExecuteTestAfterSetupAsync(
        Func<UITestContext, Task> testAsync,
        Browser browser,
        Func<OrchardCoreUITestExecutorConfiguration, Task> changeConfigurationAsync) =>
        ExecuteTestAsync(testAsync, browser, SetupHelpers.RunSetupAsync, changeConfigurationAsync);

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
                configuration.BrowserConfiguration.DefaultBrowserSize = CommonDisplayResolutions.HdPlus;

                configuration.BrowserConfiguration.Headless =
                    TestConfigurationManager.GetBoolConfiguration("BrowserConfiguration:Headless", defaultValue: false);

                configuration.AssertAppLogsAsync = AssertAppLogsDefaultOSOCEAsync;

                if (changeConfigurationAsync != null) await changeConfigurationAsync(configuration);
            });

    public static readonly Func<IWebApplicationInstance, Task> AssertAppLogsDefaultOSOCEAsync =
        async webApplicationInstance =>
            (await webApplicationInstance.GetLogOutputAsync())
            .ReplaceOrdinalIgnoreCase(
                "|Lombiq.TrainingDemo.Services.DemoBackgroundTask|ERROR|Expected non-error",
                "|Lombiq.TrainingDemo.Services.DemoBackgroundTask|EXPECTED_ERROR|Expected non-error")
            .ShouldNotContain("|ERROR|");
}
