using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Lombiq.OSOCE.Web
{
    public sealed class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration) => _configuration = configuration;

        public void ConfigureServices(IServiceCollection services) =>
            // Here we're configuring the UI Testing Toolbox (https://github.com/Lombiq/UI-Testing-Toolbox) so UI tests
            // can be executed on the app. For a tutorial on how to create UI tests check out the project.
            // ConfigureUITesting() won't do anything when the app is not run for UI testing.
            services.AddOrchardCms(builder => builder.ConfigureUITesting(_configuration, enableShortcutsDuringUITesting: true));

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
