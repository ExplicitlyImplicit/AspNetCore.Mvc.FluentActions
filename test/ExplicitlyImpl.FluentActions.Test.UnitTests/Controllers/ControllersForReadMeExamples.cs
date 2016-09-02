using Microsoft.AspNetCore.Mvc;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers
{
    public class ControllerForReadMeExample1 : Controller
    {
        [HttpGet]
        [Route("/")]
        public string HandlerAction()
        {
            return $"Hello World!";
        }
    }

    public class ControllerForReadMeExample2 : Controller
    {
        [HttpGet]
        [Route("/users/{userId}")]
        public ViewResult HandlerAction([FromServices]IUserService userService, [FromRoute]int userId)
        {
            var user = userService.GetUserById(userId);
            return View("~/Views/Users/DisplayUser.cshtml", user);
        }
    }
}
