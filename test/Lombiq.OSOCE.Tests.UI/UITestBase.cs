using Lombiq.Tests.UI;
using Lombiq.Tests.UI.Constants;
using Lombiq.Tests.UI.Samples.Helpers;
using Lombiq.Tests.UI.Services;
using Xunit;

namespace Lombiq.OSOCE.Tests.UI;

public abstract class UITestBase : OrchardCoreUITestBase<Program>
{
    protected UITestBase(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper) =>
        UseSetup(SetupHelpers.RunSetupAsync);

    protected override void Configure(OrchardCoreUITestExecutorConfiguration configuration) =>
        ChangeConfiguration(configuration);

    protected static void ChangeConfiguration(OrchardCoreUITestExecutorConfiguration configuration)
    {
        configuration.BrowserConfiguration.DefaultBrowserSize = CommonDisplayResolutions.HdPlus;

        configuration.BrowserConfiguration.Headless =
            TestConfigurationManager.GetBoolConfiguration("BrowserConfiguration:Headless", defaultValue: false);
    }
}
