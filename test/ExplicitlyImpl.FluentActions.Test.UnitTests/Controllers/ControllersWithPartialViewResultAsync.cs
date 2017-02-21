using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers
{
    public class ControllerWithNoUsingsXToReturnsPartialViewAsync : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public async Task<PartialViewResult> HandlerAction()
        {
            await Task.Delay(1);
            return PartialView("~/Path/To/PartialViewWithStringModel.cshtml", "Hello World!");
        }
    }

    public class ControllerWithNoUsingsXDoReturnsPartialViewAsync : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public async Task<PartialViewResult> HandlerAction()
        {
            await Task.Delay(1);
            return PartialView("~/Path/To/PartialViewWithoutModel.cshtml");
        }
    }

    public class ControllerWithNoUsings1Do1ToReturnsPartialViewAsync : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public async Task<PartialViewResult> HandlerAction()
        {
            await Task.Delay(1);
            return PartialView("~/Path/To/PartialViewWithStringModel.cshtml", "baz");
        }
    }

    public class ControllerWith1BodyReturnsPartialViewAsync : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public async Task<PartialViewResult> HandlerAction([FromBody]string name)
        {
            await Task.Delay(1);
            return PartialView("~/Path/To/PartialViewWithStringModel.cshtml", $"Hello {name}!");
        }
    }

    public class ControllerWith1Body1RouteParamReturnsPartialViewAsync : Controller
    {
        [HttpGet]
        [Route("/route/{lastName}")]
        public async Task<PartialViewResult> HandlerAction([FromBody]string firstName, [FromRoute]string lastName)
        {
            await Task.Delay(1);
            return PartialView("~/Path/To/PartialViewWithStringModel.cshtml", $"Hello {firstName} {lastName}!");
        }
    }

    public class ControllerWith1Body1RouteParam2ToReturnsPartialViewAsync : Controller
    {
        [HttpGet]
        [Route("/route/{lastName}")]
        public async Task<PartialViewResult> HandlerAction([FromBody]string firstName, [FromRoute]string lastName)
        {
            await Task.Delay(1);
            return PartialView("~/Path/To/PartialViewWithStringModel.cshtml", $"Hello {firstName} {lastName}!");
        }
    }
}
