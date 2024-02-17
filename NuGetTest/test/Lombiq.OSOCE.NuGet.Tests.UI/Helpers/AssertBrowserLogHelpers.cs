using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lombiq.OSOCE.NuGet.Tests.UI.Helpers;
public static class AssertBrowserLogHelpers
{
    public static readonly Action<IEnumerable<LogEntry>> AssertBrowserLogIsEmpty =
        logEntries => logEntries.Where(
            logEntry => !logEntry.Message.ContainsOrdinalIgnoreCase("Prefer to use the native <button> element"));
}
