using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers
{
    public class ControllerWithNoUsingsNoToReturnsView : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public ViewResult HandlerAction()
        {
            return View("~/Path/To/ViewWithoutModel.cshtml");
        }
    }

    public class ControllerWithNoUsingsXToReturnsView : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public ViewResult HandlerAction()
        {
            return View("~/Path/To/ViewWithStringModel.cshtml", "Hello World!");
        }
    }

    public class ControllerWithNoUsingsXDoReturnsView : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public ViewResult HandlerAction()
        {
            return View("~/Path/To/ViewWithoutModel.cshtml");
        }
    }

    public class ControllerWithNoUsings1Do1ToReturnsView : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public ViewResult HandlerAction()
        {
            return View("~/Path/To/ViewWithStringModel.cshtml", "baz");
        }
    }

    public class ControllerWith1BodyReturnsView : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public ViewResult HandlerAction([FromBody]string name)
        {
            return View("~/Path/To/ViewWithStringModel.cshtml", $"Hello {name}!");
        }
    }

    public class ControllerPassing1BodyReturnsView : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public ViewResult HandlerAction([FromBody]string model)
        {
            return View("~/Path/To/ViewWithStringModel.cshtml", model);
        }
    }

    public class ControllerWith1Body1RouteParamPassing1BodyReturnsView : Controller
    {
        [HttpGet]
        [Route("/route/url/{unused}")]
        public ViewResult HandlerAction([FromBody]string model, [FromRoute]string unused)
        {
            return View("~/Path/To/ViewWithStringModel.cshtml", model);
        }
    }

    public class ControllerWith1Body1RouteParamReturnsView : Controller
    {
        [HttpGet]
        [Route("/route/{lastName}")]
        public ViewResult HandlerAction([FromBody]string firstName, [FromRoute]string lastName)
        {
            return View("~/Path/To/ViewWithStringModel.cshtml", $"Hello {firstName} {lastName}!");
        }
    }

    public class ControllerWith1Body1RouteParam2ToReturnsView : Controller
    {
        [HttpGet]
        [Route("/route/{lastName}")]
        public ViewResult HandlerAction([FromBody]string firstName, [FromRoute]string lastName)
        {
            return View("~/Path/To/ViewWithStringModel.cshtml", $"Hello {firstName} {lastName}!");
        }
    }
}
