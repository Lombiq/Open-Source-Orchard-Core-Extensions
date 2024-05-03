using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using Lombiq.Walkthroughs.Tests.UI.Extensions;
using OpenQA.Selenium;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

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
            // Could be removed if https://github.com/shepherd-pro/shepherd/issues/2555 is fixed.
            changeConfiguration: configuration =>
            {
                // Could be removed if https://github.com/shepherd-pro/shepherd/issues/2555 is fixed.
                configuration.HtmlValidationConfiguration.AssertHtmlValidationResultAsync =
                    validationResult =>
                    {
                        // Error filtering due to https://github.com/OrchardCMS/OrchardCore/issues/15222,
                        // can be removed once it is resolved.
                        var errors = validationResult.GetParsedErrors()
                            .Where(error => error.RuleId is not "no-implicit-button-type");
                        errors.ShouldBeEmpty(string.Join('\n', errors.Select(error => error.Message)));
                        return Task.CompletedTask;
                    };

                // Once the linked issues are fixed, the custom browser log assertion can be removed completely.
                configuration.AssertBrowserLog = logEntries => logEntries.ShouldNotContain(
                    logEntry => IsValidLogEntry(logEntry),
                    logEntries.Where(IsValidLogEntry).ToFormattedString());
            });

    private static bool IsValidLogEntry(LogEntry logEntry) =>
        OrchardCoreUITestExecutorConfiguration.IsValidBrowserLogEntry(logEntry) &&
        // See https://github.com/OrchardCMS/OrchardCore/issues/15301.
        !(logEntry.Message.ContainsOrdinalIgnoreCase("/OrchardCore.Resources/Scripts/jquery.js?v=") &&
            logEntry.Message.ContainsOrdinalIgnoreCase("Uncaught")) &&
        // See https://github.com/OrchardCMS/OrchardCore/issues/14598. This error has multiple variations, so targeting
        // the lowest common denominator with the file name.
        !logEntry.Message.ContainsOrdinalIgnoreCase("/monaco/IStandaloneEditorConstructionOptions.json");
}
