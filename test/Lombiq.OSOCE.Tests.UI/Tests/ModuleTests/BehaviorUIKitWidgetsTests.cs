using Lombiq.Tests.UI.Extensions;
using Lombiq.UIKit.Widgets.Tests.UI.Extensions;
using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests.ModuleTests;

public class BehaviorUIKitWidgetsTests : UITestBase
{
    public BehaviorUIKitWidgetsTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Fact]
    public Task UIKitCarouselWidgetShouldHaveSlickContainer()
        => ExecuteTestAfterSetupAsync(
            context => context.TestCarouselWidgetBehavior(),
            configuration => configuration.HtmlValidationConfiguration.AssertHtmlValidationResultAsync =
                validationResult =>
                {
                    configuration.HtmlValidationConfiguration.WithRelativeConfigPath("NoUniqueLandmark.htmlvalidate.json");

                    // Error filtering due to https://github.com/OrchardCMS/OrchardCore/issues/15222, can be removed
                    // once it is resolved.
                    var errors = validationResult.GetParsedErrors()
                        .Where(error =>
                            error.RuleId is not "prefer-native-element" and
                                not "text-content" and
                                not "no-redundant-role");
                    errors.ShouldBeEmpty(HtmlValidationResultExtensions.GetParsedErrorMessageString(errors));
                    return Task.CompletedTask;
                });

    [Fact]
    public Task CarouselWidgetPartSettingsHasJsonEditorForOptionsAndOptionsAreUsed()
        => ExecuteTestAfterSetupAsync(
            context => context.TestCarouselWidgetOptionsAsync(),
            configuration => configuration.HtmlValidationConfiguration.AssertHtmlValidationResultAsync =
                validationResult =>
                {
                    configuration.HtmlValidationConfiguration.WithRelativeConfigPath("NoUniqueLandmark.htmlvalidate.json");

                    // Error filtering due to https://github.com/OrchardCMS/OrchardCore/issues/15222, can be removed
                    // once it is resolved.
                    var errors = validationResult.GetParsedErrors()
                        .Where(error =>
                            error.RuleId is not "prefer-native-element" and
                                not "text-content" and
                                not "no-redundant-role");
                    errors.ShouldBeEmpty(HtmlValidationResultExtensions.GetParsedErrorMessageString(errors));
                    return Task.CompletedTask;
                });
}
