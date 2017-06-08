using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers;
using Xunit;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public class BuilderTestsWithViewComponentResult
    {
        [Fact(DisplayName = "no usings, no To, returns ViewComponentResult")]
        public void FluentControllerBuilder_FluentActionNoUsingsNoToReturnsViewComponent()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .ToViewComponent("ViewComponentWithoutModel"),
                typeof(ControllerWithNoUsingsNoToReturnsViewComponent),
                null);
        }

        [Fact(DisplayName = "1 body (string), no To, returns ViewComponentResult")]
        public void FluentControllerBuilder_FluentAction1BodyNoToReturnsViewComponent()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingBody<string>()
                    .ToViewComponent("ViewComponentWithStringModel"),
                typeof(ControllerPassing1BodyReturnsViewComponent),
                new object[] { "Text" });
        }

        [Fact(DisplayName = "1 body (string), 1 route param (string), no To, returns ViewComponentResult")]
        public void FluentControllerBuilder_FluentAction1Body1RouteParamNoToReturnsViewComponent()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url/{unused}", HttpMethod.Get)
                    .UsingBody<string>()
                    .UsingRouteParameter<string>("unused")
                    .ToViewComponent("ViewComponentWithStringModel"),
                typeof(ControllerWith1Body1RouteParamPassing1BodyReturnsViewComponent),
                new object[] { "Text", "Unused" });
        }

        [Fact(DisplayName = "no usings, 1 To, returns ViewComponentResult")]
        public void FluentControllerBuilder_FluentActionNoUsings1ToReturnsViewComponent()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .To(() => "Hello World!")
                    .ToViewComponent("ViewComponentWithStringModel"),
                typeof(ControllerWithNoUsingsXToReturnsViewComponent),
                null);
        }

        [Fact(DisplayName = "no usings, 3 To, returns ViewComponentResult")]
        public void FluentControllerBuilder_FluentActionNoUsings3ToReturnsViewComponent()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .To(() => "Hello")
                    .UsingResultFromHandler()
                    .To(text => $"{text} World")
                    .UsingResultFromHandler()
                    .To(text => $"{text}!")
                    .ToViewComponent("ViewComponentWithStringModel"),
                typeof(ControllerWithNoUsingsXToReturnsViewComponent),
                null);
        }

        [Fact(DisplayName = "no usings, 1 Do, returns ViewComponentResult")]
        public void FluentControllerBuilder_FluentActionNoUsings1DoReturnsViewComponent()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .Do(() => { /* woop woop */ })
                    .ToViewComponent("ViewComponentWithoutModel"),
                typeof(ControllerWithNoUsingsXDoReturnsViewComponent),
                null);
        }

        [Fact(DisplayName = "no usings, 3 Do, returns ViewComponentResult")]
        public void FluentControllerBuilder_FluentActionNoUsings3DoReturnsViewComponent()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .Do(() => { /* woop woop */ })
                    .Do(() => { /* woop woop */ })
                    .Do(() => { /* woop woop */ })
                    .ToViewComponent("ViewComponentWithoutModel"),
                typeof(ControllerWithNoUsingsXDoReturnsViewComponent),
                null);
        }

        [Fact(DisplayName = "no usings, 1 Do, 1 To, returns ViewComponentResult")]
        public void FluentControllerBuilder_FluentActionNoUsings1Do1ToReturnsViewComponent()
        {
            var foo = "bar";

            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .Do(() => { foo = "baz"; })
                    .To(() => foo)
                    .ToViewComponent("ViewComponentWithStringModel"),
                typeof(ControllerWithNoUsings1Do1ToReturnsViewComponent),
                null);
        }

        [Fact(DisplayName = "1 body (string), returns ViewComponentResult")]
        public void FluentControllerBuilder_FluentAction1BodyReturnsViewComponent()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingBody<string>()
                    .To(name => $"Hello {name}!")
                    .ToViewComponent("ViewComponentWithStringModel"),
                typeof(ControllerWith1BodyReturnsViewComponent),
                new object[] { "Bob" });
        }

        [Fact(DisplayName = "1 body (string), 1 Do, 1 To, returns ViewComponentResult")]
        public void FluentControllerBuilder_FluentAction1Body1DoReturnsViewComponent()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .Do(() => { /* woop woop */ })
                    .UsingBody<string>()
                    .To(name => $"Hello {name}!")
                    .ToViewComponent("ViewComponentWithStringModel"),
                typeof(ControllerWith1BodyReturnsViewComponent),
                new object[] { "Bob" });
        }

        [Fact(DisplayName = "1 body (string), 1 route param (string), returns ViewComponentResult")]
        public void FluentControllerBuilder_FluentAction1Body1RouteParamReturnsViewComponent()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/{lastName}", HttpMethod.Get)
                    .UsingBody<string>()
                    .UsingRouteParameter<string>("lastName")
                    .To((firstName, lastName) => $"Hello {firstName} {lastName}!")
                    .ToViewComponent("ViewComponentWithStringModel"),
                typeof(ControllerWith1Body1RouteParamReturnsViewComponent),
                new object[] { "Bob", "Bobsson" });
        }

        [Fact(DisplayName = "1 body (string), 1 route param (string), 2 To, returns ViewComponentResult")]
        public void FluentControllerBuilder_FluentAction1Body1RouteParam2ToReturnsViewComponent()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/{lastName}", HttpMethod.Get)
                    .UsingBody<string>()
                    .To(firstName => $"Hello {firstName}")
                    .UsingRouteParameter<string>("lastName")
                    .To(lastName => $"{lastName}!")
                    .ToViewComponent("ViewComponentWithStringModel"),
                typeof(ControllerWith1Body1RouteParam2ToReturnsViewComponent),
                new object[] { "Bob", "Bobsson" });
        }
    }
}
