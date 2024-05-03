using Lombiq.Tests.UI.Extensions;
using Lombiq.UIKit.Tests.UI.Extensions;
using Shouldly;
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
                    validationResult =>
                    {
                        // Error filtering due to https://github.com/OrchardCMS/OrchardCore/issues/15222,
                        // can be removed once it is resolved.
                        var errors = validationResult.GetParsedErrors()
                            .Where(error =>
                                error.RuleId is not "prefer-native-element" and
                                    not "text-content" and
                                    not "no-redundant-role");
                        errors.ShouldBeEmpty(string.Join('\n', errors.Select(error => error.Message)));
                        return Task.CompletedTask;
                    });
}
