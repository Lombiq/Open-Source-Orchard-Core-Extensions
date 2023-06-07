using Lombiq.Tests.UI.Constants;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Pages;
using Lombiq.Tests.UI.Services;
using System;
using System.Threading.Tasks;

namespace Lombiq.OSOCE.Tests.UI.Helpers;

public static class SetupHelpers
{
    public static async Task<Uri> RunBlogSetupAsync(UITestContext context)
    {
        // We must use a custom configuration with the "Blog" setup to test if the feature works when enabled on
        // an existing stock site without setup pre-configuration.
        var homepageUri = await context.GoToSetupPageAndSetupOrchardCoreAsync(
            new OrchardCoreSetupParameters(context)
            {
                SiteName = "Lombiq's OSOCE - UI Testing - With Blog Setup",
                RecipeId = "Blog",
                TablePrefix = "OSOCE_blog",
                SiteTimeZoneValue = "Europe/Budapest",
            });

        return homepageUri;
    }
}
