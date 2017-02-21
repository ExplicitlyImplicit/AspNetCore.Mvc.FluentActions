using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

    public class ControllerWithQueryStringParameterReturnsStringAsync : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public async Task<string> HandlerAction([FromQuery]string name)
        {
            await Task.Delay(1);
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

    public class ControllerWithQueryStringParameterAndDefaultValueReturnsStringAsync : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public async Task<string> HandlerAction([FromQuery]string name = "Hanzel")
        {
            await Task.Delay(1);
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

    public class ControllerWithTwoIdenticalQueryStringParametersReturnsStringAsync : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public async Task<string> HandlerAction([FromQuery]string name)
        {
            await Task.Delay(1);
            return $"Hello {name}! I said hello {name}!";
        }
    }
}
