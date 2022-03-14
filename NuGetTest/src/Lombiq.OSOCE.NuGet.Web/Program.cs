using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseNLog();

var configuration = builder.Configuration;

builder.Services
    .AddOrchardCms(builder => builder
    .ConfigureUITesting(configuration, enableShortcutsDuringUITesting: true)
    .AuthorizeApiRequestsIfEnabled(configuration));

var app = builder.Build();

app.UseStaticFiles();
app.UseOrchardCore();
app.Run();
