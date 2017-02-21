using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Internal;
using System.Threading.Tasks;

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

    public class ControllerWithModelBinderReturnsStringAsync : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public async Task<string> HandlerAction([ModelBinder(BinderType = typeof(NoOpBinder))]string name)
        {
            await Task.Delay(1);
            return $"Hello {name}!";
        }
    }

    public class ControllerWithModelBinderAndNamePropertyReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public string HandlerAction([ModelBinder(BinderType = typeof(NoOpBinder), Name = "NoOpName")]string name)
        {
            return $"Hello {name}!";
        }
    }

    public class ControllerWithModelBinderAndNamePropertyReturnsStringAsync : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public async Task<string> HandlerAction([ModelBinder(BinderType = typeof(NoOpBinder), Name = "NoOpName")]string name)
        {
            await Task.Delay(1);
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

    public class ControllerWithModelBinderAndDefaultValueReturnsStringAsync : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public async Task<string> HandlerAction([ModelBinder(BinderType = typeof(NoOpBinder))]string name = "Hanzel")
        {
            await Task.Delay(1);
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

    public class ControllerWithTwoIdenticalModelBindersReturnsStringAsync : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public async Task<string> HandlerAction([ModelBinder(BinderType = typeof(NoOpBinder))]string name)
        {
            await Task.Delay(1);
            return $"Hello {name}! I said hello {name}!";
        }
    }
}
