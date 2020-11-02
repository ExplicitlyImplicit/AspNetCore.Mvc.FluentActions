using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public class AppendReturnsStringContainer
    {
        [Fact(DisplayName = "1 Append in collection config returns string container")]
        public void FluentActionCollection_DefineActions_Config_Append_ReturnsStringContainer()
        {
            var actionCollection = FluentActionCollection.DefineActions(
                actions =>
                {
                    actions.Configure(config =>
                    {
                        config.Append(action => action
                            .UsingResult()
                            .To(result => new StringContainer { Value = result.ToString() })
                        );
                    });

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
                            typeof(Append_HelloController),
                            new object[] { "Bob" });
                        break;
                    case "/helloAsync":
                        BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                            action,
                            typeof(Append_HelloAsyncController),
                            new object[] { "Bob" });
                        break;
                    case "/helloAsyncWithDo":
                        BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                            action,
                            typeof(Append_HelloAsyncWithDoController),
                            new object[] { "Bob" });
                        break;
                    case "/helloAsyncWithAsyncDo":
                        BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                            action,
                            typeof(Append_HelloAsyncWithAsyncDoController),
                            new object[] { "Bob" });
                        break;
                    case "/hi/{name}":
                        BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                            action,
                            typeof(Append_HiController),
                            new object[] { "Bob" });
                        break;
                    case "/userCount":
                        BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                            action,
                            typeof(Append_UserCountController),
                            new object[] { new UserService() });
                        break;
                    case "/toView":
                        BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                            action,
                            typeof(Append_ToViewController),
                            new object[0]);
                        break;
                    default:
                        throw new Exception($"Could not find controller type to compare with for action {action}");
                }
            }
        }
    }

    public class StringContainer
    {
        public string Value { get; set; }

        public override bool Equals(object obj)
        {
            return obj is StringContainer && obj != null && ((StringContainer) obj).Value == Value;
        }

        public override int GetHashCode()
        {
            return (Value ?? "").GetHashCode();
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public class Append_HelloController : Controller
    {
        [HttpGet]
        [Route("/hello")]
        public StringContainer HandlerAction([FromQuery]string name)
        {
            return new StringContainer { Value = $"Hello {name}!" };
        }
    }

    public class Append_HelloAsyncController : Controller
    {
        [HttpGet]
        [Route("/helloAsync")]
        public async Task<StringContainer> HandlerAction([FromQuery]string name)
        {
            await Task.Delay(1);
            return new StringContainer { Value = $"Hello {name}!" };
        }
    }

    public class Append_HelloAsyncWithDoController : Controller
    {
        [HttpGet]
        [Route("/helloAsyncWithDo")]
        public async Task<StringContainer> HandlerAction([FromQuery]string name)
        {
            await Task.Delay(1);
            return new StringContainer { Value = $"Hello {name}!" };
        }
    }

    public class Append_HelloAsyncWithAsyncDoController : Controller
    {
        [HttpGet]
        [Route("/helloAsyncWithAsyncDo")]
        public async Task<StringContainer> HandlerAction([FromQuery]string name)
        {
            await Task.Delay(1);
            return new StringContainer { Value = $"Hello {name}!" };
        }
    }

    public class Append_HiController : Controller
    {
        [HttpGet]
        [Route("/hi/{name}")]
        public StringContainer HandlerAction([FromRoute]string name)
        {
            return new StringContainer { Value = $"Hi {name}! How are you?" };
        }
    }

    public class Append_UserCountController : Controller
    {
        [HttpGet]
        [Route("/userCount")]
        public StringContainer HandlerAction([FromServices]IUserService userService)
        {
            var userCount = userService.ListUsers().Count;
            return new StringContainer { Value = userCount.ToString() };
        }
    }

    public class Append_ToViewController : Controller
    {
        [HttpGet]
        [Route("/toView")]
        public StringContainer HandlerAction()
        {
            var view = View("~/path/to/view");
            return new StringContainer { Value = view.ToString() };
        }
    }
}
