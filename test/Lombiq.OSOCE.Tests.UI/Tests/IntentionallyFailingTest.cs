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

#pragma warning disable CA2201
    [Fact]
    public Task IntentionallyFailing() =>
        throw new System.Exception();
#pragma warning restore CA2201

    [Fact]
    public void IntentionallyEmptyButPassing()
    {
        // Won't fail.
    }
}
