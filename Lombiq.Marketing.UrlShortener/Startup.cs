using Lombiq.Marketing.UrlShortener.Constants;
using Lombiq.Marketing.UrlShortener.Handlers;
using Lombiq.Marketing.UrlShortener.Indexes;
using Lombiq.Marketing.UrlShortener.Migrations;
using Lombiq.Marketing.UrlShortener.Models;
using Lombiq.Marketing.UrlShortener.Services;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.Data;
using OrchardCore.Modules;

namespace Lombiq.Marketing.UrlShortener;

[Feature(FeatureIds.UrlShortener)]
public sealed class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddContentPart<ShortUrlPart>()
            .AddHandler<ShortUrlPartHandler>()
            .WithMigration<ShortUrlMigration>();

        services.AddIndexProvider<ShortUrlPartIndexProvider>();

        services.AddScoped<IUrlShorteningService, UrlShorteningService>();
    }
}
