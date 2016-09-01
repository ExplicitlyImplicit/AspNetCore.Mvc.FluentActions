using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers;
using System;
using Xunit;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public class BuilderTestsWithForm
    {
        [Fact(DisplayName = "1 form (string), returns string")]
        public void FluentControllerBuilder_FluentActionUsingFormReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticAction(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingForm<string>()
                    .To(name => $"Hello {name}!"),
                typeof(ControllerWithFormReturnsString),
                new object[] { "Charlie" });
        }

        [Fact(DisplayName = "1 form (string) with used default value, returns string")]
        public void FluentControllerBuilder_FluentActionUsingFormWithUsedDefaultValueReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticAction(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingForm<string>("Hanzel")
                    .To(name => $"Hello {name}!"),
                typeof(ControllerWithFormAndDefaultValueReturnsString),
                new object[] { Type.Missing });
        }

        [Fact(DisplayName = "1 form (string) with unused default value, returns string")]
        public void FluentControllerBuilder_FluentActionUsingFormWithUnusedDefaultValueReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticAction(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingForm<string>("Hanzel")
                    .To(name => $"Hello {name}!"),
                typeof(ControllerWithFormAndDefaultValueReturnsString),
                new object[] { "Charlie" });
        }

        [Fact(DisplayName = "2 form (string, identical), returns string")]
        public void FluentControllerBuilder_FluentActionUsingTwoIdenticalFormsReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticAction(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingForm<string>()
                    .UsingForm<string>()
                    .To((name1, name2) => $"Hello {name1}! I said hello {name2}!"),
                typeof(ControllerWithTwoIdenticalFormsReturnsString),
                new object[] { "Charlie" });
        }
    }
}
