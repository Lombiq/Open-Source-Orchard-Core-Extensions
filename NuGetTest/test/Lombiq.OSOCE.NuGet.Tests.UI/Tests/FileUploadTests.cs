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
                context =>
                {
                    // Testing if sample files work.
                    context.SignInDirectlyAndGoToRelativeUrl("/Admin/DeploymentPlan/Import/Index");
                    context.UploadFile(By.Name("importedPackage"), FileUploadHelper.SamplePdfPath);
                    context.ClickReliablyOnSubmit();
                    context.Get(By.CssSelector(".message-error"))
                        .Text
                        .ShouldBe("Only zip or json files are supported.");
                },
                browser);
    }
}
