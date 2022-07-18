using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseNLogHost();

var configuration = builder.Configuration;

// Here we're configuring the UI Testing Toolbox (https://github.com/Lombiq/UI-Testing-Toolbox) so UI tests can be
// executed on the app. For a tutorial on how to create UI tests check out the project. ConfigureUITesting() won't do
// anything when the app is not run for UI testing.
builder.Services.AddOrchardCms(orchard => orchard.ConfigureUITesting(configuration, enableShortcutsDuringUITesting: true));

var app = builder.Build();

app.UseStaticFiles();
app.UseOrchardCore();
app.Run();

// As described here(https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-6.0).
#pragma warning disable CA1050
public partial class Program
#pragma warning restore CA1050
{
    protected Program()
    {

    }
}
