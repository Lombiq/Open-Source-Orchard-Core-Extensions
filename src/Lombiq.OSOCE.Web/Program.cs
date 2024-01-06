using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Logging;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

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
            new Dictionary<string, IEnumerable<string>>
            {
                ["OrchardCore.Twitter"] = new[]
                {
                    Lombiq.UIKit.FeatureIds.Base,
                    Lombiq.ChartJs.Constants.FeatureIds.Default,
                },
            })
        .EnableAutoSetupIfNotUITesting(configuration)
        .ConfigureSecurityDefaults(allowInlineStyle: true)); // Necessary because style attributes are used in the Blog theme.

var app = builder.Build();

app.UseStaticFiles();
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
