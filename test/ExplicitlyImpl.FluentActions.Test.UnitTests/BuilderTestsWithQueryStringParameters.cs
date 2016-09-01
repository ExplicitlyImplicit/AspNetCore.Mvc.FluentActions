using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers;
using System;
using Xunit;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public class BuilderTestsWithQueryStringParameters
    {
        [Fact(DisplayName = "1 query string parameter (string), returns string")]
        public void FluentControllerBuilder_FluentActionUsingQueryStringParmeterReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticAction(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingQueryStringParameter<string>("name")
                    .To(name => $"Hello {name}!"),
                typeof(ControllerWithQueryStringParameterReturnsString),
                new object[] { "Charlie" });
        }

        [Fact(DisplayName = "1 query string parameter (string) with used default value, returns string")]
        public void FluentControllerBuilder_FluentActionUsingQueryStringParmeterWithUsedDefaultValueReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticAction(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingQueryStringParameter<string>("name", "Hanzel")
                    .To(name => $"Hello {name}!"),
                typeof(ControllerWithQueryStringParameterAndDefaultValueReturnsString),
                new object[] { Type.Missing });
        }

        [Fact(DisplayName = "1 query string parameter (string) with unused default value, returns string")]
        public void FluentControllerBuilder_FluentActionUsingQueryStringParmeterWithUnusedDefaultValueReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticAction(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingQueryStringParameter<string>("name", "Hanzel")
                    .To(name => $"Hello {name}!"),
                typeof(ControllerWithQueryStringParameterAndDefaultValueReturnsString),
                new object[] { "Charlie" });
        }

        [Fact(DisplayName = "2 query string parameters (string, identical), returns string")]
        public void FluentControllerBuilder_FluentActionUsingTwoIdenticalQueryStringParmetersReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticAction(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingQueryStringParameter<string>("name")
                    .UsingQueryStringParameter<string>("name")
                    .To((name1, name2) => $"Hello {name1}! I said hello {name2}!"),
                typeof(ControllerWithTwoIdenticalQueryStringParametersReturnsString),
                new object[] { "Charlie" });
        }
    }
}
