using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using Shouldly;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Lombiq.OSOCE.NuGet.Tests.UI.Helpers;

public static class AssertAppLogsHelpers
{
    public static readonly Func<IWebApplicationInstance, Task> AssertOsoceAppLogsAreEmptyAsync = app =>
        app.OsoceLogsShouldBeEmptyAsync();

    public static async Task OsoceLogsShouldBeEmptyAsync(
        this IWebApplicationInstance webApplicationInstance,
        bool canContainWarnings = false,
        CancellationToken cancellationToken = default)
    {
        if (cancellationToken == default) cancellationToken = CancellationToken.None;

        var logOutput = await webApplicationInstance.GetLogOutputAsync(cancellationToken);

        if (!string.IsNullOrEmpty(logOutput))
        {
            var messages = logOutput.SplitByNewLines().ToList();

            // Temporarily filtering out irrelevant cache errors from logs.
            var filteredLogOutput = messages.Where(message =>
                !message.Contains("orchard-log-") &&
                !message.Contains("Microsoft.Hosting.Lifetime|INFO|") &&
                !string.IsNullOrEmpty(message) &&
                !message.Contains("System.IO") &&
                !message.Contains("Azure Media Storage is enabled but not active because the 'ContainerName' is missing or empty") &&
                !message.Contains("Azure Media Storage is enabled but not active because the 'ConnectionString' is missing") &&
                !message.Contains("OrchardCore.Media.Core.DefaultMediaFileStoreCacheFileProvider.TryDeleteDirectoryAsync") &&
                !message.Contains("OrchardCore.Media.Core.DefaultMediaFileStoreCacheFileProvider|ERROR|Error deleting cache folder"));

            filteredLogOutput.ShouldBeEmpty();

            return;
        }

        if (canContainWarnings)
        {
            logOutput.ShouldNotContain("|ERROR|");
            logOutput.ShouldNotContain("|FATAL|");
        }
        else
        {
            logOutput.ShouldBeEmpty();
        }
    }
}
