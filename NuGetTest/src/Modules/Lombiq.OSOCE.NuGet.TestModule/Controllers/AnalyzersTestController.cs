using Microsoft.AspNetCore.Mvc;
using OrchardCore.Modules;
using System.Globalization;

namespace Lombiq.OSOCE.NuGet.TestModule.Controllers;

public class AnalyzersTestController : Controller
{
    private readonly IClock _clock;

    // Since the Analyzers NuGet package is added, you can see that it works like this: Disable the IDE0021 rule in
    // the root Analyzers OrchardCore.ruleset file, then change this constructor to use a block body. You should
    // get an analyzer violation since the expression-bodied rule is still enforced from the package
    public AnalyzersTestController(IClock clock) => _clock = clock;

    // /Lombiq.OSOCE.NuGet.TestModule/AnalyzersTest/Index
    public string Index() => _clock.UtcNow.ToString(CultureInfo.InvariantCulture);
}
