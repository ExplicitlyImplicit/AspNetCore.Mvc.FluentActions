using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public class BuilderTestsWithHeaders
    {
        [Fact(DisplayName = "1 header (string), returns string")]
        public void FluentControllerBuilder_FluentActionUsingHeaderReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingHeader<string>("name")
                    .To(name => $"Hello {name}!"),
                typeof(ControllerWithHeaderReturnsString),
                new object[] { "Charlie" });
        }

        [Fact(DisplayName = "1 header (string), returns string async")]
        public void FluentControllerBuilder_FluentActionUsingHeaderReturnsStringAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingHeader<string>("name")
                    .To(async name => { await Task.Delay(1); return $"Hello {name}!"; }),
                typeof(ControllerWithHeaderReturnsStringAsync),
                new object[] { "Charlie" });
        }

        [Fact(DisplayName = "1 header (string) with used default value, returns string")]
        public void FluentControllerBuilder_FluentActionUsingHeaderWithUsedDefaultValueReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingHeader<string>("name", "Hanzel")
                    .To(name => $"Hello {name}!"),
                typeof(ControllerWithHeaderAndDefaultValueReturnsString),
                new object[] { Type.Missing });
        }

        [Fact(DisplayName = "1 header (string) with used default value, returns string async")]
        public void FluentControllerBuilder_FluentActionUsingHeaderWithUsedDefaultValueReturnsStringAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingHeader<string>("name", "Hanzel")
                    .To(async name => { await Task.Delay(1); return $"Hello {name}!"; }),
                typeof(ControllerWithHeaderAndDefaultValueReturnsStringAsync),
                new object[] { Type.Missing });
        }

        [Fact(DisplayName = "1 header (string) with unused default value, returns string")]
        public void FluentControllerBuilder_FluentActionUsingHeaderWithUnusedDefaultValueReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingHeader<string>("name", "Hanzel")
                    .To(name => $"Hello {name}!"),
                typeof(ControllerWithHeaderAndDefaultValueReturnsString),
                new object[] { "Charlie" });
        }

        [Fact(DisplayName = "1 header (string) with unused default value, returns string async")]
        public void FluentControllerBuilder_FluentActionUsingHeaderWithUnusedDefaultValueReturnsStringAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingHeader<string>("name", "Hanzel")
                    .To(async name => { await Task.Delay(1); return $"Hello {name}!"; }),
                typeof(ControllerWithHeaderAndDefaultValueReturnsStringAsync),
                new object[] { "Charlie" });
        }

        [Fact(DisplayName = "2 headers (string, identical), returns string")]
        public void FluentControllerBuilder_FluentActionUsingTwoIdenticalHeadersReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingHeader<string>("name")
                    .UsingHeader<string>("name")
                    .To((name1, name2) => $"Hello {name1}! I said hello {name2}!"),
                typeof(ControllerWithTwoIdenticalHeadersReturnsString),
                new object[] { "Charlie" });
        }

        [Fact(DisplayName = "2 headers (string, identical), returns string async")]
        public void FluentControllerBuilder_FluentActionUsingTwoIdenticalHeadersReturnsStringAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingHeader<string>("name")
                    .UsingHeader<string>("name")
                    .To(async (name1, name2) => { await Task.Delay(1); return $"Hello {name1}! I said hello {name2}!"; }),
                typeof(ControllerWithTwoIdenticalHeadersReturnsStringAsync),
                new object[] { "Charlie" });
        }
    }
}
