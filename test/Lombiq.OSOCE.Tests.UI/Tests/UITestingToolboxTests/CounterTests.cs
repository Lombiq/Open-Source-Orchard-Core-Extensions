using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Exceptions;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using Shouldly;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.UITestingToolboxTests;

public class CounterTests : UITestBase
{
    public CounterTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Theory, Chrome]
    public Task RepeatedSqlQueryDuringRunningPhaseShouldThrow(Browser browser) =>
        Should.ThrowAsync<CounterThresholdException>(() =>
            ExecuteTestAfterSetupAsync(
                context => context.SignInDirectlyAndGoToHomepageAsync(),
                browser,
                configuration =>
                {
                    // The test is guaranteed to fail so we don't want to retry it needlessly.
                    configuration.MaxRetryCount = 0;

                    configuration.CounterConfiguration.Running.SessionThreshold.DbCommandExecutionThreshold = 0;

                    return Task.CompletedTask;
                }));

    [Theory, Chrome]
    public Task DbReaderReadDuringRunningPhaseShouldThrow(Browser browser) =>
        Should.ThrowAsync<CounterThresholdException>(() =>
            ExecuteTestAfterSetupAsync(
                async context => await context.GoToHomePageAsync(onlyIfNotAlreadyThere: false),
                browser,
                configuration =>
                {
                    // The test is guaranteed to fail so we don't want to retry it needlessly.
                    configuration.MaxRetryCount = 0;

                    configuration.CounterConfiguration.Running.NavigationThreshold.DbReaderReadThreshold = 0;

                    return Task.CompletedTask;
                }));
}
