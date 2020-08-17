using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;

namespace Lombiq.OSOCE.Web
{
    [SuppressMessage("Major Code Smell", "S1118:Utility classes should not have public constructors", Justification = "Needs to be non-static for UseStartup().")]
    public sealed class Startup
    {
        public static void ConfigureServices(IServiceCollection services) => services.AddOrchardCms();

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseOrchardCore();
        }
    }
}
