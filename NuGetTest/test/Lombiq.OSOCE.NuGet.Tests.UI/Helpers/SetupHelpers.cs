using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Models;
using Lombiq.Tests.UI.Services;
using OpenQA.Selenium;
using System;
using System.Threading.Tasks;

namespace Lombiq.OSOCE.NuGet.Tests.UI.Helpers;

public static class SetupHelpers
{
    public const string RecipeId = "Lombiq.OSOCE.NuGet.Tests";

    public static async Task<Uri> RunSetupAsync(UITestContext context)
    {
        var homepageUri = await context.GoToSetupAndSetupOrchardCoreAsync(
            new OrchardCoreSetupParameters(context)
            {
                SiteName = "Lombiq's OSOCE - UI Testing",
                RecipeId = RecipeId,
                TablePrefix = "OSOCE",
                SiteTimeZoneValue = "Europe/Budapest",
            });

        context.Exists(By.Id("navbar"));

        return homepageUri;
    }
}
