using Microsoft.AspNetCore.Mvc;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers
{
    public class ControllerForHandlerWithoutUsingsAndReturnsString : Controller
    {
        [HttpGet]
        [Route("/")]
        public string HandlerAction()
        {
            return "Hello";
        }
    }
}
