using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

    public class ControllerWithFormReturnsStringAsync : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public async Task<string> HandlerAction([FromForm]string name)
        {
            await Task.Delay(1);
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

    public class ControllerWithFormAndDefaultValueReturnsStringAsync : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public async Task<string> HandlerAction([FromForm]string name = "Hanzel")
        {
            await Task.Delay(1);
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

    public class ControllerWithTwoIdenticalFormsReturnsStringAsync : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public async Task<string> HandlerAction([FromForm]string name)
        {
            await Task.Delay(1);
            return $"Hello {name}! I said hello {name}!";
        }
    }
}
