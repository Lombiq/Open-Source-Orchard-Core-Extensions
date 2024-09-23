using Lombiq.Hosting.Tenants.EmailQuotaManagement.Tests.UI.Extensions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.NuGet.Tests.UI.Tests.ModuleTests;

public class BehaviorEmailQuotaTests : UITestBase
{
    public BehaviorEmailQuotaTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task EmailQuotaShouldBlockEmailsOverLimitAndWarn() =>
        ExecuteTestAfterSetupAsync(
            context => context.TestEmailQuotaManagementBehaviorAsync(10),
            configuration => configuration.SetEmailQuotaManagementOptionsForUITest(10));

    [Fact]
    public Task EmailQuotaShouldNotBlockEmailsWhenDifferentHostIsUsedThanOriginalFromConfig() =>
        ExecuteTestAfterSetupAsync(
            context => context.TestEmailQuotaManagementBehaviorAsync(1, moduleShouldInterfere: false),
            // The default SMTP host is localhost during UI tests, we set it to 127.0.0.1 to be able to send emails,
            // but the Email Quota module shouldn't interfere.
            configuration => configuration.SetEmailQuotaManagementOptionsForUITest(1));
}
