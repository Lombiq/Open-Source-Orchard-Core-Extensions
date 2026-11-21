using Lombiq.EmailClient.Tests.UI.Extensions;
using Lombiq.Tests.UI.Extensions;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.NuGet.Tests.UI.Tests.ModuleTests;

public class BehaviorEmailClientTests : UITestBase
{
    public BehaviorEmailClientTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper) =>
        TestBuilder.UseSmtpService();

    [Fact]
    public Task ImapEmailFetchingShouldWork() =>
        RunAsync(context => context.TestImapEmailFetchingAsync());
}
