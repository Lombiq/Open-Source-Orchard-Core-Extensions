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
            context => context.TestEmailQuotaManagementBehaviorAsync(1, moduleOn: false),
            browser,
            configuration => configuration.SetEmailQuotaManagementOptionsForUITest(1, "127.0.0.1"));
}
