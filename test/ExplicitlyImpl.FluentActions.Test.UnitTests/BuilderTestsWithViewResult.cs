using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers;
using Xunit;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public class BuilderTestsWithViewResult
    {
        [Fact(DisplayName = "no usings, no To, returns ViewResult")]
        public void FluentControllerBuilder_FluentActionNoUsingsNoToReturnsView()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .ToView("~/Path/To/ViewWithoutModel.cshtml"),
                typeof(ControllerWithNoUsingsNoToReturnsView),
                null);
        }

        [Fact(DisplayName = "1 body (string), no To, returns ViewResult")]
        public void FluentControllerBuilder_FluentAction1BodyNoToReturnsView()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingBody<string>()
                    .ToView("~/Path/To/ViewWithStringModel.cshtml"),
                typeof(ControllerPassing1BodyReturnsView),
                new object[] { "Text" });
        }

        [Fact(DisplayName = "1 body (string), 1 route param (string), no To, returns ViewResult")]
        public void FluentControllerBuilder_FluentAction1Body1RouteParamNoToReturnsView()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url/{unused}", HttpMethod.Get)
                    .UsingBody<string>()
                    .UsingRouteParameter<string>("unused")
                    .ToView("~/Path/To/ViewWithStringModel.cshtml"),
                typeof(ControllerWith1Body1RouteParamPassing1BodyReturnsView),
                new object[] { "Text", "Unused" });
        }

        [Fact(DisplayName = "no usings, 1 To, returns ViewResult")]
        public void FluentControllerBuilder_FluentActionNoUsings1ToReturnsView()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .To(() => "Hello World!")
                    .ToView("~/Path/To/ViewWithStringModel.cshtml"),
                typeof(ControllerWithNoUsingsXToReturnsView),
                null);
        }

        [Fact(DisplayName = "no usings, 3 To, returns ViewResult")]
        public void FluentControllerBuilder_FluentActionNoUsings3ToReturnsView()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .To(() => "Hello")
                    .UsingResult()
                    .To(text => $"{text} World")
                    .UsingResult()
                    .To(text => $"{text}!")
                    .ToView("~/Path/To/ViewWithStringModel.cshtml"),
                typeof(ControllerWithNoUsingsXToReturnsView),
                null);
        }

        [Fact(DisplayName = "no usings, 1 Do, returns ViewResult")]
        public void FluentControllerBuilder_FluentActionNoUsings1DoReturnsView()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .Do(() => { /* woop woop */ })
                    .ToView("~/Path/To/ViewWithoutModel.cshtml"),
                typeof(ControllerWithNoUsingsXDoReturnsView),
                null);
        }

        [Fact(DisplayName = "no usings, 3 Do, returns ViewResult")]
        public void FluentControllerBuilder_FluentActionNoUsings3DoReturnsView()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .Do(() => { /* woop woop */ })
                    .Do(() => { /* woop woop */ })
                    .Do(() => { /* woop woop */ })
                    .ToView("~/Path/To/ViewWithoutModel.cshtml"),
                typeof(ControllerWithNoUsingsXDoReturnsView),
                null);
        }

        [Fact(DisplayName = "no usings, 1 Do, 1 To, returns ViewResult")]
        public void FluentControllerBuilder_FluentActionNoUsings1Do1ToReturnsView()
        {
            var foo = "bar";

            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .Do(() => { foo = "baz"; })
                    .To(() => foo)
                    .ToView("~/Path/To/ViewWithStringModel.cshtml"),
                typeof(ControllerWithNoUsings1Do1ToReturnsView),
                null);
        }

        [Fact(DisplayName = "1 body (string), returns ViewResult")]
        public void FluentControllerBuilder_FluentAction1BodyReturnsView()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingBody<string>()
                    .To(name => $"Hello {name}!")
                    .ToView("~/Path/To/ViewWithStringModel.cshtml"),
                typeof(ControllerWith1BodyReturnsView),
                new object[] { "Bob" });
        }

        [Fact(DisplayName = "1 body (string), 1 Do, 1 To, returns ViewResult")]
        public void FluentControllerBuilder_FluentAction1Body1DoReturnsView()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .Do(() => { /* woop woop */ })
                    .UsingBody<string>()
                    .To(name => $"Hello {name}!")
                    .ToView("~/Path/To/ViewWithStringModel.cshtml"),
                typeof(ControllerWith1BodyReturnsView),
                new object[] { "Bob" });
        }

        [Fact(DisplayName = "1 body (string), 1 route param (string), returns ViewResult")]
        public void FluentControllerBuilder_FluentAction1Body1RouteParamReturnsView()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/{lastName}", HttpMethod.Get)
                    .UsingBody<string>()
                    .UsingRouteParameter<string>("lastName")
                    .To((firstName, lastName) => $"Hello {firstName} {lastName}!")
                    .ToView("~/Path/To/ViewWithStringModel.cshtml"),
                typeof(ControllerWith1Body1RouteParamReturnsView),
                new object[] { "Bob", "Bobsson" });
        }

        [Fact(DisplayName = "1 body (string), 1 route param (string), 2 To, returns ViewResult")]
        public void FluentControllerBuilder_FluentAction1Body1RouteParam2ToReturnsView()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/{lastName}", HttpMethod.Get)
                    .UsingBody<string>()
                    .To(firstName => $"Hello {firstName}")
                    .UsingRouteParameter<string>("lastName")
                    .To(lastName => $"{lastName}!")
                    .ToView("~/Path/To/ViewWithStringModel.cshtml"),
                typeof(ControllerWith1Body1RouteParam2ToReturnsView),
                new object[] { "Bob", "Bobsson" });
        }
    }
}
