using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseNLogHost();

var configuration = builder.Configuration;

builder.Services
    .AddOrchardCms(builder => builder
        .ConfigureUITesting(configuration, enableShortcutsDuringUITesting: true)
        .AuthorizeApiRequestsIfEnabled(configuration));

var app = builder.Build();

app.UseStaticFiles();
app.UseOrchardCore();
app.Run();
