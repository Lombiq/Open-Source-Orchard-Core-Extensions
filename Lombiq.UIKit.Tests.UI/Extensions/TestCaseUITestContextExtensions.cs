using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using OpenQA.Selenium;
using System.Threading.Tasks;

namespace Lombiq.UIKit.Tests.UI.Extensions
{
    public static class TestCaseUITestContextExtensions
    {
        public static async Task TestShowCasePageAsync(this UITestContext context)
        {
            await context.SignInDirectlyAsync();
            await context.EnableUIKitShowcaseDirectlyAsync();
            await context.GoToShowcasePageAsync();

            await context.TestTextBoxesAsync();
        }

        public static async Task TestTextBoxesAsync(this UITestContext context)
        {
            var textBox = By.Id("TextBox1");

            context.Exists(textBox);
            //context.VerifyElementTexts(textBox, string.Empty);

            //await context.ClickAndFillInWithRetriesAsync(textBox, "Test");
            //context.VerifyElementTexts(textBox, "Test");
        }
    }
}
