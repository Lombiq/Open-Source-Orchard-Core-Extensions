using Lombiq.Tests.UI.Samples.Tests;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.Samples;

public class AzureBlobStorageTests : AzureBlobStorageTests<Program>
{
    public AzureBlobStorageTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }
}
