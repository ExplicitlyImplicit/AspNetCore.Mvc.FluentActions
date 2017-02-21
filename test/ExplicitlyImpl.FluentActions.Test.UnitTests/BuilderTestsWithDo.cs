using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers;
using System;
using System.Threading.Tasks;
using Xunit;
using static ExplicitlyImpl.AspNetCore.Mvc.FluentActions.FluentActionControllerDefinitionBuilder;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public class BuilderTestsWithDo
    {
        [Fact(DisplayName = "1 Do, no return, throws")]
        public void FluentControllerBuilder_FluentActionWithDoAndNoReturnThrows()
        {
            Assert.Throws<FluentActionValidationException>(() =>
                BuilderTestUtils.BuildAction(
                    new FluentAction("/route/url", HttpMethod.Get)
                        .Do(() => Console.WriteLine("foo"))));
        }

        [Fact(DisplayName = "1 Do, returns string")]
        public void FluentControllerBuilder_FluentActionWithDoReturnsString()
        {
            var foo = "bar";

            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .Do(() => { foo = "baz"; })
                    .To(() => foo),
                typeof(ControllerWithDoReturnsString),
                null);
        }

        [Fact(DisplayName = "1 Do, returns string async")]
        public void FluentControllerBuilder_FluentActionWithDoReturnsStringAsync()
        {
            var foo = "bar";

            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .DoAsync(async () => { await Task.Delay(1); foo = "baz"; })
                    .To(() => foo),
                typeof(ControllerWithDoReturnsStringAsync),
                null);
        }
    }
}
