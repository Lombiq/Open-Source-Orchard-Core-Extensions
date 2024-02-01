using Lombiq.Tests.UI.Extensions;
using Lombiq.UIKit.Tests.UI.Extensions;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

public class BehaviorUIKitShowcaseTests : UITestBase
{
    public BehaviorUIKitShowcaseTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task UIKitShowcasePageShouldBeCorrect()
        => ExecuteTestAfterSetupAsync(
            context => context.TestUIKitShowcaseBehaviorAsync(),
            configuration => configuration.HtmlValidationConfiguration.AssertHtmlValidationResultAsync =
                async validationResult =>
                {
                    // Error filtering due to https://github.com/OrchardCMS/OrchardCore/issues/15222,
                    // can be removed once it is resolved.
                    var errors = (await validationResult.GetErrorsAsync())
                        .Where(error =>
                        !error.ContainsOrdinalIgnoreCase("Prefer to use the native <button> element") &&
                        !error.ContainsOrdinalIgnoreCase("<button> must have accessible text") &&
                        !error.ContainsOrdinalIgnoreCase("Redundant role \"button\" on <button>"));
                    errors.ShouldBeEmpty();
                });
}
