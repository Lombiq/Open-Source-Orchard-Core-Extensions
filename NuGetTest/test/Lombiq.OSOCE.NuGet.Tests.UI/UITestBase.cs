using Lombiq.OSOCE.NuGet.Tests.UI.Helpers;
using Lombiq.Tests.UI;
using Xunit;

namespace Lombiq.OSOCE.NuGet.Tests.UI;

public class UITestBase : OrchardCoreUITestBase<Program>
{
    protected UITestBase(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
        UseSetup(SetupHelpers.RunSetupAsync);

        // This can be removed once https://github.com/OrchardCMS/OrchardCore/issues/15222 is done.
        AddConfiguration(configuration => configuration.HtmlValidationConfiguration.WithOC15222Filter());
    }
}
