using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers
{
    public class ControllerWithNoUsingsNoToReturnsViewComponent : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public ViewComponentResult HandlerAction()
        {
            return ViewComponent("ViewComponentWithoutModel");
        }
    }

    public class ControllerWithNoUsingsXToReturnsViewComponent : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public ViewComponentResult HandlerAction()
        {
            return ViewComponent("ViewComponentWithStringModel", "Hello World!");
        }
    }

    public class ControllerWithNoUsingsXDoReturnsViewComponent : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public ViewComponentResult HandlerAction()
        {
            return ViewComponent("ViewComponentWithoutModel");
        }
    }

    public class ControllerWithNoUsings1Do1ToReturnsViewComponent : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public ViewComponentResult HandlerAction()
        {
            return ViewComponent("ViewComponentWithStringModel", "baz");
        }
    }

    public class ControllerWith1BodyReturnsViewComponent : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public ViewComponentResult HandlerAction([FromBody]string name)
        {
            return ViewComponent("ViewComponentWithStringModel", $"Hello {name}!");
        }
    }

    public class ControllerWith1Body1RouteParamReturnsViewComponent : Controller
    {
        [HttpGet]
        [Route("/route/{lastName}")]
        public ViewComponentResult HandlerAction([FromBody]string firstName, [FromRoute]string lastName)
        {
            return ViewComponent("ViewComponentWithStringModel", $"Hello {firstName} {lastName}!");
        }
    }

    public class ControllerWith1Body1RouteParam2ToReturnsViewComponent : Controller
    {
        [HttpGet]
        [Route("/route/{lastName}")]
        public ViewComponentResult HandlerAction([FromBody]string firstName, [FromRoute]string lastName)
        {
            return ViewComponent("ViewComponentWithStringModel", $"Hello {firstName} {lastName}!");
        }
    }
}
