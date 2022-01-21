using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Pages;
using Lombiq.Tests.UI.Services;
using OpenQA.Selenium;
using System;

namespace Lombiq.OSOCE.NuGet.Tests.UI.Helpers
{
    public static class SetupHelpers
    {
        public const string RecipeId = "Lombiq.OSOCE.NuGet.Tests";

        public static Uri RunSetup(UITestContext context)
        {
            var uri = context
                .GoToSetupPage()
                .SetupOrchardCore(
                    context,
                    new OrchardCoreSetupParameters(context)
                    {
                        SiteName = "Lombiq's Open-Source Orchard Core Extensions - UI Testing",
                        RecipeId = RecipeId,
                        TablePrefix = "OSOCE",
                        SiteTimeZoneValue = "Europe/Budapest",
                    })
                .PageUri
                .Value;

            context.Exists(By.Id("navbar"));

            return uri;
        }
    }
}
