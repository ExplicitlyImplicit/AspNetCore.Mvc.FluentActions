using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers
{
    public class ControllerWithFormValueReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public string HandlerAction([FromForm]string name)
        {
            return $"Hello {name}!";
        }
    }

    public class ControllerWithFormValueReturnsStringAsync : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public async Task<string> HandlerAction([FromForm]string name)
        {
            await Task.Delay(1);
            return $"Hello {name}!";
        }
    }

    public class ControllerWithFormValueAndDefaultValueReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public string HandlerAction([FromForm]string name = "Hanzel")
        {
            return $"Hello {name}!";
        }
    }

    public class ControllerWithFormValueAndDefaultValueReturnsStringAsync : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public async Task<string> HandlerAction([FromForm]string name = "Hanzel")
        {
            await Task.Delay(1);
            return $"Hello {name}!";
        }
    }

    public class ControllerWithTwoIdenticalFormValuesReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public string HandlerAction([FromForm]string name)
        {
            return $"Hello {name}! I said hello {name}!";
        }
    }

    public class ControllerWithTwoIdenticalFormValuesReturnsStringAsync : Controller
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
