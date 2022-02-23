using Lombiq.HelpfulLibraries.Libraries.Navigation;
using Lombiq.HelpfulLibraries.Samples.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using OrchardCore.Navigation;

namespace Lombiq.BaseTheme.Samples.Navigation
{
    public class HelpfulLibrariesNavigationProvider : MainMenuNavigationProviderBase
    {
        private readonly IHttpContextAccessor _hca;

        public HelpfulLibrariesNavigationProvider(
            IHttpContextAccessor hca,
            IStringLocalizer<HelpfulLibrariesNavigationProvider> stringLocalizer)
            : base(hca, stringLocalizer) =>
            _hca = hca;

        protected override void Build(NavigationBuilder builder)
        {
            var context = _hca.HttpContext;
            builder
                .Add(T["Helpful Libraries"], builder => builder
                    .Add(T["LINQ to DB"], itemBuilder => itemBuilder
                        .Add(T["Simple Query"], subMenu => subMenu
                            .Action<LinqToDbSamplesController>(context, controller => controller.SimpleQuery()))
                        .Add(T["Join Query"], subMenu => subMenu
                            .Action<LinqToDbSamplesController>(context, controller => controller.JoinQuery()))
                        .Add(T["CRUD"], subMenu => subMenu
                            .Action<LinqToDbSamplesController>(context, controller => controller.Crud())))
                    .Add(T["Typed Route"], itemBuilder => itemBuilder
                        .Action<TypedRouteController>(context, controller => controller.Index())));
        }
    }
}
