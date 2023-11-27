using Lombiq.Tests.UI.Constants;
using Lombiq.Tests.UI.Extensions;
using OpenQA.Selenium;
using SixLabors.ImageSharp;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.VisualVerificationTests;

public class VisualVerificationTests : UITestBase
{
    private static readonly Size[] _visualVerificationSizes =
    {
        CommonDisplayResolutions.Standard,
    };

    public VisualVerificationTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task VerifyHomePageAndLayout() =>
        // Check the whole page so we can verify the margins and to see if header/footer is affected.
        ExecuteTestAfterSetupAsync(
            context => context.AssertVisualVerificationOnAllResolutions(
                _visualVerificationSizes,
                _ => By.TagName("body"),
                configurator: configuration => configuration.WithFileNameSuffix(
                    RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "Windows" : "Unix")));
}
