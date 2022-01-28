using Lombiq.HelpfulLibraries.Libraries.Users;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Lombiq.OSOCE.NuGet.TestModule.Controllers
{
    public class SourceLinkTestController : Controller
    {
        private readonly ICachingUserManager _cachingUserManager;

        // Since the Analyzers NuGet package is added, you can see that it works like this: Disable the IDE0021 rule in
        // the root Analyzers OrchardCore.ruleset file, then change this constructor to use a block body. You should
        // get an analyzer violation since the expression-bodied rule is still enforced from the package
        public SourceLinkTestController(ICachingUserManager cachingUserManager) => _cachingUserManager = cachingUserManager;

        // /Lombiq.OSOCE.NuGet.TestModule/SourceLinkTest/Index
        public async Task<string> Index()
        {
            // If you step into this from a debugger session you can check if Source Link works and symbols have been
            // loaded. You should be able to step into the original source.
            var adminUser = await _cachingUserManager.GetUserByNameAsync("admin");
            return adminUser?.UserName ?? "No user found.";
        }
    }
}
