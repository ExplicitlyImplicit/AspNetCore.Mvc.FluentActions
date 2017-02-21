using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers;
using System.Threading.Tasks;
using Xunit;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public class BuilderTestsWithViewResultAsync
    {
        [Fact(DisplayName = "no usings, 1 To, returns ViewResult async")]
        public void FluentControllerBuilder_FluentActionNoUsings1ToReturnsViewAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .To(async () => { await Task.Delay(1); return "Hello World!"; })
                    .ToView("~/Path/To/ViewWithStringModel.cshtml"),
                typeof(ControllerWithNoUsingsXToReturnsViewAsync),
                null);
        }

        [Fact(DisplayName = "no usings, 3 To, returns ViewResult async")]
        public void FluentControllerBuilder_FluentActionNoUsings3ToReturnsViewAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .To(async () => { await Task.Delay(1); return "Hello"; })
                    .UsingResultFromHandler()
                    .To(async text => { await Task.Delay(1); return $"{text} World"; })
                    .UsingResultFromHandler()
                    .To(async text => { await Task.Delay(1); return $"{text}!"; })
                    .ToView("~/Path/To/ViewWithStringModel.cshtml"),
                typeof(ControllerWithNoUsingsXToReturnsViewAsync),
                null);
        }

        [Fact(DisplayName = "no usings, 1 Do, returns ViewResult async")]
        public void FluentControllerBuilder_FluentActionNoUsings1DoReturnsViewAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .DoAsync(async () => { await Task.Delay(1); })
                    .ToView("~/Path/To/ViewWithoutModel.cshtml"),
                typeof(ControllerWithNoUsingsXDoReturnsViewAsync),
                null);
        }

        [Fact(DisplayName = "no usings, 3 Do, returns ViewResult async")]
        public void FluentControllerBuilder_FluentActionNoUsings3DoReturnsViewAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .DoAsync(async () => { await Task.Delay(1); })
                    .DoAsync(async () => { await Task.Delay(1); })
                    .DoAsync(async () => { await Task.Delay(1); })
                    .ToView("~/Path/To/ViewWithoutModel.cshtml"),
                typeof(ControllerWithNoUsingsXDoReturnsViewAsync),
                null);
        }

        [Fact(DisplayName = "no usings, 1 Do, 1 To, returns ViewResult async")]
        public void FluentControllerBuilder_FluentActionNoUsings1Do1ToReturnsViewAsync()
        {
            var foo = "bar";

            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .DoAsync(async () => { await Task.Delay(1); foo = "baz"; })
                    .To(async () => { await Task.Delay(1); return foo; })
                    .ToView("~/Path/To/ViewWithStringModel.cshtml"),
                typeof(ControllerWithNoUsings1Do1ToReturnsViewAsync),
                null);
        }

        [Fact(DisplayName = "1 body (string), returns ViewResult async")]
        public void FluentControllerBuilder_FluentAction1BodyReturnsViewAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingBody<string>()
                    .To(async (name) => { await Task.Delay(1); return $"Hello {name}!"; })
                    .ToView("~/Path/To/ViewWithStringModel.cshtml"),
                typeof(ControllerWith1BodyReturnsViewAsync),
                new object[] { "Bob" });
        }

        [Fact(DisplayName = "1 body (string), 1 Do, 1 To, returns ViewResult async")]
        public void FluentControllerBuilder_FluentAction1Body1DoReturnsViewAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .DoAsync(async () => { await Task.Delay(1); })
                    .UsingBody<string>()
                    .To(async (name) => { await Task.Delay(1); return $"Hello {name}!"; })
                    .ToView("~/Path/To/ViewWithStringModel.cshtml"),
                typeof(ControllerWith1BodyReturnsViewAsync),
                new object[] { "Bob" });
        }

        [Fact(DisplayName = "1 body (string), 1 route param (string), returns ViewResult async")]
        public void FluentControllerBuilder_FluentAction1Body1RouteParamReturnsViewAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/{lastName}", HttpMethod.Get)
                    .UsingBody<string>()
                    .UsingRouteParameter<string>("lastName")
                    .To(async (firstName, lastName) => { await Task.Delay(1); return $"Hello {firstName} {lastName}!"; })
                    .ToView("~/Path/To/ViewWithStringModel.cshtml"),
                typeof(ControllerWith1Body1RouteParamReturnsViewAsync),
                new object[] { "Bob", "Bobsson" });
        }

        [Fact(DisplayName = "1 body (string), 1 route param (string), 2 To, returns ViewResult async")]
        public void FluentControllerBuilder_FluentAction1Body1RouteParam2ToReturnsViewAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/{lastName}", HttpMethod.Get)
                    .UsingBody<string>()
                    .To(async firstName => { await Task.Delay(1); return $"Hello {firstName}"; })
                    .UsingRouteParameter<string>("lastName")
                    .To(async lastName => { await Task.Delay(1); return $"{lastName}!"; })
                    .ToView("~/Path/To/ViewWithStringModel.cshtml"),
                typeof(ControllerWith1Body1RouteParam2ToReturnsViewAsync),
                new object[] { "Bob", "Bobsson" });
        }
    }
}
