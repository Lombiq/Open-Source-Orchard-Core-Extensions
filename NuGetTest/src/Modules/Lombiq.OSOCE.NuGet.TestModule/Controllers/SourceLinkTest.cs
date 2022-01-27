using Lombiq.HelpfulLibraries.Libraries.Users;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Lombiq.OSOCE.NuGet.TestModule.Controllers
{
    public class SourceLinkTestController : Controller
    {
        private readonly ICachingUserManager _cachingUserManager;

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
