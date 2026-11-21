using Lombiq.EmailClient.Tests.UI.Extensions;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

public class BehaviorEmailClientTests : UITestBase
{
    public BehaviorEmailClientTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper) =>
        AddConfiguration(config => config.UseSmtpService = true);

    [Fact]
    public Task ImapEmailFetchingShouldWork() =>
        RunAsync(context => context.TestImapEmailFetchingAsync());
}
