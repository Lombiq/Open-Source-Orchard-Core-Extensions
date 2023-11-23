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
                sarifLog => sarifLog.Runs[0].Results.Count.ShouldBeLessThan(6)));
}
