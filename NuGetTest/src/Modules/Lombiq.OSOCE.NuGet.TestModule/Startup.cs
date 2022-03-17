using Lombiq.HelpfulLibraries.Libraries.DependencyInjection;
using Lombiq.HelpfulLibraries.Libraries.Users;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;

namespace Lombiq.OSOCE.NuGet.TestModule;

public class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddCachingUserServer();
        services.AddLazyInjectionSupport();
    }
}
