using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers
{
    public class ControllerWithTempDataReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public string HandlerAction()
        {
            TempData["foo"] = "bar";
            return (string) TempData["foo"];
        }
    }

    public class ControllerWithTempDataReturnsStringAsync : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public async Task<string> HandlerAction()
        {
            await Task.Delay(1);
            TempData["foo"] = "bar";
            return (string)TempData["foo"];
        }
    }
}
