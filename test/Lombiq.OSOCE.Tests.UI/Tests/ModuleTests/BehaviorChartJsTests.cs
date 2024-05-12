using Lombiq.ChartJs.Models;
using Lombiq.ChartJs.Tests.UI.Extensions;
using Shouldly;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

public class BehaviorChartJsTests : UITestBase
{
    public BehaviorChartJsTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task RecipeDataShouldBeDisplayedCorrectly() =>
        ExecuteTestAfterSetupAsync(context => context.TestChartJsSampleBehaviorAsync());

    [Fact]
    public void DataLabelAlignmentConfigurationShouldSerializeCorrectly()
    {
        var data = new DataLabelAlignmentConfiguration
        {
            Align = DataLabelAlignment.Start,
            Anchor = DataLabelAlignment.Center,
            Font = new DataLabelAlignmentConfiguration.FontStyle { IsBold = true, Size = 16.5 },
            Offset = 3.14,
        };

        var json = JsonSerializer.Serialize(data);
        json.ShouldBe("{\"align\":\"start\",\"anchor\":\"center\",\"offset\":3.14,\"font\":{\"size\":16.5}}");

        var deserialized = JsonSerializer.Deserialize<DataLabelAlignmentConfiguration>(json);
        deserialized.ShouldBe(data);
    }
}
