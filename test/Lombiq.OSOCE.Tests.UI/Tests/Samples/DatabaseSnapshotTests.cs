using Lombiq.Tests.UI.Samples.Tests;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.Samples;

public class DatabaseSnapshotTests : DatabaseSnapshotTests<Program>
{
    public DatabaseSnapshotTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }
}
