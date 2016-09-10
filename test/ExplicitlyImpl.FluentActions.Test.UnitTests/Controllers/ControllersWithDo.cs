using Microsoft.AspNetCore.Mvc;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers
{
    public class ControllerWithDoReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public string HandlerAction()
        {
            return "baz";
        }
    }
}
