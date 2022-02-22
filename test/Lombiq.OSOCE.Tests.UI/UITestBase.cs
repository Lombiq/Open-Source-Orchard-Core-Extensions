using Lombiq.Tests.UI;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Helpers;
using Lombiq.Tests.UI.Samples.Helpers;
using Lombiq.Tests.UI.Services;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI
{
    public class UITestBase : OrchardCoreUITestBase
    {
        protected override string AppAssemblyPath => WebAppConfigHelper
            .GetAbsoluteApplicationAssemblyPath("Lombiq.OSOCE.Web", "netcoreapp3.1");

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
            Action<UITestContext> test,
            Browser browser,
            Func<UITestContext, Task<Uri>> setupOperation = null,
            Action<OrchardCoreUITestExecutorConfiguration> changeConfiguration = null) =>
            base.ExecuteTestAsync(
                test,
                browser,
                setupOperation,
                configuration =>
                {
                    configuration.AccessibilityCheckingConfiguration.RunAccessibilityCheckingAssertionOnAllPageChanges = true;
                    configuration.UseSqlServer = true;

                    changeConfiguration?.Invoke(configuration);

                    configuration.OrchardCoreConfiguration.EnableApplicationInsightsOfflineOperation();

                    configuration.AssertAppLogsAsync = async webApplicationInstance =>
                        (await webApplicationInstance.GetLogOutputAsync())
                        .ReplaceOrdinalIgnoreCase(
                            "|Lombiq.TrainingDemo.Services.DemoBackgroundTask|ERROR|Expected non-error",
                            "|Lombiq.TrainingDemo.Services.DemoBackgroundTask|EXPECTED_ERROR|Expected non-error")
                        .ShouldNotContain("|ERROR|");
                });
    }
}
