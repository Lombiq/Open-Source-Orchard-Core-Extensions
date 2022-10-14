using Lombiq.Privacy.Tests.UI.Extensions;
using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using Lombiq.Tests.UI.Shortcuts;
using Shouldly;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests;

public class WorkflowShortcutsTests : UITestBase
{
    public WorkflowShortcutsTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Theory, Chrome]
    public Task GenerateHttpEventUrlShouldWork(Browser browser) =>
        ExecuteTestAfterSetupAsync(
            async context =>
            {
                await context.EnableFeatureDirectlyAsync(ShortcutsFeatureIds.Workflows);
                await context.ExecutePrivacySampleRecipeDirectlyAsync();
                (await context.GenerateHttpEventUrlAsync(
                    "registrationworkflow000000",
                    "registrationhttpevent00000"))
                    .ShouldStartWith("/workflows/Invoke?token=");
            },
            browser,
            configuration =>
                configuration.HtmlValidationConfiguration.RunHtmlValidationAssertionOnAllPageChanges = false);
}
