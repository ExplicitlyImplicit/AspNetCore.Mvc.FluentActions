using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers;
using System;
using Xunit;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public class BuilderTestsWithBody
    {
        [Fact(DisplayName = "1 body (string), returns string")]
        public void FluentControllerBuilder_FluentActionUsingBodyReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticAction(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingBody<string>()
                    .To(name => $"Hello {name}!"),
                typeof(ControllerWithBodyReturnsString),
                new object[] { "Charlie" });
        }

        [Fact(DisplayName = "1 body (string) with used default value, returns string")]
        public void FluentControllerBuilder_FluentActionUsingBodyWithUsedDefaultValueReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticAction(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingBody<string>("Hanzel")
                    .To(name => $"Hello {name}!"),
                typeof(ControllerWithBodyAndDefaultValueReturnsString),
                new object[] { Type.Missing });
        }

        [Fact(DisplayName = "1 body (string) with unused default value, returns string")]
        public void FluentControllerBuilder_FluentActionUsingBodyWithUnusedDefaultValueReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticAction(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingBody<string>("Hanzel")
                    .To(name => $"Hello {name}!"),
                typeof(ControllerWithBodyAndDefaultValueReturnsString),
                new object[] { "Charlie" });
        }

        [Fact(DisplayName = "2 body (string, identical), returns string")]
        public void FluentControllerBuilder_FluentActionUsingTwoIdenticalBodysReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticAction(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingBody<string>()
                    .UsingBody<string>()
                    .To((name1, name2) => $"Hello {name1}! I said hello {name2}!"),
                typeof(ControllerWithTwoIdenticalBodysReturnsString),
                new object[] { "Charlie" });
        }
    }
}
