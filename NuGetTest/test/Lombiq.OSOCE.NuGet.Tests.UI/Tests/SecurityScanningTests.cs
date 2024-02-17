using Lombiq.OSOCE.NuGet.Tests.UI.Helpers;
using Lombiq.Tests.UI.SecurityScanning;
using Shouldly;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.NuGet.Tests.UI.Tests;

public class SecurityScanningTests : UITestBase
{
    public SecurityScanningTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    // Only scanning the homepage, since this is just to make sure that ZAP still works from NuGet.
    [Fact]
    public Task BasicSecurityScanShouldPass() =>
        ExecuteTestAfterSetupAsync(
            context => context.RunAndAssertBaselineSecurityScanAsync(
                configuration => configuration.ExcludeUrlWithRegex(".*:[0-9]+\\/.+"),
                // We expect 5 alerts from ZAP. This is using "less than" not to fail the test, should ZAP be a bit
                // inconsistent, which it can be (see https://www.zaproxy.org/faq/why-can-zap-scans-be-inconsistent/).
                // If this starts failing after some update, then inspect the scan report in the failure dump to see if
                // the alerts can be simply expected and this number should be increased.
                sarifLog => sarifLog.Runs[0].Results.Count.ShouldBeLessThan(3)),
            // Can be removed once  https://github.com/OrchardCMS/OrchardCore/issues/15222 is done.
            changeConfiguration => changeConfiguration.AssertBrowserLog = AssertBrowserLogHelpers.AssertBrowserLogIsEmpty);
}
