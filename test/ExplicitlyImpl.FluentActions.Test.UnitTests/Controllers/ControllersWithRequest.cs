using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers
{
    public class ControllerWithRequestReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public string HandlerAction()
        {
            if (Request == null)
            {
                throw new Exception("Request is null inside fluent action delegate.");
            }

            return "Hello";
        }
    }

    public class ControllerWithRequestReturnsStringAsync : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public async Task<string> HandlerAction()
        {
            await Task.Delay(1);
            if (Request == null)
            {
                throw new Exception("Request is null inside fluent action delegate.");
            }

            return "Hello";
        }
    }
}
