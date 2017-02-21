using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

    public class ControllerWithAntiForgeryTokenReturnsStringAsync : Controller
    {
        [HttpPost]
        [Route("/route/url")]
        [ValidateAntiForgeryToken]
        public async Task<string> HandlerAction()
        {
            await Task.Delay(1);
            return $"Hello World!";
        }
    }
}
