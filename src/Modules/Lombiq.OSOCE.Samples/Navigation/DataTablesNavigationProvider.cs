using Lombiq.DataTables.Controllers;
using Lombiq.DataTables.Samples.Controllers;
using Lombiq.DataTables.Samples.Services;
using Lombiq.HelpfulLibraries.Libraries.Navigation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using OrchardCore.Navigation;
using System;
using System.Diagnostics.CodeAnalysis;

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

        protected override void Build(NavigationBuilder builder) =>
            builder
                .Add(T["Data Tables"], builder => builder
                    .Add(T["Tag Helper"], itemBuilder => itemBuilder
                        .ActionTask<SampleController>(_hca.HttpContext, controller => controller.DataTableTagHelper()))
                    .Add(T["JSON Provider"], itemBuilder => itemBuilder
                        .Action<SampleController>(_hca.HttpContext, controller => controller.ProviderWithShape()))
                    .Add(T["JSON-based Provider (Admin)"], AdminTable(nameof(SampleJsonResultDataTableDataProvider)))
                    .Add(T["Index-based Provider (Admin)"], AdminTable(nameof(SampleIndexBasedDataTableDataProvider))));

        [SuppressMessage(
            "Style",
            "MA0003:Add argument name to improve readability",
            Justification = "You can't use named arguments in Expressions.")]
        private Action<NavigationItemBuilder> AdminTable(string name) =>
            itemBuilder => itemBuilder
                .ActionTask<TableController>(_hca.HttpContext, controller => controller.Get(
                    name,
                    null,
                    true,
                    false));
    }
}
