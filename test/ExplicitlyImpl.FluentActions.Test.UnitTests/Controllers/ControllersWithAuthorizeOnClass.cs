using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers
{
    [Authorize]
    public class ControllerWith1AuthorizeClassReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public string HandlerAction()
        {
            return "hello";
        }
    }

    [Authorize("CanSayHello")]
    public class ControllerWith1AuthorizeClassPolicyReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public string HandlerAction()
        {
            return "hello";
        }
    }

    [Authorize(Roles = "Admin")]
    public class ControllerWith1AuthorizeClassRolesReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public string HandlerAction()
        {
            return "hello";
        }
    }

    [Authorize(AuthenticationSchemes = "Scheme")]
    public class ControllerWith1AuthorizeClassAuthenticationSchemesReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public string HandlerAction()
        {
            return "hello";
        }
    }

    [Authorize(Policy = "CanSayHello", Roles = "Admin", AuthenticationSchemes = "Scheme")]
    public class ControllerWith1AuthorizeClassPolicyRolesAuthenticationSchemesReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public string HandlerAction()
        {
            return "hello";
        }
    }

    [Authorize]
    public class ControllerWith1AuthorizeClassReturnsViewResultAsync : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public async Task<ViewResult> HandlerAction()
        {
            await Task.Delay(1);
            return View("~/Path/To/ViewWithStringModel.cshtml", "hello");
        }
    }
}
