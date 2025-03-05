using Lombiq.EmailClient.Tests.UI.Extensions;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

public class BehaviorEmailClientTests : UITestBase
{
    public BehaviorEmailClientTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task ImapEmailFetchingShouldWork() =>
        ExecuteTestAfterSetupAsync(
            context => context.TestImapEmailFetchingAsync(),
            config => config.UseSmtpService = true);
}
