using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers
{
    public class ControllerWithHttpContextReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public string HandlerAction()
        {
            if (HttpContext == null)
            {
                throw new Exception("HttpContext is null inside fluent action delegate.");
            }

            return "Hello";
        }
    }

    public class ControllerWithHttpContextReturnsStringAsync : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public async Task<string> HandlerAction()
        {
            await Task.Delay(1);
            if (HttpContext == null)
            {
                throw new Exception("HttpContext is null inside fluent action delegate.");
            }

            return "Hello";
        }
    }
}
