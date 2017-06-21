using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers
{
    public class ControllerWith1AllowAnonymousReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        [AllowAnonymous]
        public string HandlerAction()
        {
            return "hello";
        }
    }

    public class ControllerWith1AllowAnonymousReturnsViewResultAsync : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        [AllowAnonymous]
        public async Task<ViewResult> HandlerAction()
        {
            await Task.Delay(1);
            return View("~/Path/To/ViewWithStringModel.cshtml", "hello");
        }
    }
}
