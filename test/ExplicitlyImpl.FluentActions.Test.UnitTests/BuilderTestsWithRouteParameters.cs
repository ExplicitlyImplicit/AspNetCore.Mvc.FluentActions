using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers;
using System;
using Xunit;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public class BuilderTestsWithRouteParameters
    {
        [Fact(DisplayName = "1 route parameter (string), returns string")]
        public void FluentControllerBuilder_FluentActionUsingRouteParmeterReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticAction(
                new FluentAction("/route/{name}", HttpMethod.Get)
                    .UsingRouteParameter<string>("name")
                    .To(name => $"Hello {name}!"),
                typeof(ControllerWithRouteParameterReturnsString),
                new object[] { "Charlie" });
        }

        [Fact(DisplayName = "1 route parameter (string) with used default value, returns string")]
        public void FluentControllerBuilder_FluentActionUsingRouteParmeterWithUsedDefaultValueReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticAction(
                new FluentAction("/route/{name}", HttpMethod.Get)
                    .UsingRouteParameter<string>("name", "Hanzel")
                    .To(name => $"Hello {name}!"),
                typeof(ControllerWithRouteParameterAndDefaultValueReturnsString),
                new object[] { Type.Missing });
        }

        [Fact(DisplayName = "1 route parameter (string) with unused default value, returns string")]
        public void FluentControllerBuilder_FluentActionUsingRouteParmeterWithUnusedDefaultValueReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticAction(
                new FluentAction("/route/{name}", HttpMethod.Get)
                    .UsingRouteParameter<string>("name", "Hanzel")
                    .To(name => $"Hello {name}!"),
                typeof(ControllerWithRouteParameterAndDefaultValueReturnsString),
                new object[] { "Charlie" });
        }

        [Fact(DisplayName = "2 route parameters (string, identical), returns string")]
        public void FluentControllerBuilder_FluentActionUsingTwoIdenticalRouteParmetersReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticAction(
                new FluentAction("/route/{name}", HttpMethod.Get)
                    .UsingRouteParameter<string>("name")
                    .UsingRouteParameter<string>("name")
                    .To((name1, name2) => $"Hello {name1}! I said hello {name2}!"),
                typeof(ControllerWithTwoIdenticalRouteParametersReturnsString),
                new object[] { "Charlie" });
        }
    }
}
