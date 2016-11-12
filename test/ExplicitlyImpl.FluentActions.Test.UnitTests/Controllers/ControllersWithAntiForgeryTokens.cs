using Microsoft.AspNetCore.Mvc;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers
{
    public class ControllerWithAntiForgeryTokenReturnsString : Controller
    {
        [HttpPost]
        [Route("/route/url")]
        [ValidateAntiForgeryToken]
        public string HandlerAction()
        {
            return $"Hello World!";
        }
    }
}
