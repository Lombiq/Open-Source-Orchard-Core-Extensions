using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using OpenQA.Selenium;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.NuGet.Tests.UI.Tests;

public class BasicTests : UITestBase
{
    public BasicTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Theory, Chrome]
    public Task AnonymousHomePageShouldExist(Browser browser) =>
        ExecuteTestAfterSetupAsync(
            context => context.Exists(By.ClassName("navbar-brand")),
            browser,
            configuration => configuration.UseSqlServer = true);
}
