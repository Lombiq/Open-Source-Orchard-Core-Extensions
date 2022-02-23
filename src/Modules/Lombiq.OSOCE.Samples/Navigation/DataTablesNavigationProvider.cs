using Lombiq.DataTables.Controllers;
using Lombiq.DataTables.Samples.Controllers;
using Lombiq.DataTables.Samples.Services;
using Lombiq.HelpfulLibraries.Libraries.Navigation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using OrchardCore.Navigation;

namespace Lombiq.BaseTheme.Samples.Navigation
{
    public class DataTablesNavigationProvider : MainMenuNavigationProviderBase
    {
        private readonly IHttpContextAccessor _hca;

        public DataTablesNavigationProvider(
            IHttpContextAccessor hca,
            IStringLocalizer<DataTablesNavigationProvider> stringLocalizer)
            : base(hca, stringLocalizer) =>
            _hca = hca;

        protected override void Build(NavigationBuilder builder)
        {
            var context = _hca.HttpContext;
            builder
                .Add(T["Data Tables"], builder => builder
                    .Add(T["Tag Helper"], itemBuilder => itemBuilder
                        .Action<SampleController>(context, controller => controller.DataTableTagHelper()))
                    .Add(T["JSON Provider"], itemBuilder => itemBuilder
                        .Action<SampleController>(context, controller => controller.ProviderWithShape()))
                    .Add(T["JSON-based Provider (Admin)"], itemBuilder => itemBuilder
                        .Action<TableController>(context, controller => controller.Get(
                            nameof(SampleJsonResultDataTableDataProvider),
                            null,
                            true,
                            false)))
                    .Add(T["Index-based Provider (Admin)"], itemBuilder => itemBuilder
                        .Action<TableController>(context, controller => controller.Get(
                            nameof(SampleIndexBasedDataTableDataProvider),
                            null,
                            true,
                            false))));
        }
    }
}
