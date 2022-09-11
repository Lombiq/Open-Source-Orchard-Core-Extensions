using Lombiq.Tests.UI.Samples.Tests;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.Samples;

public class AccessibilityTest : AccessibilityTest<Program>
{
    public AccessibilityTest(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }
}
