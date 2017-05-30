using Microsoft.AspNetCore.Mvc;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers
{
    public class ViewComponentWithoutModel : ViewComponent
    {

    }

    public class ViewComponentWithStringModel : ViewComponent
    {
        public IViewComponentResult Invoke(string model)
        {
            return View((object)model);
        }
    }
    
    public class ControllerWithNoUsingsNoToReturnsViewComponentUsingType : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public ViewComponentResult HandlerAction()
        {
            return ViewComponent(typeof(ViewComponentWithoutModel));
        }
    }

    public class ControllerWithNoUsingsXToReturnsViewComponentUsingType : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public ViewComponentResult HandlerAction()
        {
            return ViewComponent(typeof(ViewComponentWithStringModel), "Hello World!");
        }
    }

    public class ControllerWithNoUsingsXDoReturnsViewComponentUsingType : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public ViewComponentResult HandlerAction()
        {
            return ViewComponent(typeof(ViewComponentWithoutModel));
        }
    }

    public class ControllerWithNoUsings1Do1ToReturnsViewComponentUsingType : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public ViewComponentResult HandlerAction()
        {
            return ViewComponent(typeof(ViewComponentWithStringModel), "baz");
        }
    }

    public class ControllerWith1BodyReturnsViewComponentUsingType : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public ViewComponentResult HandlerAction([FromBody]string name)
        {
            return ViewComponent(typeof(ViewComponentWithStringModel), $"Hello {name}!");
        }
    }

    public class ControllerWith1Body1RouteParamReturnsViewComponentUsingType : Controller
    {
        [HttpGet]
        [Route("/route/{lastName}")]
        public ViewComponentResult HandlerAction([FromBody]string firstName, [FromRoute]string lastName)
        {
            return ViewComponent(typeof(ViewComponentWithStringModel), $"Hello {firstName} {lastName}!");
        }
    }

    public class ControllerWith1Body1RouteParam2ToReturnsViewComponentUsingType : Controller
    {
        [HttpGet]
        [Route("/route/{lastName}")]
        public ViewComponentResult HandlerAction([FromBody]string firstName, [FromRoute]string lastName)
        {
            return ViewComponent(typeof(ViewComponentWithStringModel), $"Hello {firstName} {lastName}!");
        }
    }
}
