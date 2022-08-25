using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using Shouldly;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests;

public class IdleTenantTests : UITestBase
{
    public IdleTenantTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Theory, Chrome]
    public Task ShuttingDownIdleTenantsShouldWork(Browser browser) =>
        ExecuteTestAfterSetupAsync(
            context =>
            {
                // We are letting the site to sit idle for more than a minute so that the
                // tenant could be shut down by the background task.
                System.Threading.Thread.Sleep(71000);

                context.Configuration.AssertAppLogsAsync = async webApplicationInstance =>
                {
                    var webAppInstanceLog = await webApplicationInstance.GetLogOutputAsync();

                    webAppInstanceLog.ShouldContain(
                        "Shutting down tenant \"Default\" because of idle timeout");
                };
            },
            browser,
            configuration =>
                configuration.OrchardCoreConfiguration.BeforeAppStart += (_, argumentsBuilder) =>
                {
                    argumentsBuilder
                        .Add("--OrchardCore:Lombiq_Hosting_Tenants_IdleTenantManagement:IdleMinutesOptions:MaxIdleMinutes")
                        .Add("1");

                    return Task.CompletedTask;
                }
            );
}
