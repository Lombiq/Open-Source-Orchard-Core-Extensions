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
            async context =>
            {
                System.Threading.Thread.Sleep(71000);

                await context.SignInDirectlyAsync();
                await context.GoToDashboardAsync();

                context.Configuration.AssertAppLogsAsync = async webApplicationInstance =>
                {
                    var webAppInstanceLog = await webApplicationInstance.GetLogOutputAsync();

                    webAppInstanceLog.ShouldContain(
                        "Shutting down tenant \"Default\" because of idle timeout");

                    webAppInstanceLog.ShouldContain(
                        "Creating shell context for tenant 'Default'");
                };
            },
            browser,
            configuration =>
            {
                configuration.OrchardCoreConfiguration.BeforeAppStart +=
                    (_, argumentsBuilder) =>
                    {
                        argumentsBuilder
                            .Add("--OrchardCore:Lombiq_Hosting_Tenants_IdleTenantManagement:IdleMinutesOptions")
                            .Add("1");

                        return Task.CompletedTask;
                    };
            });
}
