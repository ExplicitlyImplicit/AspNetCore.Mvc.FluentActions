using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers;
using Xunit;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public class BuilderTestsWithViewComponentTypeResult
    {
        [Fact(DisplayName = "no usings, no To, returns ViewComponentResult (type)")]
        public void FluentControllerBuilder_FluentActionNoUsingsNoToReturnsViewComponentUsingType()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .ToViewComponent(typeof(ViewComponentWithoutModel)),
                typeof(ControllerWithNoUsingsNoToReturnsViewComponentUsingType),
                null);
        }

        [Fact(DisplayName = "no usings, 1 To, returns ViewComponentResult (type)")]
        public void FluentControllerBuilder_FluentActionNoUsings1ToReturnsViewComponentUsingType()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .To(() => "Hello World!")
                    .ToViewComponent(typeof(ViewComponentWithStringModel)),
                typeof(ControllerWithNoUsingsXToReturnsViewComponentUsingType),
                null);
        }

        [Fact(DisplayName = "no usings, 3 To, returns ViewComponentResult (type)")]
        public void FluentControllerBuilder_FluentActionNoUsings3ToReturnsViewComponentUsingType()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .To(() => "Hello")
                    .UsingResult()
                    .To(text => $"{text} World")
                    .UsingResult()
                    .To(text => $"{text}!")
                    .ToViewComponent(typeof(ViewComponentWithStringModel)),
                typeof(ControllerWithNoUsingsXToReturnsViewComponentUsingType),
                null);
        }

        [Fact(DisplayName = "no usings, 1 Do, returns ViewComponentResult (type)")]
        public void FluentControllerBuilder_FluentActionNoUsings1DoReturnsViewComponentUsingType()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .Do(() => { /* woop woop */ })
                    .ToViewComponent(typeof(ViewComponentWithoutModel)),
                typeof(ControllerWithNoUsingsXDoReturnsViewComponentUsingType),
                null);
        }

        [Fact(DisplayName = "no usings, 3 Do, returns ViewComponentResult (type)")]
        public void FluentControllerBuilder_FluentActionNoUsings3DoReturnsViewComponentUsingType()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .Do(() => { /* woop woop */ })
                    .Do(() => { /* woop woop */ })
                    .Do(() => { /* woop woop */ })
                    .ToViewComponent(typeof(ViewComponentWithoutModel)),
                typeof(ControllerWithNoUsingsXDoReturnsViewComponentUsingType),
                null);
        }

        [Fact(DisplayName = "no usings, 1 Do, 1 To, returns ViewComponentResult (type)")]
        public void FluentControllerBuilder_FluentActionNoUsings1Do1ToReturnsViewComponentUsingType()
        {
            var foo = "bar";

            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .Do(() => { foo = "baz"; })
                    .To(() => foo)
                    .ToViewComponent(typeof(ViewComponentWithStringModel)),
                typeof(ControllerWithNoUsings1Do1ToReturnsViewComponentUsingType),
                null);
        }

        [Fact(DisplayName = "1 body (string), returns ViewComponentResult (type)")]
        public void FluentControllerBuilder_FluentAction1BodyReturnsViewComponentUsingType()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingBody<string>()
                    .To(name => $"Hello {name}!")
                    .ToViewComponent(typeof(ViewComponentWithStringModel)),
                typeof(ControllerWith1BodyReturnsViewComponentUsingType),
                new object[] { "Bob" });
        }

        [Fact(DisplayName = "1 body (string), 1 Do, 1 To, returns ViewComponentResult (type)")]
        public void FluentControllerBuilder_FluentAction1Body1DoReturnsViewComponentUsingType()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .Do(() => { /* woop woop */ })
                    .UsingBody<string>()
                    .To(name => $"Hello {name}!")
                    .ToViewComponent(typeof(ViewComponentWithStringModel)),
                typeof(ControllerWith1BodyReturnsViewComponentUsingType),
                new object[] { "Bob" });
        }

        [Fact(DisplayName = "1 body (string), 1 route param (string), returns ViewComponentResult (type)")]
        public void FluentControllerBuilder_FluentAction1Body1RouteParamReturnsViewComponentUsingType()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/{lastName}", HttpMethod.Get)
                    .UsingBody<string>()
                    .UsingRouteParameter<string>("lastName")
                    .To((firstName, lastName) => $"Hello {firstName} {lastName}!")
                    .ToViewComponent(typeof(ViewComponentWithStringModel)),
                typeof(ControllerWith1Body1RouteParamReturnsViewComponentUsingType),
                new object[] { "Bob", "Bobsson" });
        }

        [Fact(DisplayName = "1 body (string), 1 route param (string), 2 To, returns ViewComponentResult (type)")]
        public void FluentControllerBuilder_FluentAction1Body1RouteParam2ToReturnsViewComponentUsingType()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/{lastName}", HttpMethod.Get)
                    .UsingBody<string>()
                    .To(firstName => $"Hello {firstName}")
                    .UsingRouteParameter<string>("lastName")
                    .To(lastName => $"{lastName}!")
                    .ToViewComponent(typeof(ViewComponentWithStringModel)),
                typeof(ControllerWith1Body1RouteParam2ToReturnsViewComponentUsingType),
                new object[] { "Bob", "Bobsson" });
        }
    }
}
