using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseNLog();

var configuration = builder.Configuration;

// Without this we get an error after setup that says "InvalidOperationException: Unable to resolve service for type
// 'Microsoft.AspNetCore.Identity.IdentityErrorDescriber' while attempting to activate
// 'Microsoft.AspNetCore.Identity.RoleManager`1[OrchardCore.Security.IRole]'.".
builder.Services.AddScoped<IdentityErrorDescriber>();

// Here we're configuring the UI Testing Toolbox (https://github.com/Lombiq/UI-Testing-Toolbox) so UI tests can be
// executed on the app. For a tutorial on how to create UI tests check out the project. ConfigureUITesting() won't do
// anything when the app is not run for UI testing.
builder.Services.AddOrchardCms(builder => builder.ConfigureUITesting(configuration, enableShortcutsDuringUITesting: true));

var app = builder.Build();

app.UseStaticFiles();
app.UseOrchardCore();
app.Run();
