using Lombiq.HelpfulLibraries.OrchardCore.Navigation;
using Lombiq.UIKit.Showcase.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using OrchardCore.Navigation;

namespace Lombiq.OSOCE.Samples;

public class Navigation : MainMenuNavigationProviderBase
{
    public Navigation(IHttpContextAccessor hca, IStringLocalizer<Navigation> stringLocalizer)
        : base(hca, stringLocalizer)
    {
    }

    protected override void Build(NavigationBuilder builder)
    {
        var context = _hca.HttpContext;
        builder
            .Add(T["UI Kit"], builder => builder
                .Add(T["Showcase"], itemBuilder => itemBuilder
                    .Action<ShowcaseController>(context, controller => controller.Showcase()))
            );
    }
}
