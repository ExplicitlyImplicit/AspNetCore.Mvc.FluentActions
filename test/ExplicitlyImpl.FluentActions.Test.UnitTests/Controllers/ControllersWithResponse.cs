using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers
{
    public class ControllerWithResponseReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public string HandlerAction()
        {
            if (Response == null)
            {
                throw new Exception("Response is null inside fluent action delegate.");
            }

            return "Hello";
        }
    }

    public class ControllerWithResponseReturnsStringAsync : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public async Task<string> HandlerAction()
        {
            await Task.Delay(1);
            if (Response == null)
            {
                throw new Exception("Response is null inside fluent action delegate.");
            }

            return "Hello";
        }
    }
}
