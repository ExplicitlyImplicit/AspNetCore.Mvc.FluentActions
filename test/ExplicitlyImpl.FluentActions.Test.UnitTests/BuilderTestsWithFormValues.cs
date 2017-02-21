using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public class BuilderTestsWithFormValues
    {
        [Fact(DisplayName = "1 form value (string), returns string")]
        public void FluentControllerBuilder_FluentActionUsingFormValueReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingFormValue<string>("name")
                    .To(name => $"Hello {name}!"),
                typeof(ControllerWithFormValueReturnsString),
                new object[] { "Charlie" });
        }

        [Fact(DisplayName = "1 form value (string), returns string async")]
        public void FluentControllerBuilder_FluentActionUsingFormValueReturnsStringAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingFormValue<string>("name")
                    .To(async name => { await Task.Delay(1); return $"Hello {name}!"; }),
                typeof(ControllerWithFormValueReturnsStringAsync),
                new object[] { "Charlie" });
        }

        [Fact(DisplayName = "1 form value (string) with used default value, returns string")]
        public void FluentControllerBuilder_FluentActionUsingFormValueWithUsedDefaultValueReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingFormValue<string>("name", "Hanzel")
                    .To(name => $"Hello {name}!"),
                typeof(ControllerWithFormValueAndDefaultValueReturnsString),
                new object[] { Type.Missing });
        }

        [Fact(DisplayName = "1 form value (string) with used default value, returns string async")]
        public void FluentControllerBuilder_FluentActionUsingFormValueWithUsedDefaultValueReturnsStringAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingFormValue<string>("name", "Hanzel")
                    .To(async name => { await Task.Delay(1); return $"Hello {name}!"; }),
                typeof(ControllerWithFormValueAndDefaultValueReturnsStringAsync),
                new object[] { Type.Missing });
        }

        [Fact(DisplayName = "1 form value (string) with unused default value, returns string")]
        public void FluentControllerBuilder_FluentActionUsingFormValueWithUnusedDefaultValueReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingFormValue<string>("name", "Hanzel")
                    .To(name => $"Hello {name}!"),
                typeof(ControllerWithFormValueAndDefaultValueReturnsString),
                new object[] { "Charlie" });
        }

        [Fact(DisplayName = "1 form value (string) with unused default value, returns string async")]
        public void FluentControllerBuilder_FluentActionUsingFormValueWithUnusedDefaultValueReturnsStringAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingFormValue<string>("name", "Hanzel")
                    .To(async name => { await Task.Delay(1); return $"Hello {name}!"; }),
                typeof(ControllerWithFormValueAndDefaultValueReturnsStringAsync),
                new object[] { "Charlie" });
        }

        [Fact(DisplayName = "2 form values (string, identical), returns string")]
        public void FluentControllerBuilder_FluentActionUsingTwoIdenticalFormValuesReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingFormValue<string>("name")
                    .UsingFormValue<string>("name")
                    .To((name1, name2) => $"Hello {name1}! I said hello {name2}!"),
                typeof(ControllerWithTwoIdenticalFormValuesReturnsString),
                new object[] { "Charlie" });
        }

        [Fact(DisplayName = "2 form values (string, identical), returns string async")]
        public void FluentControllerBuilder_FluentActionUsingTwoIdenticalFormValuesReturnsStringAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .UsingFormValue<string>("name")
                    .UsingFormValue<string>("name")
                    .To(async (name1, name2) => { await Task.Delay(1); return $"Hello {name1}! I said hello {name2}!"; }),
                typeof(ControllerWithTwoIdenticalFormValuesReturnsStringAsync),
                new object[] { "Charlie" });
        }
    }
}
