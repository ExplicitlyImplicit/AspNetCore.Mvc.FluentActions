using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public class NestedAppendsReturnsHashContainer
    {
        [Fact(DisplayName = "2 Append in nested collection config returns hash container")]
        public void FluentActionCollection_DefineActions_Config_NestedAppend_ReturnsHashContainer()
        {
            var innerActionCollection = FluentActionCollection.DefineActions(
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

            var actionCollection = FluentActionCollection.DefineActions(
                actions =>
                {
                    actions.Configure(config =>
                    {
                        config.Append(action => action
                            .WithCustomAttribute<NestedAppendCustomAttribute>(new Type[] { typeof(string) }, new object[] { "AttrValue" })
                            .UsingResult()
                            .UsingQueryStringParameter<string>("additional")
                            .To((result, additional) => new HashContainer { Value = result.GetHashCode(), Additional = additional })
                        );
                    });

                    actions.Add(innerActionCollection);
                }
            );

            foreach (var action in actionCollection)
            {
                switch (action.RouteTemplate)
                {
                    case "/hello":
                        BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                            action,
                            typeof(NestedAppend_HelloController),
                            new object[] { "Bob", "Extra" });
                        break;
                    case "/helloAsync":
                        BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                            action,
                            typeof(NestedAppend_HelloAsyncController),
                            new object[] { "Bob", "Extra" });
                        break;
                    case "/helloAsyncWithDo":
                        BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                            action,
                            typeof(NestedAppend_HelloAsyncWithDoController),
                            new object[] { "Bob", "Extra" });
                        break;
                    case "/helloAsyncWithAsyncDo":
                        BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                            action,
                            typeof(NestedAppend_HelloAsyncWithAsyncDoController),
                            new object[] { "Bob", "Extra" });
                        break;
                    case "/hi/{name}":
                        BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                            action,
                            typeof(NestedAppend_HiController),
                            new object[] { "Bob", "Extra" });
                        break;
                    case "/userCount":
                        BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                            action,
                            typeof(NestedAppend_UserCountController),
                            new object[] { new UserService(), "Extra" });
                        break;
                    case "/toView":
                        BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                            action,
                            typeof(NestedAppend_ToViewController),
                            new object[] { "Extra" });
                        break;
                    default:
                        throw new Exception($"Could not find controller type to compare with for action {action}");
                }
            }
        }
    }

    public class HashContainer
    {
        public int Value { get; set; }

        public string Additional { get; set; }

        public override bool Equals(object obj)
        {
            return obj is HashContainer && obj != null && ((HashContainer)obj).Value == Value && ((HashContainer)obj).Additional == Additional;
        }

        public override int GetHashCode()
        {
            return Value * 17 + (Additional ?? "").GetHashCode();
        }
    }

    public class NestedAppendCustomAttribute : Attribute
    {
        public string Value { get; set; }

        public NestedAppendCustomAttribute(string value = null)
        {
            Value = value;
        }
    }

    public class NestedAppend_HelloController : Controller
    {
        [HttpGet]
        [Route("/hello")]
        [NestedAppendCustom("AttrValue")]
        public HashContainer HandlerAction([FromQuery]string name, [FromQuery]string additional)
        {
            return new HashContainer { Value = $"Hello {name}!".GetHashCode(), Additional = additional };
        }
    }

    public class NestedAppend_HelloAsyncController : Controller
    {
        [HttpGet]
        [Route("/helloAsync")]
        [NestedAppendCustom("AttrValue")]
        public async Task<HashContainer> HandlerAction([FromQuery]string name, [FromQuery]string additional)
        {
            await Task.Delay(1);
            return new HashContainer { Value = $"Hello {name}!".GetHashCode(), Additional = additional };
        }
    }

    public class NestedAppend_HelloAsyncWithDoController : Controller
    {
        [HttpGet]
        [Route("/helloAsyncWithDo")]
        [NestedAppendCustom("AttrValue")]
        public async Task<HashContainer> HandlerAction([FromQuery]string name, [FromQuery]string additional)
        {
            await Task.Delay(1);
            return new HashContainer { Value = $"Hello {name}!".GetHashCode(), Additional = additional };
        }
    }

    public class NestedAppend_HelloAsyncWithAsyncDoController : Controller
    {
        [HttpGet]
        [Route("/helloAsyncWithAsyncDo")]
        [NestedAppendCustom("AttrValue")]
        public async Task<HashContainer> HandlerAction([FromQuery]string name, [FromQuery]string additional)
        {
            await Task.Delay(1);
            return new HashContainer { Value = $"Hello {name}!".GetHashCode(), Additional = additional };
        }
    }

    public class NestedAppend_HiController : Controller
    {
        [HttpGet]
        [Route("/hi/{name}")]
        [NestedAppendCustom("AttrValue")]
        public HashContainer HandlerAction([FromRoute]string name, [FromQuery]string additional)
        {
            return new HashContainer { Value = $"Hi {name}! How are you?".GetHashCode(), Additional = additional };
        }
    }

    public class NestedAppend_UserCountController : Controller
    {
        [HttpGet]
        [Route("/userCount")]
        [NestedAppendCustom("AttrValue")]
        public HashContainer HandlerAction([FromServices]IUserService userService, [FromQuery]string additional)
        {
            var userCount = userService.ListUsers().Count;
            return new HashContainer { Value = userCount.ToString().GetHashCode(), Additional = additional };
        }
    }

    public class NestedAppend_ToViewController : Controller
    {
        [HttpGet]
        [Route("/toView")]
        [NestedAppendCustom("AttrValue")]
        public HashContainer HandlerAction([FromQuery]string additional)
        {
            var view = View("~/path/to/view");
            return new HashContainer { Value = view.ToString().GetHashCode(), Additional = additional };
        }
    }
}
