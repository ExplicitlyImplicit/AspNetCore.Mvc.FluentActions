using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public class BuilderTestsWithQueryStringParameters
    {
        [Fact(DisplayName = "1 query string parameter (string), returns string")]
        public void FluentControllerBuilder_FluentActionUsingQueryStringParmeterReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingQueryStringParameter<string>("name")
                    .To(name => $"Hello {name}!"),
                typeof(ControllerWithQueryStringParameterReturnsString),
                new object[] { "Charlie" });
        }

        [Fact(DisplayName = "1 query string parameter (string), returns string async")]
        public void FluentControllerBuilder_FluentActionUsingQueryStringParmeterReturnsStringAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingQueryStringParameter<string>("name")
                    .To(async name => { await Task.Delay(1); return $"Hello {name}!"; }),
                typeof(ControllerWithQueryStringParameterReturnsStringAsync),
                new object[] { "Charlie" });
        }

        [Fact(DisplayName = "1 query string parameter (string) with used default value, returns string")]
        public void FluentControllerBuilder_FluentActionUsingQueryStringParameterWithUsedDefaultValueReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingQueryStringParameter<string>("name", "Hanzel")
                    .To(name => $"Hello {name}!"),
                typeof(ControllerWithQueryStringParameterAndDefaultValueReturnsString),
                new object[] { Type.Missing });
        }

        [Fact(DisplayName = "1 query string parameter (string) with used default value, returns string async")]
        public void FluentControllerBuilder_FluentActionUsingQueryStringParameterWithUsedDefaultValueReturnsStringAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingQueryStringParameter<string>("name", "Hanzel")
                    .To(async name => { await Task.Delay(1); return $"Hello {name}!"; }),
                typeof(ControllerWithQueryStringParameterAndDefaultValueReturnsStringAsync),
                new object[] { Type.Missing });
        }

        [Fact(DisplayName = "1 query string parameter (string) with unused default value, returns string")]
        public void FluentControllerBuilder_FluentActionUsingQueryStringParameterWithUnusedDefaultValueReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingQueryStringParameter<string>("name", "Hanzel")
                    .To(name => $"Hello {name}!"),
                typeof(ControllerWithQueryStringParameterAndDefaultValueReturnsString),
                new object[] { "Charlie" });
        }

        [Fact(DisplayName = "1 query string parameter (string) with unused default value, returns string async")]
        public void FluentControllerBuilder_FluentActionUsingQueryStringParameterWithUnusedDefaultValueReturnsStringAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingQueryStringParameter<string>("name", "Hanzel")
                    .To(async name => { await Task.Delay(1); return $"Hello {name}!"; }),
                typeof(ControllerWithQueryStringParameterAndDefaultValueReturnsStringAsync),
                new object[] { "Charlie" });
        }

        [Fact(DisplayName = "2 query string parameters (string, identical), returns string")]
        public void FluentControllerBuilder_FluentActionUsingTwoIdenticalQueryStringParmetersReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingQueryStringParameter<string>("name")
                    .UsingQueryStringParameter<string>("name")
                    .To((name1, name2) => $"Hello {name1}! I said hello {name2}!"),
                typeof(ControllerWithTwoIdenticalQueryStringParametersReturnsString),
                new object[] { "Charlie" });
        }

        [Fact(DisplayName = "2 query string parameters (string, identical), returns string async")]
        public void FluentControllerBuilder_FluentActionUsingTwoIdenticalQueryStringParmetersReturnsStringAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingQueryStringParameter<string>("name")
                    .UsingQueryStringParameter<string>("name")
                    .To(async (name1, name2) => { await Task.Delay(1); return $"Hello {name1}! I said hello {name2}!"; }),
                typeof(ControllerWithTwoIdenticalQueryStringParametersReturnsStringAsync),
                new object[] { "Charlie" });
        }
    }
}
