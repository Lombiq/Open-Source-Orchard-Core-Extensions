using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using Newtonsoft.Json;
using OpenQA.Selenium;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests;

public class BehaviorHelpfulLibrariesTests : UITestBase
{
    public BehaviorHelpfulLibrariesTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Theory, Chrome]
    public Task SafeJsonShouldWork(Browser browser) =>
        ExecuteTestAfterSetupAsync(
            async context =>
            {
                async Task CheckButton(By button, bool isError)
                {
                    await context.ClickReliablyOnAsync(button);
                    context.Exists(By.CssSelector("#status.status-ready"));

                    var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(
                        context.Get(By.Id("output")).Text);
                    json.ShouldNotBeNull();

                    if (isError)
                    {
                        json.ShouldContainKey("error");
                        json["error"].ShouldBe("Intentional failure.");

                        json.ShouldContainKey("data");
                        json["data"].ToString()?.StartsWithOrdinal("System.InvalidOperationException:").ShouldBeTrue();
                    }
                    else
                    {
                        json.ShouldNotContainKey("error");

                        var expected = new
                        {
                            Foo = "bar",
                            This = true,
                            That = 10,
                        };

                        var resolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() };
                        var settings = new JsonSerializerSettings
                        {
                            ContractResolver = resolver,
                            Formatting = Formatting.Indented,
                        };

                        JsonConvert.SerializeObject(json, settings)
                            .ShouldBe(JsonConvert.SerializeObject(expected, settings));
                    }
                }

                await context.GoToRelativeUrlAsync("/Lombiq.HelpfulLibraries.Samples/Error/Json");
                await CheckButton(By.Id("error"), isError: true);
                await CheckButton(By.Id("error-await"), isError: true);
                await CheckButton(By.Id("success"), isError: false);
            },
            browser);
}
