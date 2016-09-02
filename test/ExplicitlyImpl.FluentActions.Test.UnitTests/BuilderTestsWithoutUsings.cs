using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers;
using System;
using Xunit;
using static ExplicitlyImpl.AspNetCore.Mvc.FluentActions.FluentActionControllerDefinitionBuilder;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public class BuilderTestsWithoutUsings
    {
        [Fact(DisplayName = "No handler, throws")]
        public void FluentControllerBuilder_ThrowsOnFluentActionWithoutHandler()
        {
            var fluentAction = new FluentAction("/route/url", HttpMethod.Get);

            Assert.Throws<FluentActionValidationException>(() => BuilderTestUtils.BuildController(fluentAction));
        }

        [Fact(DisplayName = "No usings, returns string")]
        public void FluentControllerBuilder_FluentActionWithoutUsingsAndReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticAction(
                new FluentAction("/route/url", HttpMethod.Get)
                    .To(() => "Hello"), 
                typeof(ParameterlessControllerReturnsString),
                null);
        }

        [Fact(DisplayName = "No usings, returns int")]
        public void FluentControllerBuilder_FluentActionWithoutUsingsAndReturnsInt()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticAction(
                new FluentAction("/route/url", HttpMethod.Get)
                    .To(() => 13),
                typeof(ParameterlessControllerReturnsInt),
                null);
        }

        [Fact(DisplayName = "No usings, returns guid")]
        public void FluentControllerBuilder_FluentActionWithoutUsingsAndReturnsGuid()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticAction(
                new FluentAction("/route/url", HttpMethod.Get)
                    .To(() => new Guid("2a6d4959-817c-4514-90f3-52b518e9ddb0")),
                typeof(ParameterlessControllerReturnsGuid),
                null);
        }

        [Fact(DisplayName = "No usings, returns enum")]
        public void FluentControllerBuilder_FluentActionWithoutUsingsAndReturnsEnum()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticAction(
                new FluentAction("/route/url", HttpMethod.Get)
                    .To(() => ExampleEnumWithoutUsings.ExampleEnumValue2),
                typeof(ParameterlessControllerReturnsEnum),
                null);
        }

        [Fact(DisplayName = "No usings, returns object")]
        public void FluentControllerBuilder_FluentActionWithoutUsingsAndReturnsObject()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticAction(
                new FluentAction("/route/url", HttpMethod.Get)
                    .To(() => new ExampleClassWithoutUsings
                    {
                        StringField = "Hello",
                        IntField = 14,
                        StringProperty = "World!"
                    }),
                typeof(ParameterlessControllerReturnsObject),
                null);
        }
    }
}
