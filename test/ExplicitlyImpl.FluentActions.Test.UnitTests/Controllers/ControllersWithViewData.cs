using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers
{
    public class ControllerWithViewDataReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public string HandlerAction()
        {
            ViewData["foo"] = "bar";
            return (string) ViewData["foo"];
        }
    }

    public class ControllerWithViewDataReturnsStringAsync : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public async Task<string> HandlerAction()
        {
            await Task.Delay(1);
            ViewData["foo"] = "bar";
            return (string)ViewData["foo"];
        }
    }
}
