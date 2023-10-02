using Lombiq.Hosting.Tenants.EmailQuotaManagement.Tests.UI.Extensions;
using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Services;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

public class BehaviorEmailQuotaTests : UITestBase
{
    public BehaviorEmailQuotaTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Theory, Chrome]
    public Task EmailQuotaShouldBlockEmailsOverLimit(Browser browser) =>
        ExecuteTestAfterSetupAsync(
            context => context.TestEmailQuotaManagementBehaviorAsync(1),
            browser,
            configuration => configuration.SetEmailQuotaManagementOptionsForUITest(1));

    [Theory, Chrome]
    public Task EmailQuotaShouldNotBlockEmails(Browser browser) =>
        ExecuteTestAfterSetupAsync(
            context => context.TestEmailQuotaManagementBehaviorAsync(1, moduleShouldInterfere: false),
            browser,
            // The default SMTP host is localhost during UI tests, we set it to 127.0.0.1 to be able to send emails,
            // but the Email Quota module shouldn't interfere.
            configuration => configuration.SetEmailQuotaManagementOptionsForUITest(1, "127.0.0.1"));
}
