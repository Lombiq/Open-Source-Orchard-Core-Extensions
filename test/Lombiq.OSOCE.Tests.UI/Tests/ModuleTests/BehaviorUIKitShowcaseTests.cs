using Lombiq.Tests.UI.Extensions;
using Lombiq.UIKit.Tests.UI.Extensions;
using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

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
                validationResult =>
                {
                    configuration.HtmlValidationConfiguration.WithRelativeConfigPath("NoUniqueLandmark.htmlvalidate.json");

                    // The first three rule exclusions due to https://github.com/OrchardCMS/OrchardCore/issues/15222,
                    // can be removed once it is resolved.
                    // "aria-label-misuse" due to https://github.com/OrchardCMS/OrchardCore/issues/18510.
                    var errors = validationResult.GetParsedErrors()
                        .Where(error =>
                            error.RuleId is not "prefer-native-element" and
                                not "text-content" and
                                not "no-redundant-role" and
                                not "aria-label-misuse")
                        .ToList();
                    errors.ShouldBeEmpty(HtmlValidationResultExtensions.GetParsedErrorMessageString(errors));
                    return Task.CompletedTask;
                });
}
