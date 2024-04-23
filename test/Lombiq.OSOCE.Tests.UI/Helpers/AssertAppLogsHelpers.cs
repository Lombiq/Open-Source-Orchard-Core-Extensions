using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Lombiq.OSOCE.Tests.UI.Helpers;

public static class AssertAppLogsHelpers
{
    public static readonly Func<IWebApplicationInstance, Task> AssertOsoceAppLogsAreEmptyAsync = app =>
        app.OsoceLogsShouldBeEmptyAsync();

    public static async Task OsoceLogsShouldBeEmptyAsync(
        this IWebApplicationInstance webApplicationInstance,
        CancellationToken cancellationToken = default)
    {
        var logOutput = await webApplicationInstance.GetLogOutputAsync(cancellationToken);

        if (!string.IsNullOrEmpty(logOutput))
        {
            var messages = logOutput.SplitByNewLines().ToList();

            // Temporarily filtering out irrelevant cache errors from logs.
            var filteredLogOutput = messages.Where(message =>
                !message.Contains("|Lombiq.TrainingDemo.Services.DemoBackgroundTask|ERROR|Expected non-error") &&
                !message.Contains("OrchardCore.Media.Core.DefaultMediaFileStoreCacheFileProvider|ERROR|Error deleting cache folder"));

            var errors = filteredLogOutput.Where(item => item.Contains("|ERROR|") || item.Contains("|FATAL|"));
            errors.ShouldBeEmpty();
        }
    }
}
