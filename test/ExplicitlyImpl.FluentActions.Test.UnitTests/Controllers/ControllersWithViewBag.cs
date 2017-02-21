using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers
{
    public class ControllerWithViewBagReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public string HandlerAction()
        {
            ViewBag.Foo = "bar";
            return (string)ViewBag.Foo;
        }
    }

    public class ControllerWithViewBagReturnsStringAsync : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public async Task<string> HandlerAction()
        {
            await Task.Delay(1);
            ViewBag.Foo = "bar";
            return (string)ViewBag.Foo;
        }
    }
}
