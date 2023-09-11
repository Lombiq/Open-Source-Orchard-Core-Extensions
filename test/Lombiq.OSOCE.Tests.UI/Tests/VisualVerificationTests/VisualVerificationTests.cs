using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Constants;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using OpenQA.Selenium;
using SixLabors.ImageSharp;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.VisualVerificationTests;

public class VisualVerificationTests : UITestBase
{
    public static readonly Size[] VisualVerificationSizes =
    {
        CommonDisplayResolutions.Standard,
    };

    public VisualVerificationTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Theory, Chrome]
    public Task VerifyHomePageAndLayout(Browser browser) =>
        // Check the whole page so we can verify the margins and to see if header/footer is affected.
        ExecuteTestAfterSetupAsync(
            context => context.AssertVisualVerificationOnAllResolutions(
                VisualVerificationSizes,
                _ => By.TagName("body"),
                configurator: configuration => configuration.WithFileNameSuffix(
                    RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "Windows" : "Unix")),
            browser);
}
