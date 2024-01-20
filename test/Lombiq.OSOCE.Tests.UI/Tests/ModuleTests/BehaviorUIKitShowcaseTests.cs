using Lombiq.Tests.UI.Extensions;
using Lombiq.UIKit.Tests.UI.Extensions;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

public class BehaviorUIKitShowcaseTests(ITestOutputHelper testOutputHelper) : UITestBase(testOutputHelper)
{
    [Fact]
    public Task UIKitShowcasePageShouldBeCorrect()
        => ExecuteTestAfterSetupAsync(
            context => context.TestUIKitShowcaseBehaviorAsync(),
            configuration => configuration.HtmlValidationConfiguration.AssertHtmlValidationResultAsync =
                async validationResult =>
                {
                    var errors = (await validationResult.GetErrorsAsync())
                        .Where(error => !error.ContainsOrdinalIgnoreCase("Prefer to use the native <button> element"));
                    errors.ShouldBeEmpty();
                });
}
