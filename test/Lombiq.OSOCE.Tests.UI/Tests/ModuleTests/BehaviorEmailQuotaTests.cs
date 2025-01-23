using Lombiq.Hosting.Tenants.EmailQuotaManagement.Tests.UI.Extensions;
using Lombiq.Tests.UI.Extensions;
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
            context => context.TestEmailQuotaManagementBehaviorAsync(10),
            configuration => configuration.SetEmailQuotaManagementOptionsForUITest(10));

    [Fact]
    public Task EmailQuotaShouldNotBlockEmailsWhenDifferentHostIsUsedThanOriginalFromConfig() =>
        ExecuteTestAfterSetupAsync(
            async context =>
            {
                // The default SMTP host is localhost during UI tests. We set it to 127.0.0.1 to still be able to send
                // emails (since localhost and 127.0.0.1 is the same), but the Email Quota module shouldn't interfere,
                // thinking it's a non-default host.
                await context.SignInDirectlyAndGoToDashboardAsync();
                await context.ConfigureSmtpSettingsAsync("sender@example.com", "127.0.0.1");
                await context.TestEmailQuotaManagementBehaviorAsync(1, moduleShouldInterfere: false);
            },
            configuration => configuration.SetEmailQuotaManagementOptionsForUITest(1));
}
