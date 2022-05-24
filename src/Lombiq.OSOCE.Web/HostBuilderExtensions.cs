using Microsoft.Extensions.Hosting;
using NLog;
using NLog.Config;
using NLog.LayoutRenderers;
using NLog.Web;
using System.IO;
using System.Reflection;

namespace OrchardCore.Logging;

public static class HostBuilderExtensions
{
    public static IHostBuilder UseNLogHost(this IHostBuilder builder)
    {
        LayoutRenderer.Register<TenantLayoutRenderer>(TenantLayoutRenderer.LayoutRendererName);
        builder.UseNLog();
        builder.ConfigureAppConfiguration((context, _) =>
        {
            var environment = context.HostingEnvironment;

            environment.ConfigureNLog(Path.Combine(environment.ContentRootPath, "NLog.config"));
            LogManager.Configuration.Variables["configDir"] = environment.ContentRootPath;
        });

        return builder;
    }
}

internal static class AspNetExtensions
{
    public static LoggingConfiguration ConfigureNLog(this IHostEnvironment env, string configFileRelativePath)
    {
        ConfigurationItemFactory.Default.RegisterItemsFromAssembly(typeof(AspNetExtensions).GetTypeInfo().Assembly);
        LogManager.AddHiddenAssembly(typeof(AspNetExtensions).GetTypeInfo().Assembly);

        var fileName = Path.Combine(env.ContentRootPath, configFileRelativePath);
        LogManager.LoadConfiguration(fileName);

        return LogManager.Configuration;
    }
}
