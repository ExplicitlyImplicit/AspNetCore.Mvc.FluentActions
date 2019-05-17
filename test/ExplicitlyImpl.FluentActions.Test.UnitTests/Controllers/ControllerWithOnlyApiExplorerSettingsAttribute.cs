using Microsoft.AspNetCore.Mvc;
using System;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers
{
    public class ControllerWithGroupNameOnlyApiExplorerSettingsAttribute : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        [ApiExplorerSettings(GroupName = "CustomGroupName")]
        public string HandlerAction()
        {
            return "Hello";
        }
    }

    public class ControllerWithIgnoreApiOnlyApiExplorerSettingsAttribute : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public string HandlerAction()
        {
            return "Hello";
        }
    }

    public class ControllerWithApiExplorerSettingsAttribute : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        [ApiExplorerSettings(GroupName = "CustomGroupName", IgnoreApi = true)]
        public string HandlerAction()
        {
            return "Hello";
        }
    }
}
