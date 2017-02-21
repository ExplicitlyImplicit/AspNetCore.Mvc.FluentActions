using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers
{
    public class ControllerWithBodyReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public string HandlerAction([FromBody]string name)
        {
            return $"Hello {name}!";
        }
    }

    public class ControllerWithBodyReturnsStringAsync : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public async Task<string> HandlerAction([FromBody]string name)
        {
            await Task.Delay(1);
            return $"Hello {name}!";
        }
    }

    public class ControllerWithBodyAndDefaultValueReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public string HandlerAction([FromBody]string name = "Hanzel")
        {
            return $"Hello {name}!";
        }
    }

    public class ControllerWithBodyAndDefaultValueReturnsStringAsync : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public async Task<string> HandlerAction([FromBody]string name = "Hanzel")
        {
            await Task.Delay(1);
            return $"Hello {name}!";
        }
    }

    public class ControllerWithTwoIdenticalBodysReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public string HandlerAction([FromBody]string name)
        {
            return $"Hello {name}! I said hello {name}!";
        }
    }

    public class ControllerWithTwoIdenticalBodysReturnsStringAsync : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public async Task<string> HandlerAction([FromBody]string name)
        {
            await Task.Delay(1);
            return $"Hello {name}! I said hello {name}!";
        }
    }
}
