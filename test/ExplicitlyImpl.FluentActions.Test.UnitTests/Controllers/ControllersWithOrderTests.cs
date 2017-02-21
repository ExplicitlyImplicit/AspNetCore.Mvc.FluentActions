using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers
{
    public class ControllerForOrderTestsReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public string HandlerAction()
        {
            return "Hello";
        }
    }

    public class ControllerForOrderTestsReturnsStringAsync : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public async Task<string> HandlerAction()
        {
            await Task.Delay(1);
            return "Hello";
        }
    }
}
