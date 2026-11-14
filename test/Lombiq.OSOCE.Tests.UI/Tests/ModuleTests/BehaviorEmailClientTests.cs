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
        AddSyncConfiguration(config => config.UseSmtpService = true)
            .RunAsync(context => context.TestImapEmailFetchingAsync());
}
