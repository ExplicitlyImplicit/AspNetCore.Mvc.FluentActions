using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Internal;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers
{
    public class ControllerWithModelBinderReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public string HandlerAction([ModelBinder(BinderType = typeof(NoOpBinder))]string name)
        {
            return $"Hello {name}!";
        }
    }

    public class ControllerWithModelBinderAndDefaultValueReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public string HandlerAction([ModelBinder(BinderType = typeof(NoOpBinder))]string name = "Hanzel")
        {
            return $"Hello {name}!";
        }
    }

    public class ControllerWithTwoIdenticalModelBindersReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public string HandlerAction([ModelBinder(BinderType = typeof(NoOpBinder))]string name)
        {
            return $"Hello {name}! I said hello {name}!";
        }
    }
}
