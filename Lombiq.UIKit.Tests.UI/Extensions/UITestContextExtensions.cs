using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using System.Threading.Tasks;

namespace Lombiq.UIKit.Tests.UI.Extensions
{
    public static class UITestContextExtensions
    {
        public static Task EnableUIKitShowcaseDirectlyAsync(this UITestContext context) =>
            context.EnableFeatureDirectlyAsync("Lombiq.UIKit.Showcase");

        public static Task GoToShowcasePageAsync(this UITestContext context) =>
            context.GoToRelativeUrlAsync("/UIKitShowcase");
    }
}
