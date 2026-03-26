using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.NuGet.Tests;

public class TailwindTargetsTests
{
    [Fact]
    public async Task GeneratedCssShouldBeServedFromTheNuGetConsumedTestTheme()
    {
        await using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();

        using var response = await client.GetAsync("/Lombiq.OSOCE.NuGet.TestTheme/css/tailwind-site.css");

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
}
