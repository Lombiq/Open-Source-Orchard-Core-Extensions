using Microsoft.AspNetCore.Mvc;

namespace Lombiq.OSOCE.Samples.Controllers
{
    public class TypedRouteController : Controller
    {
        public ActionResult Index() => View();

        public ActionResult TypedRouteSample(string text, int number) => Content($"{text}: {number}");
    }
}
