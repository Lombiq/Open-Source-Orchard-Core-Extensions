using Lombiq.Tests.UI.Samples.Tests;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.Samples;

public class TenantTests : TenantTests<Program>
{
    public TenantTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }
}
