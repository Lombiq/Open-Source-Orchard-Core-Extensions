using Lombiq.HelpfulLibraries.Common.DependencyInjection;
using Lombiq.HelpfulLibraries.OrchardCore.Users;
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
