using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers
{
    public class ControllerWithNoUsingsXToReturnsViewAsync : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public async Task<ViewResult> HandlerAction()
        {
            await Task.Delay(1);
            return View("~/Path/To/ViewWithStringModel.cshtml", "Hello World!");
        }
    }

    public class ControllerWithNoUsingsXDoReturnsViewAsync : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public async Task<ViewResult> HandlerAction()
        {
            await Task.Delay(1);
            return View("~/Path/To/ViewWithoutModel.cshtml");
        }
    }

    public class ControllerWithNoUsings1Do1ToReturnsViewAsync : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public async Task<ViewResult> HandlerAction()
        {
            await Task.Delay(1);
            return View("~/Path/To/ViewWithStringModel.cshtml", "baz");
        }
    }

    public class ControllerWith1BodyReturnsViewAsync : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public async Task<ViewResult> HandlerAction([FromBody]string name)
        {
            await Task.Delay(1);
            return View("~/Path/To/ViewWithStringModel.cshtml", $"Hello {name}!");
        }
    }

    public class ControllerWith1Body1RouteParamReturnsViewAsync : Controller
    {
        [HttpGet]
        [Route("/route/{lastName}")]
        public async Task<ViewResult> HandlerAction([FromBody]string firstName, [FromRoute]string lastName)
        {
            await Task.Delay(1);
            return View("~/Path/To/ViewWithStringModel.cshtml", $"Hello {firstName} {lastName}!");
        }
    }

    public class ControllerWith1Body1RouteParam2ToReturnsViewAsync : Controller
    {
        [HttpGet]
        [Route("/route/{lastName}")]
        public async Task<ViewResult> HandlerAction([FromBody]string firstName, [FromRoute]string lastName)
        {
            await Task.Delay(1);
            return View("~/Path/To/ViewWithStringModel.cshtml", $"Hello {firstName} {lastName}!");
        }
    }
}
