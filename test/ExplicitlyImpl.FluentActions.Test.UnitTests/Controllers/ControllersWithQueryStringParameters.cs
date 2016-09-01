using Microsoft.AspNetCore.Mvc;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers
{
    public class ControllerWithQueryStringParameterReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public string HandlerAction([FromQuery]string name)
        {
            return $"Hello {name}!";
        }
    }

    public class ControllerWithQueryStringParameterAndDefaultValueReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public string HandlerAction([FromQuery]string name = "Hanzel")
        {
            return $"Hello {name}!";
        }
    }

    public class ControllerWithTwoIdenticalQueryStringParametersReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public string HandlerAction([FromQuery]string name)
        {
            return $"Hello {name}! I said hello {name}!";
        }
    }
}
