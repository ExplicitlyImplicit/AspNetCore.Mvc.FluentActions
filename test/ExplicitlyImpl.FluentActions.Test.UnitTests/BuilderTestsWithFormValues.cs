using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers;
using System;
using Xunit;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public class BuilderTestsWithFormValues
    {
        [Fact(DisplayName = "1 form value (string), returns string")]
        public void FluentControllerBuilder_FluentActionUsingFormValueReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticAction(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingFormValue<string>("name")
                    .To(name => $"Hello {name}!"),
                typeof(ControllerWithFormValueReturnsString),
                new object[] { "Charlie" });
        }

        [Fact(DisplayName = "1 form value (string) with used default value, returns string")]
        public void FluentControllerBuilder_FluentActionUsingFormValueWithUsedDefaultValueReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticAction(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingFormValue<string>("name", "Hanzel")
                    .To(name => $"Hello {name}!"),
                typeof(ControllerWithFormValueAndDefaultValueReturnsString),
                new object[] { Type.Missing });
        }

        [Fact(DisplayName = "1 form value (string) with unused default value, returns string")]
        public void FluentControllerBuilder_FluentActionUsingFormValueWithUnusedDefaultValueReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticAction(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingFormValue<string>("name", "Hanzel")
                    .To(name => $"Hello {name}!"),
                typeof(ControllerWithFormValueAndDefaultValueReturnsString),
                new object[] { "Charlie" });
        }

        [Fact(DisplayName = "2 form values (string, identical), returns string")]
        public void FluentControllerBuilder_FluentActionUsingTwoIdenticalFormValuesReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticAction(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingFormValue<string>("name")
                    .UsingFormValue<string>("name")
                    .To((name1, name2) => $"Hello {name1}! I said hello {name2}!"),
                typeof(ControllerWithTwoIdenticalFormValuesReturnsString),
                new object[] { "Charlie" });
        }
    }
}
