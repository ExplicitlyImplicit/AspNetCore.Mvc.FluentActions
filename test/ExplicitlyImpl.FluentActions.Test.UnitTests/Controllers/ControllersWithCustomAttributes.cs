using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers
{
    public class MyCustomAttribute : Attribute
    {
        public int ConstructorArg { get; set; }

        public string Property { get; set; }

        public string Field;

        public MyCustomAttribute() { }

        public MyCustomAttribute(int constructorArg)
        {
            ConstructorArg = constructorArg;
        }
    }

    public class MySecondCustomAttribute : Attribute
    {

    }

    public class ControllerWith1EmptyCustomAttributeReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        [MyCustomAttribute()]
        public string HandlerAction()
        {
            return "hello";
        }
    }

    public class ControllerWith1ConstructorCustomAttributeReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        [MyCustomAttribute(10)]
        public string HandlerAction()
        {
            return "hello";
        }
    }

    public class ControllerWith1PropertyCustomAttributeReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        [MyCustomAttribute(Property = "prop")]
        public string HandlerAction()
        {
            return "hello";
        }
    }

    public class ControllerWith1FieldCustomAttributeReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        [MyCustomAttribute(Property = "field")]
        public string HandlerAction()
        {
            return "hello";
        }
    }

    public class ControllerWith1ConstructorPropertyCustomAttributeReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        [MyCustomAttribute(ConstructorArg = 10, Property = "prop")]
        public string HandlerAction()
        {
            return "hello";
        }
    }

    public class ControllerWith1ConstructorFieldCustomAttributeReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        [MyCustomAttribute(ConstructorArg = 10, Field = "field")]
        public string HandlerAction()
        {
            return "hello";
        }
    }

    public class ControllerWith1FieldPropertyCustomAttributeReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        [MyCustomAttribute(Property = "prop", Field = "field")]
        public string HandlerAction()
        {
            return "hello";
        }
    }

    public class ControllerWith1ConstructorFieldPropertyCustomAttributeReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        [MyCustomAttribute(ConstructorArg = 10, Property = "prop", Field = "field")]
        public string HandlerAction()
        {
            return "hello";
        }
    }

    public class ControllerWith2EmptyCustomAttributesReturnsString : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        [MyCustomAttribute()]
        [MySecondCustomAttribute()]
        public string HandlerAction()
        {
            return "hello";
        }
    }

    public class ControllerWith1EmptyCustomAttributeReturnsViewResultAsync : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        [MyCustomAttribute()]
        public async Task<ViewResult> HandlerAction()
        {
            await Task.Delay(1);
            return View("~/Path/To/ViewWithStringModel.cshtml", "hello");
        }
    }

    public class ControllerWith2EmptyCustomAttributesReturnsViewResultAsync : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        [MyCustomAttribute()]
        [MySecondCustomAttribute()]
        public async Task<ViewResult> HandlerAction()
        {
            await Task.Delay(1);
            return View("~/Path/To/ViewWithStringModel.cshtml", "hello");
        }
    }
}
