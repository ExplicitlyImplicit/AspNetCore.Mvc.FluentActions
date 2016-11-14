using Microsoft.AspNetCore.Mvc;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers
{
    public class ControllerWithModelStateReturnsString : Controller
    {
        [HttpPost]
        [Route("/route/url")]
        public string HandlerAction()
        {
            return $"Hello World!";
        }
    }
}
