using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers;
using Xunit;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public class BuilderTestsWithPartialViewResult
    {
        [Fact(DisplayName = "no usings, no To, returns PartialViewResult")]
        public void FluentControllerBuilder_FluentActionNoUsingsNoToReturnsPartialView()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .ToPartialView("~/Path/To/PartialViewWithoutModel.cshtml"),
                typeof(ControllerWithNoUsingsNoToReturnsPartialView),
                null);
        }

        [Fact(DisplayName = "1 body (string), no To, returns PartialViewResult")]
        public void FluentControllerBuilder_FluentAction1BodyNoToReturnsPartialView()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingBody<string>()
                    .ToPartialView("~/Path/To/PartialViewWithStringModel.cshtml"),
                typeof(ControllerPassing1BodyReturnsPartialView),
                new object[] { "Text" });
        }

        [Fact(DisplayName = "1 body (string), 1 route param (string), no To, returns PartialViewResult")]
        public void FluentControllerBuilder_FluentAction1Body1RouteParamNoToReturnsPartialView()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url/{unused}", HttpMethod.Get)
                    .UsingBody<string>()
                    .UsingRouteParameter<string>("unused")
                    .ToPartialView("~/Path/To/PartialViewWithStringModel.cshtml"),
                typeof(ControllerWith1Body1RouteParamPassing1BodyReturnsPartialView),
                new object[] { "Text", "Unused" });
        }

        [Fact(DisplayName = "no usings, 1 To, returns PartialViewResult")]
        public void FluentControllerBuilder_FluentActionNoUsings1ToReturnsPartialView()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .To(() => "Hello World!")
                    .ToPartialView("~/Path/To/PartialViewWithStringModel.cshtml"),
                typeof(ControllerWithNoUsingsXToReturnsPartialView),
                null);
        }

        [Fact(DisplayName = "no usings, 3 To, returns PartialViewResult")]
        public void FluentControllerBuilder_FluentActionNoUsings3ToReturnsPartialView()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .To(() => "Hello")
                    .UsingResult()
                    .To(text => $"{text} World")
                    .UsingResult()
                    .To(text => $"{text}!")
                    .ToPartialView("~/Path/To/PartialViewWithStringModel.cshtml"),
                typeof(ControllerWithNoUsingsXToReturnsPartialView),
                null);
        }

        [Fact(DisplayName = "no usings, 1 Do, returns PartialViewResult")]
        public void FluentControllerBuilder_FluentActionNoUsings1DoReturnsPartialView()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .Do(() => { /* woop woop */ })
                    .ToPartialView("~/Path/To/PartialViewWithoutModel.cshtml"),
                typeof(ControllerWithNoUsingsXDoReturnsPartialView),
                null);
        }

        [Fact(DisplayName = "no usings, 3 Do, returns PartialViewResult")]
        public void FluentControllerBuilder_FluentActionNoUsings3DoReturnsPartialView()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .Do(() => { /* woop woop */ })
                    .Do(() => { /* woop woop */ })
                    .Do(() => { /* woop woop */ })
                    .ToPartialView("~/Path/To/PartialViewWithoutModel.cshtml"),
                typeof(ControllerWithNoUsingsXDoReturnsPartialView),
                null);
        }

        [Fact(DisplayName = "no usings, 1 Do, 1 To, returns PartialViewResult")]
        public void FluentControllerBuilder_FluentActionNoUsings1Do1ToReturnsPartialView()
        {
            var foo = "bar";

            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .Do(() => { foo = "baz"; })
                    .To(() => foo)
                    .ToPartialView("~/Path/To/PartialViewWithStringModel.cshtml"),
                typeof(ControllerWithNoUsings1Do1ToReturnsPartialView),
                null);
        }

        [Fact(DisplayName = "1 body (string), returns PartialViewResult")]
        public void FluentControllerBuilder_FluentAction1BodyReturnsPartialView()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingBody<string>()
                    .To(name => $"Hello {name}!")
                    .ToPartialView("~/Path/To/PartialViewWithStringModel.cshtml"),
                typeof(ControllerWith1BodyReturnsPartialView),
                new object[] { "Bob" });
        }

        [Fact(DisplayName = "1 body (string), 1 Do, 1 To, returns PartialViewResult")]
        public void FluentControllerBuilder_FluentAction1Body1DoReturnsPartialView()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .Do(() => { /* woop woop */ })
                    .UsingBody<string>()
                    .To(name => $"Hello {name}!")
                    .ToPartialView("~/Path/To/PartialViewWithStringModel.cshtml"),
                typeof(ControllerWith1BodyReturnsPartialView),
                new object[] { "Bob" });
        }

        [Fact(DisplayName = "1 body (string), 1 route param (string), returns PartialViewResult")]
        public void FluentControllerBuilder_FluentAction1Body1RouteParamReturnsPartialView()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/{lastName}", HttpMethod.Get)
                    .UsingBody<string>()
                    .UsingRouteParameter<string>("lastName")
                    .To((firstName, lastName) => $"Hello {firstName} {lastName}!")
                    .ToPartialView("~/Path/To/PartialViewWithStringModel.cshtml"),
                typeof(ControllerWith1Body1RouteParamReturnsPartialView),
                new object[] { "Bob", "Bobsson" });
        }

        [Fact(DisplayName = "1 body (string), 1 route param (string), 2 To, returns PartialViewResult")]
        public void FluentControllerBuilder_FluentAction1Body1RouteParam2ToReturnsPartialView()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/{lastName}", HttpMethod.Get)
                    .UsingBody<string>()
                    .To(firstName => $"Hello {firstName}")
                    .UsingRouteParameter<string>("lastName")
                    .To(lastName => $"{lastName}!")
                    .ToPartialView("~/Path/To/PartialViewWithStringModel.cshtml"),
                typeof(ControllerWith1Body1RouteParam2ToReturnsPartialView),
                new object[] { "Bob", "Bobsson" });
        }
    }
}
