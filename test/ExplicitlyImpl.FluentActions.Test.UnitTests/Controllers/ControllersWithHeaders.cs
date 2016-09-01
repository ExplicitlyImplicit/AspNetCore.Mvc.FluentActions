using Microsoft.AspNetCore.Mvc;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers
{
    public class ControllerWithHeaderReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public string HandlerAction([FromHeader]string name)
        {
            return $"Hello {name}!";
        }
    }

    public class ControllerWithHeaderAndDefaultValueReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public string HandlerAction([FromHeader]string name = "Hanzel")
        {
            return $"Hello {name}!";
        }
    }

    public class ControllerWithTwoIdenticalHeadersReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public string HandlerAction([FromHeader]string name)
        {
            return $"Hello {name}! I said hello {name}!";
        }
    }
}
