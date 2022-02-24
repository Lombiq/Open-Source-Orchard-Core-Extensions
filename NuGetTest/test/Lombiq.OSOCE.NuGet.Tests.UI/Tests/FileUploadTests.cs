using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Helpers;
using Lombiq.Tests.UI.Services;
using OpenQA.Selenium;
using Shouldly;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.NuGet.Tests.UI.Tests
{
    public class FileUploadTests : UITestBase
    {
        public FileUploadTests(ITestOutputHelper testOutputHelper)
            : base(testOutputHelper)
        {
        }

        [Theory, Chrome]
        public Task SampleFilesShouldBeAccessible(Browser browser) =>
            ExecuteTestAfterSetupAsync(
                async context =>
                {
                    // The file upload won't work until the privacy consent banner is accepted.
                    await context.DisableFeatureDirectlyAsync("Lombiq.Privacy");

                    // Testing if sample files work.
                    await context.SignInDirectlyAndGoToRelativeUrlAsync("/Admin/DeploymentPlan/Import/Index");
                    context.UploadFile(By.Name("importedPackage"), FileUploadHelper.SamplePdfPath);
                    await context.ClickReliablyOnSubmitAsync();
                    context.Get(By.CssSelector(".message-error"))
                        .Text
                        .ShouldContain("Only zip or json files are supported.");
                },
                browser);
    }
}
