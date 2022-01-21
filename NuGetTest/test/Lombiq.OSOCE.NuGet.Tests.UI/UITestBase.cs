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
            Action<UITestContext> test,
            Browser browser,
            Action<OrchardCoreUITestExecutorConfiguration> changeConfiguration = null) =>
            ExecuteTestAsync(test, browser, SetupHelpers.RunSetup, changeConfiguration);

        protected override Task ExecuteTestAsync(
            Action<UITestContext> test,
            Browser browser,
            Func<UITestContext, Uri> setupOperation = null,
            Action<OrchardCoreUITestExecutorConfiguration> changeConfiguration = null) =>
            base.ExecuteTestAsync(
                test,
                browser,
                setupOperation,
                configuration => changeConfiguration?.Invoke(configuration));
    }
}
