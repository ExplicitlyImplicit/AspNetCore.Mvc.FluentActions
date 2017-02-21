using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

    public class ControllerWithModelStateReturnsStringAsync : Controller
    {
        [HttpPost]
        [Route("/route/url")]
        public async Task<string> HandlerAction()
        {
            await Task.Delay(1);
            return $"Hello World!";
        }
    }
}
