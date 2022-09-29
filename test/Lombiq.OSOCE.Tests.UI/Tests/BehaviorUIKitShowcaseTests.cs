﻿using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using Lombiq.UIKit.Tests.UI.Extensions;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests;

public class BehaviorUIKitShowcaseTests : UITestBase
{
    public BehaviorUIKitShowcaseTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Theory, Chrome]
    public Task UIKitShowcasePageShouldBeCorrect(Browser browser)
        => ExecuteTestAfterSetupAsync(
            context => context.TestUIKitShowcaseBehaviorAsync(),
            browser,
            configuration => configuration.HtmlValidationConfiguration.AssertHtmlValidationResultAsync =
                async validationResult =>
                {
                    var errors = (await validationResult.GetErrorsAsync())
                        .Where(error => !error.ContainsOrdinalIgnoreCase("Prefer to use the native <button> element"));
                    errors.ShouldBeEmpty();
                });
}
