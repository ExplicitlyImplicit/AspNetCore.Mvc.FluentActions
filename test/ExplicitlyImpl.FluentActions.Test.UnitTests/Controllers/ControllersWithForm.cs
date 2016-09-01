using Microsoft.AspNetCore.Mvc;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers
{
    public class ControllerWithFormReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public string HandlerAction([FromForm]string name)
        {
            return $"Hello {name}!";
        }
    }

    public class ControllerWithFormAndDefaultValueReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public string HandlerAction([FromForm]string name = "Hanzel")
        {
            return $"Hello {name}!";
        }
    }

    public class ControllerWithTwoIdenticalFormsReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public string HandlerAction([FromForm]string name)
        {
            return $"Hello {name}! I said hello {name}!";
        }
    }
}
