using Lombiq.ChartJs.Constants;
using Lombiq.HelpfulLibraries.OrchardCore.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrchardCore.Admin;
using OrchardCore.Logging;
using OrchardCore.Mvc.Routing;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using UIKitFeatureIds = Lombiq.UIKit.FeatureIds;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseNLogHost();

var configuration = builder.Configuration;

// Here we're adding the configuration to builder services. It will be used for configuring the UI Testing Toolbox
// (https://github.com/Lombiq/UI-Testing-Toolbox) so UI tests can be executed on the app. For a tutorial on how to
// create UI tests check out the project.
builder.Services
    .AddSingleton(configuration)
    .AddOrchardCms(orchardCoreBuilder => orchardCoreBuilder
        .AddOrchardCoreApplicationInsightsTelemetry(configuration)
        .ConfigureFeaturesGuard(
            new Dictionary<string, IEnumerable<string>> { ["OrchardCore.Twitter"] = new[] { UIKitFeatureIds.Base, FeatureIds.Default, }, })
        .EnableAutoSetupIfNotUITesting(configuration)
        // allowInlineStyle is necessary because style attributes are used in the Blog theme.
        .ConfigureSecurityDefaultsWithStaticFiles(allowInlineStyle: true));

builder.Services.AddInlineStartup(
    services =>
    {
        services.RemoveAll(x => x.ServiceType == typeof(IAreaControllerRouteMapper));
        services.AddScoped<IAreaControllerRouteMapper, AdminMapper>();
        services.AddScoped<IAreaControllerRouteMapper, DefaultMapper>();
    },
    _ => { },
    order: 999999);

var app = builder.Build();
app.UseOrchardCore();
app.Run();

[SuppressMessage(
    "Design",
    "CA1050: Declare types in namespaces",
    Justification = "As described here: https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests.")]
public partial class Program
{
    protected Program()
    {
        // Nothing to do here.
    }
}

[SuppressMessage("Design", "CA1050:Declare types in namespaces", Justification = "Temporary")]
public class AdminMapper : IAreaControllerRouteMapper
{
    private const string DefaultAreaPattern = "{area}/{controller}/{action}/{id?}";
    private readonly string _adminUrlPrefix;

    public int Order => -1000;

    public AdminMapper(IOptions<AdminOptions> adminOptions) =>
        _adminUrlPrefix = adminOptions.Value.AdminUrlPrefix;

    public bool TryMapAreaControllerRoute(IEndpointRouteBuilder routes, ControllerActionDescriptor descriptor)
    {
        var controllerAttribute = descriptor.ControllerTypeInfo.GetCustomAttribute<AdminAttribute>();
        var actionAttribute = descriptor.MethodInfo.GetCustomAttribute<AdminAttribute>();

        if (descriptor.ControllerName != "Admin" && controllerAttribute == null && actionAttribute == null)
        {
            return false;
        }

        string name = null;
        var pattern = DefaultAreaPattern;

        if (!string.IsNullOrWhiteSpace(actionAttribute?.Template))
        {
            name = actionAttribute.RouteName;
            pattern = actionAttribute.Template;
        }
        else if (!string.IsNullOrWhiteSpace(controllerAttribute?.Template))
        {
            name = controllerAttribute.RouteName;
            pattern = controllerAttribute.Template;
        }

        var area = descriptor.RouteValues["area"];
        var controller = descriptor.ControllerName;
        var action = descriptor.ActionName;

        routes.MapAreaControllerRoute(
            name: ReplaceMvcPlaceholders(name, area, controller, action) ?? descriptor.DisplayName,
            areaName: area,
            pattern: $"{_adminUrlPrefix}/{ReplaceMvcPlaceholders(pattern.TrimStart('/'), area, controller, action)}",
            defaults: new { controller, action });

        return true;
    }

    public static string ReplaceMvcPlaceholders(string name, string area, string controller, string action) =>
        name?
            .Replace("{area}", area)
            .Replace("{controller}", controller)
            .Replace("{action}", action);
}

[SuppressMessage("Design", "CA1050:Declare types in namespaces", Justification = "Temporary")]
public class DefaultMapper : IAreaControllerRouteMapper
{
    private const string DefaultAreaPattern = "/{area}/{controller}/{action}/{id?}";

    public int Order => 1000;

    public bool TryMapAreaControllerRoute(IEndpointRouteBuilder routes, ControllerActionDescriptor descriptor)
    {
        var area = descriptor.RouteValues["area"];
        var controller = descriptor.ControllerName;
        var action = descriptor.ActionName;

        routes.MapAreaControllerRoute(
            name: descriptor.DisplayName,
            areaName: descriptor.RouteValues["area"],
            pattern: AdminMapper.ReplaceMvcPlaceholders(DefaultAreaPattern, area, controller, action),
            defaults: new { controller = descriptor.ControllerName, action = descriptor.ActionName }
        );

        return true;
    }
}
