using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers;
using System.Threading.Tasks;
using Xunit;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public class BuilderTestsWithPartialViewResultAsync
    {
        [Fact(DisplayName = "no usings, 1 To, returns PartialViewResult async")]
        public void FluentControllerBuilder_FluentActionNoUsings1ToReturnsPartialViewAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .To(async () => { await Task.Delay(1); return "Hello World!"; })
                    .ToPartialView("~/Path/To/PartialViewWithStringModel.cshtml"),
                typeof(ControllerWithNoUsingsXToReturnsPartialViewAsync),
                null);
        }

        [Fact(DisplayName = "no usings, 3 To, returns PartialViewResult async")]
        public void FluentControllerBuilder_FluentActionNoUsings3ToReturnsPartialViewAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .To(async () => { await Task.Delay(1); return "Hello"; })
                    .UsingResult()
                    .To(async text => { await Task.Delay(1); return $"{text} World"; })
                    .UsingResult()
                    .To(async text => { await Task.Delay(1); return $"{text}!"; })
                    .ToPartialView("~/Path/To/PartialViewWithStringModel.cshtml"),
                typeof(ControllerWithNoUsingsXToReturnsPartialViewAsync),
                null);
        }

        [Fact(DisplayName = "no usings, 1 Do, returns PartialViewResult async")]
        public void FluentControllerBuilder_FluentActionNoUsings1DoReturnsPartialViewAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .DoAsync(async () => { await Task.Delay(1); })
                    .ToPartialView("~/Path/To/PartialViewWithoutModel.cshtml"),
                typeof(ControllerWithNoUsingsXDoReturnsPartialViewAsync),
                null);
        }

        [Fact(DisplayName = "no usings, 3 Do, returns PartialViewResult async")]
        public void FluentControllerBuilder_FluentActionNoUsings3DoReturnsPartialViewAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .DoAsync(async () => { await Task.Delay(1); })
                    .DoAsync(async () => { await Task.Delay(1); })
                    .DoAsync(async () => { await Task.Delay(1); })
                    .ToPartialView("~/Path/To/PartialViewWithoutModel.cshtml"),
                typeof(ControllerWithNoUsingsXDoReturnsPartialViewAsync),
                null);
        }

        [Fact(DisplayName = "no usings, 1 Do, 1 To, returns PartialViewResult async")]
        public void FluentControllerBuilder_FluentActionNoUsings1Do1ToReturnsPartialViewAsync()
        {
            var foo = "bar";

            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .DoAsync(async () => { await Task.Delay(1); foo = "baz"; })
                    .To(async () => { await Task.Delay(1); return foo; })
                    .ToPartialView("~/Path/To/PartialViewWithStringModel.cshtml"),
                typeof(ControllerWithNoUsings1Do1ToReturnsPartialViewAsync),
                null);
        }

        [Fact(DisplayName = "1 body (string), returns PartialViewResult async")]
        public void FluentControllerBuilder_FluentAction1BodyReturnsPartialViewAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingBody<string>()
                    .To(async (name) => { await Task.Delay(1); return $"Hello {name}!"; })
                    .ToPartialView("~/Path/To/PartialViewWithStringModel.cshtml"),
                typeof(ControllerWith1BodyReturnsPartialViewAsync),
                new object[] { "Bob" });
        }

        [Fact(DisplayName = "1 body (string), 1 Do, 1 To, returns PartialViewResult async")]
        public void FluentControllerBuilder_FluentAction1Body1DoReturnsPartialViewAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .DoAsync(async () => { await Task.Delay(1); })
                    .UsingBody<string>()
                    .To(async (name) => { await Task.Delay(1); return $"Hello {name}!"; })
                    .ToPartialView("~/Path/To/PartialViewWithStringModel.cshtml"),
                typeof(ControllerWith1BodyReturnsPartialViewAsync),
                new object[] { "Bob" });
        }

        [Fact(DisplayName = "1 body (string), 1 route param (string), returns PartialViewResult async")]
        public void FluentControllerBuilder_FluentAction1Body1RouteParamReturnsPartialViewAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/{lastName}", HttpMethod.Get)
                    .UsingBody<string>()
                    .UsingRouteParameter<string>("lastName")
                    .To(async (firstName, lastName) => { await Task.Delay(1); return $"Hello {firstName} {lastName}!"; })
                    .ToPartialView("~/Path/To/PartialViewWithStringModel.cshtml"),
                typeof(ControllerWith1Body1RouteParamReturnsPartialViewAsync),
                new object[] { "Bob", "Bobsson" });
        }

        [Fact(DisplayName = "1 body (string), 1 route param (string), 2 To, returns PartialViewResult async")]
        public void FluentControllerBuilder_FluentAction1Body1RouteParam2ToReturnsPartialViewAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/{lastName}", HttpMethod.Get)
                    .UsingBody<string>()
                    .To(async firstName => { await Task.Delay(1); return $"Hello {firstName}"; })
                    .UsingRouteParameter<string>("lastName")
                    .To(async lastName => { await Task.Delay(1); return $"{lastName}!"; })
                    .ToPartialView("~/Path/To/PartialViewWithStringModel.cshtml"),
                typeof(ControllerWith1Body1RouteParam2ToReturnsPartialViewAsync),
                new object[] { "Bob", "Bobsson" });
        }
    }
}
