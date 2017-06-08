using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers
{
    public class ControllerWithNoUsingsNoToReturnsPartialView : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public PartialViewResult HandlerAction()
        {
            return PartialView("~/Path/To/PartialViewWithoutModel.cshtml");
        }
    }

    public class ControllerPassing1BodyReturnsPartialView : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public PartialViewResult HandlerAction([FromBody]string model)
        {
            return PartialView("~/Path/To/PartialViewWithStringModel.cshtml", model);
        }
    }

    public class ControllerWith1Body1RouteParamPassing1BodyReturnsPartialView : Controller
    {
        [HttpGet]
        [Route("/route/url/{unused}")]
        public PartialViewResult HandlerAction([FromBody]string model, [FromRoute]string unused)
        {
            return PartialView("~/Path/To/PartialViewWithStringModel.cshtml", model);
        }
    }

    public class ControllerWithNoUsingsXToReturnsPartialView : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public PartialViewResult HandlerAction()
        {
            return PartialView("~/Path/To/PartialViewWithStringModel.cshtml", "Hello World!");
        }
    }

    public class ControllerWithNoUsingsXDoReturnsPartialView : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public PartialViewResult HandlerAction()
        {
            return PartialView("~/Path/To/PartialViewWithoutModel.cshtml");
        }
    }

    public class ControllerWithNoUsings1Do1ToReturnsPartialView : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public PartialViewResult HandlerAction()
        {
            return PartialView("~/Path/To/PartialViewWithStringModel.cshtml", "baz");
        }
    }

    public class ControllerWith1BodyReturnsPartialView : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public PartialViewResult HandlerAction([FromBody]string name)
        {
            return PartialView("~/Path/To/PartialViewWithStringModel.cshtml", $"Hello {name}!");
        }
    }

    public class ControllerWith1Body1RouteParamReturnsPartialView : Controller
    {
        [HttpGet]
        [Route("/route/{lastName}")]
        public PartialViewResult HandlerAction([FromBody]string firstName, [FromRoute]string lastName)
        {
            return PartialView("~/Path/To/PartialViewWithStringModel.cshtml", $"Hello {firstName} {lastName}!");
        }
    }

    public class ControllerWith1Body1RouteParam2ToReturnsPartialView : Controller
    {
        [HttpGet]
        [Route("/route/{lastName}")]
        public PartialViewResult HandlerAction([FromBody]string firstName, [FromRoute]string lastName)
        {
            return PartialView("~/Path/To/PartialViewWithStringModel.cshtml", $"Hello {firstName} {lastName}!");
        }
    }
}
