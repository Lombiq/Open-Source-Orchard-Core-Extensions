using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using Lombiq.Walkthroughs.Tests.UI.Extensions;
using OpenQA.Selenium;
using Shouldly;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.NuGet.Tests.UI.Tests.ModuleTests;

public class BehaviorWalkthroughsTests : UITestBase
{
    public BehaviorWalkthroughsTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task WalkthroughsShouldWorkCorrectly() =>
        ExecuteTestAsync(
            context => context.RunSetupAndTestWalkthroughsBehaviorAsync(),
            changeConfiguration: configuration =>
            {
                configuration.HtmlValidationConfiguration.HtmlValidationOptions =
                    configuration.HtmlValidationConfiguration.HtmlValidationOptions
                        .CloneWith(validationOptions => validationOptions.ConfigPath =
                            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BehaviorWalkthroughsTests.htmlvalidate.json"));

                // Once the linked issues are fixed, the custom browser log assertion can be removed completely.
                configuration.AssertBrowserLog = logEntries => logEntries.ShouldNotContain(
                    logEntry => IsValidLogEntry(logEntry),
                    logEntries.Where(IsValidLogEntry).ToFormattedString());
            });

    private static bool IsValidLogEntry(LogEntry logEntry) =>
        OrchardCoreUITestExecutorConfiguration.IsValidBrowserLogEntry(logEntry) &&
        // See https://github.com/OrchardCMS/OrchardCore/issues/15301.
        !(logEntry.Message.ContainsOrdinalIgnoreCase("/OrchardCore.Resources/Scripts/jquery.js?v=") &&
            logEntry.Message.ContainsOrdinalIgnoreCase("3128:6 Uncaught")) &&
        // See https://github.com/OrchardCMS/OrchardCore/issues/14598. This error has multiple variations, so targeting
        // the lowest common denominator with the file name.
        !logEntry.Message.ContainsOrdinalIgnoreCase("/monaco/IStandaloneEditorConstructionOptions.json");
}
