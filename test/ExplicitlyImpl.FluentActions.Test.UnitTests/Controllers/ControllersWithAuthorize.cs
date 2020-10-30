using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers
{
    public class ControllerWith1AuthorizeReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        [Authorize]
        public string HandlerAction()
        {
            return "hello";
        }
    }

    public class ControllerWith1AuthorizePolicyReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        [Authorize("CanSayHello")]
        public string HandlerAction()
        {
            return "hello";
        }
    }

    public class ControllerWith1AuthorizeRolesReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        [Authorize(Roles = "Admin")]
        public string HandlerAction()
        {
            return "hello";
        }
    }

    public class ControllerWith1AuthorizeAuthenticationSchemesReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        [Authorize(AuthenticationSchemes = "Scheme")]
        public string HandlerAction()
        {
            return "hello";
        }
    }

    public class ControllerWith1AuthorizePolicyRolesAuthenticationSchemesReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        [Authorize(Policy = "CanSayHello", Roles = "Admin", AuthenticationSchemes = "Scheme")]
        public string HandlerAction()
        {
            return "hello";
        }
    }

    public class ControllerWith1AuthorizeReturnsViewResultAsync : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        [Authorize]
        public async Task<ViewResult> HandlerAction()
        {
            await Task.Delay(1);
            return View("~/Path/To/ViewWithStringModel.cshtml", "hello");
        }
    }
}
