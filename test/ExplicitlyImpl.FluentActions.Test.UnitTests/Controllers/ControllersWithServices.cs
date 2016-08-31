using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers
{
    public class ControllerWithStringService : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public string HandlerAction([FromServices]IStringTestService stringTestService)
        {
            return stringTestService.GetTestString() + "FromAFluentAction";
        }
    }

    public class ControllerWithStringServiceAsync : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public async Task<string> HandlerAction([FromServices]IStringTestService stringTestService)
        {
            return await stringTestService.GetTestStringAsync() + "FromAFluentAction";
        }
    }

    public class ControllerWithUserService : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public IList<UserItem> HandlerAction([FromServices]IUserService userService)
        {
            return userService.ListUsers();
        }
    }

    public class ControllerWithUserServiceAsync : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public async Task<IList<UserItem>> HandlerAction([FromServices]IUserService userService)
        {
            return await userService.ListUsersAsync();
        }
    }

    public class ControllerWithMultipleServicesOfSameInterface : Controller
    {
        [HttpGet]
        [Route("/route/url")]
        public string HandlerAction([FromServices]IStringTestService stringTestService1, [FromServices]IStringTestService stringTestService2)
        {
            return stringTestService1.GetTestString() + "And" + stringTestService2.GetTestString();
        }
    }
}
