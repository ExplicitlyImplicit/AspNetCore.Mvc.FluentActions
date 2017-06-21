using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers;
using System.Threading.Tasks;
using Xunit;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public class BuilderTestsWithViewComponentResultAsync
    {
        [Fact(DisplayName = "no usings, 1 To, returns ViewComponentResult async")]
        public void FluentControllerBuilder_FluentActionNoUsings1ToReturnsViewComponentAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .To(async () => { await Task.Delay(1); return "Hello World!"; })
                    .ToViewComponent("ViewComponentWithStringModel"),
                typeof(ControllerWithNoUsingsXToReturnsViewComponentAsync),
                null);
        }

        [Fact(DisplayName = "no usings, 3 To, returns ViewComponentResult async")]
        public void FluentControllerBuilder_FluentActionNoUsings3ToReturnsViewComponentAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .To(async () => { await Task.Delay(1); return "Hello"; })
                    .UsingResult()
                    .To(async text => { await Task.Delay(1); return $"{text} World"; })
                    .UsingResult()
                    .To(async text => { await Task.Delay(1); return $"{text}!"; })
                    .ToViewComponent("ViewComponentWithStringModel"),
                typeof(ControllerWithNoUsingsXToReturnsViewComponentAsync),
                null);
        }

        [Fact(DisplayName = "no usings, 1 Do, returns ViewComponentResult async")]
        public void FluentControllerBuilder_FluentActionNoUsings1DoReturnsViewComponentAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .DoAsync(async () => { await Task.Delay(1); })
                    .ToViewComponent("ViewComponentWithoutModel"),
                typeof(ControllerWithNoUsingsXDoReturnsViewComponentAsync),
                null);
        }

        [Fact(DisplayName = "no usings, 3 Do, returns ViewComponentResult async")]
        public void FluentControllerBuilder_FluentActionNoUsings3DoReturnsViewComponentAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .DoAsync(async () => { await Task.Delay(1); })
                    .DoAsync(async () => { await Task.Delay(1); })
                    .DoAsync(async () => { await Task.Delay(1); })
                    .ToViewComponent("ViewComponentWithoutModel"),
                typeof(ControllerWithNoUsingsXDoReturnsViewComponentAsync),
                null);
        }

        [Fact(DisplayName = "no usings, 1 Do, 1 To, returns ViewComponentResult async")]
        public void FluentControllerBuilder_FluentActionNoUsings1Do1ToReturnsViewComponentAsync()
        {
            var foo = "bar";

            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .DoAsync(async () => { await Task.Delay(1); foo = "baz"; })
                    .To(async () => { await Task.Delay(1); return foo; })
                    .ToViewComponent("ViewComponentWithStringModel"),
                typeof(ControllerWithNoUsings1Do1ToReturnsViewComponentAsync),
                null);
        }

        [Fact(DisplayName = "1 body (string), returns ViewComponentResult async")]
        public void FluentControllerBuilder_FluentAction1BodyReturnsViewComponentAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingBody<string>()
                    .To(async (name) => { await Task.Delay(1); return $"Hello {name}!"; })
                    .ToViewComponent("ViewComponentWithStringModel"),
                typeof(ControllerWith1BodyReturnsViewComponentAsync),
                new object[] { "Bob" });
        }

        [Fact(DisplayName = "1 body (string), 1 Do, 1 To, returns ViewComponentResult async")]
        public void FluentControllerBuilder_FluentAction1Body1DoReturnsViewComponentAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .DoAsync(async () => { await Task.Delay(1); })
                    .UsingBody<string>()
                    .To(async (name) => { await Task.Delay(1); return $"Hello {name}!"; })
                    .ToViewComponent("ViewComponentWithStringModel"),
                typeof(ControllerWith1BodyReturnsViewComponentAsync),
                new object[] { "Bob" });
        }

        [Fact(DisplayName = "1 body (string), 1 route param (string), returns ViewComponentResult async")]
        public void FluentControllerBuilder_FluentAction1Body1RouteParamReturnsViewComponentAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/{lastName}", HttpMethod.Get)
                    .UsingBody<string>()
                    .UsingRouteParameter<string>("lastName")
                    .To(async (firstName, lastName) => { await Task.Delay(1); return $"Hello {firstName} {lastName}!"; })
                    .ToViewComponent("ViewComponentWithStringModel"),
                typeof(ControllerWith1Body1RouteParamReturnsViewComponentAsync),
                new object[] { "Bob", "Bobsson" });
        }

        [Fact(DisplayName = "1 body (string), 1 route param (string), 2 To, returns ViewComponentResult async")]
        public void FluentControllerBuilder_FluentAction1Body1RouteParam2ToReturnsViewComponentAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/{lastName}", HttpMethod.Get)
                    .UsingBody<string>()
                    .To(async firstName => { await Task.Delay(1); return $"Hello {firstName}"; })
                    .UsingRouteParameter<string>("lastName")
                    .To(async lastName => { await Task.Delay(1); return $"{lastName}!"; })
                    .ToViewComponent("ViewComponentWithStringModel"),
                typeof(ControllerWith1Body1RouteParam2ToReturnsViewComponentAsync),
                new object[] { "Bob", "Bobsson" });
        }
    }
}
