using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

    public class ControllerWithRouteParameterReturnsStringAsync : Controller
    {
        [HttpGet]
        [Route("/route/{name}")]
        public async Task<string> HandlerAction([FromRoute]string name)
        {
            await Task.Delay(1);
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

    public class ControllerWithRouteParameterAndDefaultValueReturnsStringAsync : Controller
    {
        [HttpGet]
        [Route("/route/{name}")]
        public async Task<string> HandlerAction([FromRoute]string name = "Hanzel")
        {
            await Task.Delay(1);
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

    public class ControllerWithTwoIdenticalRouteParametersReturnsStringAsync : Controller
    {
        [HttpGet]
        [Route("/route/{name}")]
        public async Task<string> HandlerAction([FromRoute]string name)
        {
            await Task.Delay(1);
            return $"Hello {name}! I said hello {name}!";
        }
    }
}
