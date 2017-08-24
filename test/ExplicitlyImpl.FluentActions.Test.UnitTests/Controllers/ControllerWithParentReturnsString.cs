using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers
{
    public class ControllerWithParentReturnsString : BaseController
    {
        [HttpGet]
        [Route("/route/url")]
        public string HandlerAction()
        {
            return Hello();
        }
    }

    public class ControllerWithParentAndBodyReturnsString : BaseController
    {
        [HttpGet]
        [Route("/route/url")]
        public string HandlerAction([FromQuery]string name)
        {
            return Hello(name);
        }
    }

    public class ControllerWithParentReturnsStringAsync : BaseController
    {
        [HttpGet]
        [Route("/route/url")]
        public async Task<string> HandlerAction()
        {
            await Task.Delay(1);
            return Hello();
        }
    }

    public class ControllerWithParentAndBodyReturnsStringAsync : BaseController
    {
        [HttpGet]
        [Route("/route/url")]
        public async Task<string> HandlerAction([FromQuery]string name)
        {
            await Task.Delay(1);
            return Hello(name);
        }
    }
}
