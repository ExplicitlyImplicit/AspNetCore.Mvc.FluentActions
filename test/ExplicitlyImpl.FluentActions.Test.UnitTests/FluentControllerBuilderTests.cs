using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using System;
using System.Linq;
using Xunit;
using static ExplicitlyImpl.AspNetCore.Mvc.FluentActions.FluentActionControllerDefinitionBuilder;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public class FluentControllerBuilderTests
    {
        [Fact]
        public void FluentControllerBuilder_ThrowsOnFluentActionWithoutHandler()
        {
            var fluentAction = new FluentAction("/", HttpMethod.Get);

            Assert.Throws<FluentActionValidationException>(() => BuildController(fluentAction));
        }

        [Fact]
        public void FluentControllerBuilder_BuiltControllerForHandlerWithoutUsingsAndReturnsString()
        {
            var fluentAction = new FluentAction("/", HttpMethod.Get)
                .To(() => "Hello");

            var builtController = BuildController(fluentAction);
            var builtTypeInfo = builtController.TypeInfo;

            Assert.Equal("", builtController.Url);
            Assert.True(builtTypeInfo.Name.StartsWith("FluentAction"));
            Assert.True(builtTypeInfo.Name.EndsWith("Controller"));
            Assert.Equal("HandlerAction", builtController.ActionName);
            Assert.Equal(builtTypeInfo.Name, builtController.Id);
            Assert.Equal(builtTypeInfo.Name, builtController.Name);
            Assert.Equal(fluentAction, builtController.FluentAction);

            Assert.Equal(1, builtTypeInfo.GetDeclaredMethods("HandlerAction").Count());

            var actionMethod = builtTypeInfo.GetDeclaredMethods("HandlerAction").First();

            Assert.Equal(0, actionMethod.GetParameters().Count());
            Assert.Equal(typeof(string), actionMethod.ReturnType);

            var builtType = builtTypeInfo.UnderlyingSystemType;

            var controllerInstance = Activator.CreateInstance(builtType);

            var result = (string) actionMethod.Invoke(controllerInstance, null);

            Assert.Equal("Hello", result);

            // TODO compare builtTypeInfo
        }

        public static FluentActionControllerDefinition BuildController(FluentActionBase fluentAction)
        {
            var controllerBuilder = new FluentActionControllerDefinitionBuilder();
            return controllerBuilder.Build(fluentAction);
        }
    }
}
