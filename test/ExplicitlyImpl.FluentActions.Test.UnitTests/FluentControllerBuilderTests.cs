using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using ExplicitlyImpl.FluentActions.Test.UnitTests.Controllers;
using ExplicitlyImpl.FluentActions.Test.Utils;
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

            CompareDynamicControllerToStaticController(
                builtType,
                typeof(ControllerForHandlerWithoutUsingsAndReturnsString));
        }

        public static void CompareDynamicControllerToStaticController(Type dynamicControllerType, Type staticControllerType)
        {
            var comparer = new TypeComparer(new TypeComparisonFeature[]
            {
                TypeComparisonFeature.Name
            }, new TypeComparerOptions());

            var comparisonResult = comparer.Compare(staticControllerType, dynamicControllerType);

            if (!comparisonResult.CompleteMatch)
            { 
                throw new Exception(string.Format(
                    "Static controller {0} does not match dynamically created controller {1}: {2}",
                    staticControllerType.Name,
                    dynamicControllerType.Name,
                    string.Join(" ", comparisonResult.MismatchingFeaturesResults
                        .Select(comparedFeaturesResult => comparedFeaturesResult.Message))));
            }
        }

        public static FluentActionControllerDefinition BuildController(FluentActionBase fluentAction)
        {
            var controllerBuilder = new FluentActionControllerDefinitionBuilder();
            return controllerBuilder.Build(fluentAction);
        }
    }
}
