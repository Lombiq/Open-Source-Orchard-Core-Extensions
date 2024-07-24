using Lombiq.JsonEditor.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

public class BehaviorJsonEditorTests : UITestBase
{
    public BehaviorJsonEditorTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task JsonEditorShouldWorkCorrectly() =>
        ExecuteTestAfterSetupAsync(context => context.TestJsonEditorBehaviorAsync());
}
