using Lombiq.OSOCE.NuGet.Tests.UI.Helpers;
using Lombiq.Tests.UI;
using Lombiq.Tests.UI.Helpers;
using Lombiq.Tests.UI.Services;
using System;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.NuGet.Tests.UI
{
    public class UITestBase : OrchardCoreUITestBase
    {
        protected override string AppAssemblyPath => WebAppConfigHelper
            .GetAbsoluteApplicationAssemblyPath("Lombiq.OSOCE.NuGet.Web", "netcoreapp3.1");

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
                    if (changeConfigurationAsync != null) await changeConfigurationAsync(configuration);
                });
    }
}
