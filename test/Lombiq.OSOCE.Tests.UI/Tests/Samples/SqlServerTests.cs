using Lombiq.Tests.UI.Samples.Tests;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.Samples;

public class SqlServerTests : SqlServerTests<Program>
{
    public SqlServerTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }
}
