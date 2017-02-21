using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public class BuilderTestsWithRouteParameters
    {
        [Fact(DisplayName = "1 route parameter (string), returns string")]
        public void FluentControllerBuilder_FluentActionUsingRouteParmeterReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/{name}", HttpMethod.Get)
                    .UsingRouteParameter<string>("name")
                    .To(name => $"Hello {name}!"),
                typeof(ControllerWithRouteParameterReturnsString),
                new object[] { "Charlie" });
        }

        [Fact(DisplayName = "1 route parameter (string), returns string async")]
        public void FluentControllerBuilder_FluentActionUsingRouteParmeterReturnsStringAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/{name}", HttpMethod.Get)
                    .UsingRouteParameter<string>("name")
                    .To(async name => { await Task.Delay(1); return $"Hello {name}!"; }),
                typeof(ControllerWithRouteParameterReturnsStringAsync),
                new object[] { "Charlie" });
        }

        [Fact(DisplayName = "1 route parameter (string) with used default value, returns string")]
        public void FluentControllerBuilder_FluentActionUsingRouteParmeterWithUsedDefaultValueReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/{name}", HttpMethod.Get)
                    .UsingRouteParameter<string>("name", "Hanzel")
                    .To(name => $"Hello {name}!"),
                typeof(ControllerWithRouteParameterAndDefaultValueReturnsString),
                new object[] { Type.Missing });
        }

        [Fact(DisplayName = "1 route parameter (string) with used default value, returns string async")]
        public void FluentControllerBuilder_FluentActionUsingRouteParmeterWithUsedDefaultValueReturnsStringAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/{name}", HttpMethod.Get)
                    .UsingRouteParameter<string>("name", "Hanzel")
                    .To(async name => { await Task.Delay(1); return $"Hello {name}!"; }),
                typeof(ControllerWithRouteParameterAndDefaultValueReturnsStringAsync),
                new object[] { Type.Missing });
        }

        [Fact(DisplayName = "1 route parameter (string) with unused default value, returns string")]
        public void FluentControllerBuilder_FluentActionUsingRouteParmeterWithUnusedDefaultValueReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/{name}", HttpMethod.Get)
                    .UsingRouteParameter<string>("name", "Hanzel")
                    .To(name => $"Hello {name}!"),
                typeof(ControllerWithRouteParameterAndDefaultValueReturnsString),
                new object[] { "Charlie" });
        }

        [Fact(DisplayName = "1 route parameter (string) with unused default value, returns string async")]
        public void FluentControllerBuilder_FluentActionUsingRouteParmeterWithUnusedDefaultValueReturnsStringAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/{name}", HttpMethod.Get)
                    .UsingRouteParameter<string>("name", "Hanzel")
                    .To(async name => { await Task.Delay(1); return $"Hello {name}!"; }),
                typeof(ControllerWithRouteParameterAndDefaultValueReturnsStringAsync),
                new object[] { "Charlie" });
        }

        [Fact(DisplayName = "2 route parameters (string, identical), returns string")]
        public void FluentControllerBuilder_FluentActionUsingTwoIdenticalRouteParmetersReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/{name}", HttpMethod.Get)
                    .UsingRouteParameter<string>("name")
                    .UsingRouteParameter<string>("name")
                    .To((name1, name2) => $"Hello {name1}! I said hello {name2}!"),
                typeof(ControllerWithTwoIdenticalRouteParametersReturnsString),
                new object[] { "Charlie" });
        }

        [Fact(DisplayName = "2 route parameters (string, identical), returns string async")]
        public void FluentControllerBuilder_FluentActionUsingTwoIdenticalRouteParmetersReturnsStringAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/{name}", HttpMethod.Get)
                    .UsingRouteParameter<string>("name")
                    .UsingRouteParameter<string>("name")
                    .To(async (name1, name2) => { await Task.Delay(1); return $"Hello {name1}! I said hello {name2}!"; }),
                typeof(ControllerWithTwoIdenticalRouteParametersReturnsStringAsync),
                new object[] { "Charlie" });
        }
    }
}
