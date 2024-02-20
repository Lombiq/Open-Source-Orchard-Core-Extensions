using Atata.HtmlValidation;
using Lombiq.Tests.UI.Extensions;
using OpenQA.Selenium;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lombiq.OSOCE.NuGet.Tests.UI.Helpers;

public static class AssertHtmlAndBrowserErrorsHelper
{
    public static readonly Action<IEnumerable<LogEntry>> AssertBrowserLogIsEmpty =
        logEntries => logEntries.Where(
            logEntry => !logEntry.Message.ContainsOrdinalIgnoreCase("Prefer to use the native <button> element"));

    public static readonly Func<HtmlValidationResult, Task> AssertHtmlErrorsAreEmpty = async errors =>
    {
        var errorResult = (await errors.GetErrorsAsync())
            .Where(error => !error.ContainsOrdinalIgnoreCase("Prefer to use the native <button> element"));

        errorResult.ShouldBeEmpty();
    };
}
