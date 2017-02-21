using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers
{
    public class ControllerWithNoUsingsXToReturnsViewComponentAsync : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public async Task<ViewComponentResult> HandlerAction()
        {
            await Task.Delay(1);
            return ViewComponent("ViewComponentWithStringModel", "Hello World!");
        }
    }

    public class ControllerWithNoUsingsXDoReturnsViewComponentAsync : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public async Task<ViewComponentResult> HandlerAction()
        {
            await Task.Delay(1);
            return ViewComponent("ViewComponentWithoutModel");
        }
    }

    public class ControllerWithNoUsings1Do1ToReturnsViewComponentAsync : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public async Task<ViewComponentResult> HandlerAction()
        {
            await Task.Delay(1);
            return ViewComponent("ViewComponentWithStringModel", "baz");
        }
    }

    public class ControllerWith1BodyReturnsViewComponentAsync : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public async Task<ViewComponentResult> HandlerAction([FromBody]string name)
        {
            await Task.Delay(1);
            return ViewComponent("ViewComponentWithStringModel", $"Hello {name}!");
        }
    }

    public class ControllerWith1Body1RouteParamReturnsViewComponentAsync : Controller
    {
        [HttpGet]
        [Route("/route/{lastName}")]
        public async Task<ViewComponentResult> HandlerAction([FromBody]string firstName, [FromRoute]string lastName)
        {
            await Task.Delay(1);
            return ViewComponent("ViewComponentWithStringModel", $"Hello {firstName} {lastName}!");
        }
    }

    public class ControllerWith1Body1RouteParam2ToReturnsViewComponentAsync : Controller
    {
        [HttpGet]
        [Route("/route/{lastName}")]
        public async Task<ViewComponentResult> HandlerAction([FromBody]string firstName, [FromRoute]string lastName)
        {
            await Task.Delay(1);
            return ViewComponent("ViewComponentWithStringModel", $"Hello {firstName} {lastName}!");
        }
    }
}
