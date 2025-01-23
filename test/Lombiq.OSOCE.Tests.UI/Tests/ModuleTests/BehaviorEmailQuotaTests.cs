using Lombiq.Hosting.Tenants.EmailQuotaManagement.Tests.UI.Extensions;
using Lombiq.Tests.UI.Extensions;
using System.Threading.Tasks;
using Xunit;
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

    // Will be re-enabled as part of https://github.com/Lombiq/Open-Source-Orchard-Core-Extensions/issues/703.
#pragma warning disable xUnit1004 // Test methods should not be skipped
    [Fact(Skip = "Fails with smtp4dev JS exceptions, but works under https://github.com/Lombiq/Open-Source-Orchard-Core-Extensions/issues/703.")]
#pragma warning restore xUnit1004 // Test methods should not be skipped
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
