using Lombiq.OSOCE.NuGet.Tests.UI.Helpers;
using Lombiq.Tests.UI;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.NuGet.Tests.UI;

public class UITestBase : OrchardCoreUITestBase<Program>
{
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
                configuration.AssertAppLogsAsync =
                    OrchardCoreUITestExecutorConfiguration.AssertAppLogsCanContainCacheFolderErrorsAsync;

                // This can be removed once https://github.com/OrchardCMS/OrchardCore/issues/15222 is done.
                configuration.HtmlValidationConfiguration.AssertHtmlValidationResultAsync = async errors =>
                {
                    var errorResult = (await errors.GetErrorsAsync())
                        .Where(error => !error.ContainsOrdinalIgnoreCase("Prefer to use the native <button> element"));

                    errorResult.ShouldBeEmpty();
                };

                if (changeConfigurationAsync != null) await changeConfigurationAsync(configuration);
            });
}
