using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers
{
    [MyCustomAttribute()]
    public class ControllerWith1EmptyCustomAttributeOnClassReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public string HandlerAction()
        {
            return "hello";
        }
    }

    [MyCustomAttribute(10)]
    public class ControllerWith1ConstructorCustomAttributeOnClassReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public string HandlerAction()
        {
            return "hello";
        }
    }

    [MyCustomAttribute(Property = "prop")]
    public class ControllerWith1PropertyCustomAttributeOnClassReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public string HandlerAction()
        {
            return "hello";
        }
    }

    [MyCustomAttribute(Property = "field")]
    public class ControllerWith1FieldCustomAttributeOnClassReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public string HandlerAction()
        {
            return "hello";
        }
    }

    [MyCustomAttribute(ConstructorArg = 10, Property = "prop")]
    public class ControllerWith1ConstructorPropertyCustomAttributeOnClassReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public string HandlerAction()
        {
            return "hello";
        }
    }

    [MyCustomAttribute(ConstructorArg = 10, Field = "field")]
    public class ControllerWith1ConstructorFieldCustomAttributeOnClassReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public string HandlerAction()
        {
            return "hello";
        }
    }

    [MyCustomAttribute(Property = "prop", Field = "field")]
    public class ControllerWith1FieldPropertyCustomAttributeOnClassReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public string HandlerAction()
        {
            return "hello";
        }
    }

    [MyCustomAttribute(ConstructorArg = 10, Property = "prop", Field = "field")]
    public class ControllerWith1ConstructorFieldPropertyCustomAttributeOnClassReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public string HandlerAction()
        {
            return "hello";
        }
    }

    [MyCustomAttribute()]
    [MySecondCustomAttribute()]
    public class ControllerWith2EmptyCustomAttributesOnClassReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public string HandlerAction()
        {
            return "hello";
        }
    }

    [MyCustomAttribute()]
    public class ControllerWith1EmptyCustomAttributeOnClassReturnsViewResultAsync : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public async Task<ViewResult> HandlerAction()
        {
            await Task.Delay(1);
            return View("~/Path/To/ViewWithStringModel.cshtml", "hello");
        }
    }

    [MyCustomAttribute()]
    [MySecondCustomAttribute()]
    public class ControllerWith2EmptyCustomAttributesOnClassReturnsViewResultAsync : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public async Task<ViewResult> HandlerAction()
        {
            await Task.Delay(1);
            return View("~/Path/To/ViewWithStringModel.cshtml", "hello");
        }
    }
}
