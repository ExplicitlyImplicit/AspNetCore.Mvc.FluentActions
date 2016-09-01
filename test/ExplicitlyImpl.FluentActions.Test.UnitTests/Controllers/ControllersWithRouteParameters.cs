using Microsoft.AspNetCore.Mvc;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers
{
    public class ControllerWithRouteParameterReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/{name}")]
        public string HandlerAction([FromRoute]string name)
        {
            return $"Hello {name}!";
        }
    }

    public class ControllerWithRouteParameterAndDefaultValueReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/{name}")]
        public string HandlerAction([FromRoute]string name = "Hanzel")
        {
            return $"Hello {name}!";
        }
    }

    public class ControllerWithTwoIdenticalRouteParametersReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/{name}")]
        public string HandlerAction([FromRoute]string name)
        {
            return $"Hello {name}! I said hello {name}!";
        }
    }
}
