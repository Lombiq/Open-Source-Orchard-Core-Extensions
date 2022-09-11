using Lombiq.Tests.UI.Samples.Tests;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.Samples;

public class MonkeyTests : MonkeyTests<Program>
{
    public MonkeyTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }
}
