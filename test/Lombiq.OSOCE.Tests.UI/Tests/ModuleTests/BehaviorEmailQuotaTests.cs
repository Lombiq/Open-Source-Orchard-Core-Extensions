using Lombiq.Hosting.Tenants.EmailQuotaManagement.Tests.UI.Extensions;
using OrchardCore.Email;
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

    [Fact]
    public Task EmailQuotaShouldBlockEmailsOverLimitAndWarn() =>
        ExecuteTestAfterSetupAsync(
            context => context.TestEmailQuotaManagementBehaviorAsync(
                maximumEmailQuota: 10,
                new SmtpSettings { Host = "localhost", IsEnabled = true }),
            configuration => configuration.SetEmailQuotaManagementOptionsForUITest(10));

    [Fact]
    public Task EmailQuotaShouldNotBlockEmailsWhenDifferentHostIsUsedThanOriginalFromConfig() =>
        // The default SMTP host is localhost during UI tests, we set it to 127.0.0.1 to be able to send emails, but the
        // Email Quota module shouldn't interfere.
        ExecuteTestAfterSetupAsync(
            context => context.TestEmailQuotaManagementBehaviorAsync(
                maximumEmailQuota: 1,
                new SmtpSettings { Host = "127.0.0.1", IsEnabled = true },
                moduleShouldInterfere: false),
            configuration => configuration.SetEmailQuotaManagementOptionsForUITest(1));
}
