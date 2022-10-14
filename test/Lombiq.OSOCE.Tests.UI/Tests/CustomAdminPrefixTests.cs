using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests;

public class CustomAdminPrefixTests : UITestBase
{
    public CustomAdminPrefixTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Theory, Chrome]
    public Task NavigationWithCustomAdminPrefixShouldWork(Browser browser) =>
        ExecuteTestAfterSetupAsync(
            async context =>
            {
                context.AdminUrlPrefix = "custom-admin";

                await context.SignInDirectlyAsync();
                await context.GoToDashboardAsync();
                await context.GoToFeaturesPageAsync();
                await context.GoToContentItemListAsync("Blog");
                await context.GoToContentItemsPageAsync();
            },
            browser,
            configuration =>
            {
                configuration.HtmlValidationConfiguration.RunHtmlValidationAssertionOnAllPageChanges = false;
                configuration.OrchardCoreConfiguration.BeforeAppStart += (_, argsBuilder) =>
                {
                    argsBuilder.AddWithValue("OrchardCore:OrchardCore_Admin:AdminUrlPrefix", "custom-admin");

                    return Task.CompletedTask;
                };
            });
}
