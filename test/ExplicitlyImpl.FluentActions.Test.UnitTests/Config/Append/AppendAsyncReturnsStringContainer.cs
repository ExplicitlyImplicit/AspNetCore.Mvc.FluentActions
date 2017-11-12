using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public class AppendAsyncReturnsStringContainer
    {
        [Fact(DisplayName = "1 Append (async) in collection config returns string container")]
        public void FluentActionCollection_DefineActions_Config_AppendAsync_ReturnsStringContainer()
        {
            var actionCollection = FluentActionCollection.DefineActions(
                config =>
                {
                    config.Append(action => action
                        .UsingResult()
                        .To(async result => { await Task.Delay(1); return new StringContainer { Value = result.ToString() }; })
                    );
                },
                actions => 
                {
                    actions
                        .RouteGet("/hello")
                        .UsingQueryStringParameter<string>("name")
                        .To(name => $"Hello {name}!");

                    actions
                        .RouteGet("/helloAsync")
                        .UsingQueryStringParameter<string>("name")
                        .To(async name => { await Task.Delay(1); return $"Hello {name}!"; });

                    actions
                        .RouteGet("/helloAsyncWithDo")
                        .Do(() => { /* Doing nothing */ })
                        .UsingQueryStringParameter<string>("name")
                        .To(async name => { await Task.Delay(1); return $"Hello {name}!"; });

                    actions
                        .RouteGet("/helloAsyncWithAsyncDo")
                        .Do(async () => { await Task.Delay(1); })
                        .UsingQueryStringParameter<string>("name")
                        .To(async name => { await Task.Delay(1); return $"Hello {name}!"; });

                    actions
                        .RouteGet("/hi/{name}")
                        .UsingRouteParameter<string>("name")
                        .To(name => $"Hi {name}!")
                        .UsingResult()
                        .To(greeting => $"{greeting} How are you?");

                    actions
                        .RouteGet("/userCount")
                        .UsingService<IUserService>()
                        .To(userService => userService.ListUsers().Count());

                    actions
                        .RouteGet("/toView")
                        .ToView("~/path/to/view");
                }
            );

            foreach (var action in actionCollection)
            {
                switch (action.RouteTemplate)
                {
                    case "/hello":
                        BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                            action,
                            typeof(AppendAsync_HelloController),
                            new object[] { "Bob" });
                        break;
                    case "/helloAsync":
                        BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                            action,
                            typeof(AppendAsync_HelloAsyncController),
                            new object[] { "Bob" });
                        break;
                    case "/helloAsyncWithDo":
                        BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                            action,
                            typeof(AppendAsync_HelloAsyncWithDoController),
                            new object[] { "Bob" });
                        break;
                    case "/helloAsyncWithAsyncDo":
                        BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                            action,
                            typeof(AppendAsync_HelloAsyncWithAsyncDoController),
                            new object[] { "Bob" });
                        break;
                    case "/hi/{name}":
                        BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                            action,
                            typeof(AppendAsync_HiController),
                            new object[] { "Bob" });
                        break;
                    case "/userCount":
                        BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                            action,
                            typeof(AppendAsync_UserCountController),
                            new object[] { new UserService() });
                        break;
                    case "/toView":
                        BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                            action,
                            typeof(AppendAsync_ToViewController),
                            new object[0]);
                        break;
                    default:
                        throw new Exception($"Could not find controller type to compare with for action {action}");
                }
            }
        }
    }

    public class AppendAsync_HelloController : Controller
    {
        [HttpGet]
        [Route("/hello")]
        public async Task<StringContainer> HandlerAction([FromQuery]string name)
        {
            await Task.Delay(1);
            return new StringContainer { Value = $"Hello {name}!" };
        }
    }

    public class AppendAsync_HelloAsyncController : Controller
    {
        [HttpGet]
        [Route("/helloAsync")]
        public async Task<StringContainer> HandlerAction([FromQuery]string name)
        {
            await Task.Delay(1);
            return new StringContainer { Value = $"Hello {name}!" };
        }
    }

    public class AppendAsync_HelloAsyncWithDoController : Controller
    {
        [HttpGet]
        [Route("/helloAsyncWithDo")]
        public async Task<StringContainer> HandlerAction([FromQuery]string name)
        {
            await Task.Delay(1);
            return new StringContainer { Value = $"Hello {name}!" };
        }
    }

    public class AppendAsync_HelloAsyncWithAsyncDoController : Controller
    {
        [HttpGet]
        [Route("/helloAsyncWithAsyncDo")]
        public async Task<StringContainer> HandlerAction([FromQuery]string name)
        {
            await Task.Delay(1);
            return new StringContainer { Value = $"Hello {name}!" };
        }
    }

    public class AppendAsync_HiController : Controller
    {
        [HttpGet]
        [Route("/hi/{name}")]
        public async Task<StringContainer> HandlerAction([FromRoute]string name)
        {
            await Task.Delay(1);
            return new StringContainer { Value = $"Hi {name}! How are you?" };
        }
    }

    public class AppendAsync_UserCountController : Controller
    {
        [HttpGet]
        [Route("/userCount")]
        public async Task<StringContainer> HandlerAction([FromServices]IUserService userService)
        {
            await Task.Delay(1);
            var userCount = userService.ListUsers().Count;
            return new StringContainer { Value = userCount.ToString() };
        }
    }

    public class AppendAsync_ToViewController : Controller
    {
        [HttpGet]
        [Route("/toView")]
        public async Task<StringContainer> HandlerAction()
        {
            await Task.Delay(1);
            var view = View("~/path/to/view");
            return new StringContainer { Value = view.ToString() };
        }
    }
}
