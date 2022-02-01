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
            // If you step into this call from a debugger session you can check if Source Link works and symbols have
            // been loaded. You should be able to step into the original source. For this to work, you'll need to
            // untick "Enable Just My Code" in Visual Studio settings under Tools -> Options -> Debugging -> General
            // and make sure "Enable Source Link support" is checked. For more info see:
            // https://devblogs.microsoft.com/dotnet/improving-debug-time-productivity-with-source-link/#enabling-source-link.
            // Let symbol loading finish, which might take several minutes (though you can speed it up by just loading
            // symbols for "Lombiq.HelpfulLibraries.dll" under Debugging -> Symbols -> Load only specified modules).
            var adminUser = await _cachingUserManager.GetUserByNameAsync("admin");
            return adminUser?.UserName ?? "No user found.";
        }
    }
}
