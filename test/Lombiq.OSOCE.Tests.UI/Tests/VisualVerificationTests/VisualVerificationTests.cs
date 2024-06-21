using Lombiq.Tests.UI.Constants;
using Lombiq.Tests.UI.Extensions;
using OpenQA.Selenium;
using SixLabors.ImageSharp;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.VisualVerificationTests;

public class VisualVerificationTests : UITestBase
{
    private static readonly Size[] _visualVerificationSizes =
    [
        CommonDisplayResolutions.Standard,
    ];

    public VisualVerificationTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task VerifyHomePageAndLayout() =>
        // Check the whole page so we can verify the margins and to see if header/footer is affected.
        // The threshold is necessary so the year changing in the footer doesn't cause the test to crash (or other tiny
        // changes in font rendering).
        ExecuteTestAfterSetupAsync(
            context => context.AssertVisualVerificationApprovedOnAllResolutionsWithPlatformSuffix(
                _visualVerificationSizes,
                _ => By.TagName("body"),
                pixelErrorPercentageThreshold: 0.006));
}
