using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public class BuilderTestsWithBody
    {
        [Fact(DisplayName = "1 body (string), returns string")]
        public void FluentControllerBuilder_FluentActionUsingBodyReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingBody<string>()
                    .To(name => $"Hello {name}!"),
                typeof(ControllerWithBodyReturnsString),
                new object[] { "Charlie" });
        }

        [Fact(DisplayName = "1 body (string), returns string async")]
        public void FluentControllerBuilder_FluentActionUsingBodyReturnsStringAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingBody<string>()
                    .To(async name => { await Task.Delay(1);  return $"Hello {name}!"; }),
                typeof(ControllerWithBodyReturnsStringAsync),
                new object[] { "Charlie" });
        }

        [Fact(DisplayName = "1 body (string) with used default value, returns string")]
        public void FluentControllerBuilder_FluentActionUsingBodyWithUsedDefaultValueReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingBody<string>("Hanzel")
                    .To(name => $"Hello {name}!"),
                typeof(ControllerWithBodyAndDefaultValueReturnsString),
                new object[] { Type.Missing });
        }

        [Fact(DisplayName = "1 body (string) with used default value, returns string async")]
        public void FluentControllerBuilder_FluentActionUsingBodyWithUsedDefaultValueReturnsStringAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingBody<string>("Hanzel")
                    .To(async name => { await Task.Delay(1); return $"Hello {name}!"; }),
                typeof(ControllerWithBodyAndDefaultValueReturnsStringAsync),
                new object[] { Type.Missing });
        }

        [Fact(DisplayName = "1 body (string) with unused default value, returns string")]
        public void FluentControllerBuilder_FluentActionUsingBodyWithUnusedDefaultValueReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingBody<string>("Hanzel")
                    .To(name => $"Hello {name}!"),
                typeof(ControllerWithBodyAndDefaultValueReturnsString),
                new object[] { "Charlie" });
        }

        [Fact(DisplayName = "1 body (string) with unused default value, returns string")]
        public void FluentControllerBuilder_FluentActionUsingBodyWithUnusedDefaultValueReturnsStringAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingBody<string>("Hanzel")
                    .To(async name => { await Task.Delay(1); return $"Hello {name}!"; }),
                typeof(ControllerWithBodyAndDefaultValueReturnsStringAsync),
                new object[] { "Charlie" });
        }

        [Fact(DisplayName = "2 body (string, identical), returns string")]
        public void FluentControllerBuilder_FluentActionUsingTwoIdenticalBodysReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingBody<string>()
                    .UsingBody<string>()
                    .To(async (name1, name2) => { await Task.Delay(1); return $"Hello {name1}! I said hello {name2}!"; }),
                typeof(ControllerWithTwoIdenticalBodysReturnsStringAsync),
                new object[] { "Charlie" });
        }
    }
}
