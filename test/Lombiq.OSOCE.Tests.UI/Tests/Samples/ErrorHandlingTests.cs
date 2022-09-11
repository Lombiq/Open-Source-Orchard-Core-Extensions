using Lombiq.Tests.UI.Samples.Tests;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.Samples;

public class ErrorHandlingTests : ErrorHandlingTests<Program>
{
    public ErrorHandlingTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }
}
