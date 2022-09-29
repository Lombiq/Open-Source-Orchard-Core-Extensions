using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseNLogHost();

var configuration = builder.Configuration;
builder.Services.AddSingleton(configuration);

builder.Services
    .AddOrchardCms(builder => builder
        .AuthorizeApiRequestsIfEnabled(configuration));

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
        // Nothing to do here.
    }
}
