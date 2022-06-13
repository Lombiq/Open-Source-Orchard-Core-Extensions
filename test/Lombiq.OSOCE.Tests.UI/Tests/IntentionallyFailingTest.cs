using Lombiq.DataTables.Tests.UI.Extensions;
using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Services;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests;

public class IntentionallyFailingTest : UITestBase
{
    public IntentionallyFailingTest(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Theory, Chrome]
    public Task IntentionallyFailing(Browser browser) =>
        throw new System.Exception();

    [Fact]
    public void IntentionallyEmptyButPassing()
    {
        // Won't fail.
    }
}
