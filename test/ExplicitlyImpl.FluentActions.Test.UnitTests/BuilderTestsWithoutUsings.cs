using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers;
using System;
using System.Threading.Tasks;
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

            Assert.Throws<FluentActionValidationException>(() => BuilderTestUtils.BuildAction(fluentAction));
        }

        [Fact(DisplayName = "No usings, returns string")]
        public void FluentControllerBuilder_FluentActionWithoutUsingsAndReturnsString()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .To(() => "Hello"), 
                typeof(ParameterlessControllerReturnsString),
                null);
        }

        [Fact(DisplayName = "No usings, returns string async")]
        public void FluentControllerBuilder_FluentActionWithoutUsingsAndReturnsStringAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .To(async () => { await Task.Delay(1); return "Hello"; }),
                typeof(ParameterlessControllerReturnsStringAsync),
                null);
        }

        [Fact(DisplayName = "No usings, returns int")]
        public void FluentControllerBuilder_FluentActionWithoutUsingsAndReturnsInt()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .To(() => 13),
                typeof(ParameterlessControllerReturnsInt),
                null);
        }

        [Fact(DisplayName = "No usings, returns int async")]
        public void FluentControllerBuilder_FluentActionWithoutUsingsAndReturnsIntAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .To(async () => { await Task.Delay(1); return 13; }),
                typeof(ParameterlessControllerReturnsIntAsync),
                null);
        }

        [Fact(DisplayName = "No usings, returns guid")]
        public void FluentControllerBuilder_FluentActionWithoutUsingsAndReturnsGuid()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .To(() => new Guid("2a6d4959-817c-4514-90f3-52b518e9ddb0")),
                typeof(ParameterlessControllerReturnsGuid),
                null);
        }

        [Fact(DisplayName = "No usings, returns guid async")]
        public void FluentControllerBuilder_FluentActionWithoutUsingsAndReturnsGuidAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .To(async () => { await Task.Delay(1); return new Guid("2a6d4959-817c-4514-90f3-52b518e9ddb0"); }),
                typeof(ParameterlessControllerReturnsGuidAsync),
                null);
        }

        [Fact(DisplayName = "No usings, returns enum")]
        public void FluentControllerBuilder_FluentActionWithoutUsingsAndReturnsEnum()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .To(() => ExampleEnumWithoutUsings.ExampleEnumValue2),
                typeof(ParameterlessControllerReturnsEnum),
                null);
        }

        [Fact(DisplayName = "No usings, returns enum async")]
        public void FluentControllerBuilder_FluentActionWithoutUsingsAndReturnsEnumAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .To(async () => { await Task.Delay(1); return ExampleEnumWithoutUsings.ExampleEnumValue2; }),
                typeof(ParameterlessControllerReturnsEnumAsync),
                null);
        }

        [Fact(DisplayName = "No usings, returns object")]
        public void FluentControllerBuilder_FluentActionWithoutUsingsAndReturnsObject()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
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

        [Fact(DisplayName = "No usings, returns object async")]
        public void FluentControllerBuilder_FluentActionWithoutUsingsAndReturnsObjectAsync()
        {
            BuilderTestUtils.BuildActionAndCompareToStaticActionWithResult(
                new FluentAction("/route/url", HttpMethod.Get)
                    .To(async () => 
                    {
                        await Task.Delay(1);
                        return new ExampleClassWithoutUsings
                        {
                            StringField = "Hello",
                            IntField = 14,
                            StringProperty = "World!"
                        };
                    }),
                typeof(ParameterlessControllerReturnsObjectAsync),
                null);
        }
    }
}
