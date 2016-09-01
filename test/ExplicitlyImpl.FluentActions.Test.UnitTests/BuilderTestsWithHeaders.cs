using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers;
using System;
using Xunit;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public class BuilderTestsWithHeaders
    {
        [Fact(DisplayName = "1 header (string), returns string")]
        public void FluentControllerBuilder_FluentActionUsingHeaderReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticAction(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingHeader<string>("name")
                    .To(name => $"Hello {name}!"),
                typeof(ControllerWithHeaderReturnsString),
                new object[] { "Charlie" });
        }

        [Fact(DisplayName = "1 header (string) with used default value, returns string")]
        public void FluentControllerBuilder_FluentActionUsingHeaderWithUsedDefaultValueReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticAction(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingHeader<string>("name", "Hanzel")
                    .To(name => $"Hello {name}!"),
                typeof(ControllerWithHeaderAndDefaultValueReturnsString),
                new object[] { Type.Missing });
        }

        [Fact(DisplayName = "1 header (string) with unused default value, returns string")]
        public void FluentControllerBuilder_FluentActionUsingHeaderWithUnusedDefaultValueReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticAction(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingHeader<string>("name", "Hanzel")
                    .To(name => $"Hello {name}!"),
                typeof(ControllerWithHeaderAndDefaultValueReturnsString),
                new object[] { "Charlie" });
        }

        [Fact(DisplayName = "2 headers (string, identical), returns string")]
        public void FluentControllerBuilder_FluentActionUsingTwoIdenticalHeadersReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticAction(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingHeader<string>("name")
                    .UsingHeader<string>("name")
                    .To((name1, name2) => $"Hello {name1}! I said hello {name2}!"),
                typeof(ControllerWithTwoIdenticalHeadersReturnsString),
                new object[] { "Charlie" });
        }
    }
}
