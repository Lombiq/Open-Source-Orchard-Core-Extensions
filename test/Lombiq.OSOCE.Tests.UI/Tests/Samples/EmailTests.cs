using Lombiq.Tests.UI.Samples.Tests;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.Samples;

public class EmailTests : EmailTests<Program>
{
    public EmailTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }
}
