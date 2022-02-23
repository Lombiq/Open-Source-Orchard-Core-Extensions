using Lombiq.BaseTheme.Samples.Navigation;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using OrchardCore.Navigation;

namespace Lombiq.OSOCE.Samples
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<INavigationProvider, DataTablesNavigationProvider>();
            services.AddScoped<INavigationProvider, HelpfulLibrariesNavigationProvider>();
        }
    }
}
