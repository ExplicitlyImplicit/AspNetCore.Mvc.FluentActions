using Microsoft.AspNetCore.Mvc;

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
}
