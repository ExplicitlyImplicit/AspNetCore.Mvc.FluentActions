// Licensed under the MIT License. See LICENSE file in the root of the solution for license information.

using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using ExplicitlyImpl.FluentActions.Test.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public static class BuilderTestUtils
    {
        public static void AssertConstantValuesOfBuiltController(FluentActionControllerDefinition builtController, FluentActionBase fluentAction)
        {
            var builtControllerTypeInfo = builtController.TypeInfo;

            Assert.Equal(fluentAction.RouteTemplate, builtController.RouteTemplate);
            Assert.Equal("HandlerAction", builtController.ActionName);
            Assert.Equal(builtControllerTypeInfo.Name, builtController.Id);
            Assert.Equal(builtControllerTypeInfo.Name, builtController.Name);
            Assert.Equal(fluentAction, builtController.FluentAction);

            Assert.True(builtControllerTypeInfo.Name.StartsWith("FluentAction"));
            Assert.True(builtControllerTypeInfo.Name.EndsWith("Controller"));
        }

        public static void BuildActionAndCompareToStaticAction(
            FluentActionBase fluentAction, 
            Type staticControllerType,
            ILogger logger = null)
        {
            var builtController = BuildAction(fluentAction, logger);

            AssertConstantValuesOfBuiltController(builtController, fluentAction);

            CompareBuiltControllerToStaticController(builtController.TypeInfo.UnderlyingSystemType, staticControllerType);
        }

        public static void BuildActionAndCompareToStaticActionWithResult(
            FluentActionBase fluentAction, 
            Type staticControllerType, 
            object[] actionMethodArguments, 
            ILogger logger = null)
        {
            var builtController = BuildAction(fluentAction, logger);

            AssertConstantValuesOfBuiltController(builtController, fluentAction);

            CompareBuiltControllerToStaticController(builtController.TypeInfo.UnderlyingSystemType, staticControllerType);

            CompareActionMethodResults(fluentAction, builtController, staticControllerType, actionMethodArguments);
        }

        public static void CompareActionMethodResults(FluentActionBase fluentAction, FluentActionControllerDefinition builtController, Type staticControllerType, object[] actionMethodArguments = null)
        {
            var resultsFromBuiltController = InvokeActionMethod(builtController.TypeInfo, actionMethodArguments);
            var resultsFromStaticController = InvokeActionMethod(staticControllerType.GetTypeInfo(), actionMethodArguments);

            if (resultsFromBuiltController == null)
            {
                throw new Exception($"Invoked action method returns null of built controller {builtController.Name}.");
            }

            if (resultsFromStaticController == null)
            {
                throw new Exception($"Invoked action method returns null of statically defined controller {staticControllerType.Name}.");
            }

            var returnTypeIsAsync = fluentAction.Definition.IsAsync;
            var returnType = returnTypeIsAsync
                ? typeof(Task<>).MakeGenericType(fluentAction.Definition.ReturnType)
                : fluentAction.Definition.ReturnType;

            if (!returnType.IsAssignableFrom(resultsFromBuiltController.GetType()))
            {
                throw new Exception($"Incorrect return type from invoked action method of built controller {builtController.Name} ({resultsFromBuiltController.GetType().Name} should be {fluentAction.Definition.ReturnType}).");
            }

            if (!returnType.IsAssignableFrom(resultsFromStaticController.GetType()))
            {
                throw new Exception($"Incorrect return type from invoked action method of statically defined controller {staticControllerType.Name} ({resultsFromStaticController.GetType().Name} should be {fluentAction.Definition.ReturnType}).");
            }

            if (returnTypeIsAsync)
            {
                resultsFromBuiltController = GetTaskResult(resultsFromBuiltController);
                resultsFromStaticController = GetTaskResult(resultsFromStaticController);
            }

            if (!IsEqual(resultsFromBuiltController, resultsFromStaticController))
            {
                throw new Exception($"Results from invoked action methods does not match between built controller {builtController.Name} and statically defined controller {staticControllerType.Name} ({resultsFromBuiltController} vs {resultsFromStaticController}).");
            }
        }

        private static object GetTaskResult(object taskWithResult)
        {
            try
            {
                var genericArgumentType = taskWithResult.GetType().GetGenericArguments()[0];
                var resultProperty = typeof(Task<>).MakeGenericType(genericArgumentType).GetProperty("Result");
                return resultProperty.GetValue(taskWithResult);
            } 
            catch (Exception exception)
            {
                throw new Exception("Could not get results of generic task.", exception);
            }
        }

        private static bool IsEqual(object value1, object value2)
        {
            var typeInfo = value1.GetType().GetTypeInfo();

            if (typeInfo.IsPrimitive)
            {
                return value1.Equals(value2);
            } 
            else if (value1 is string)
            {
                return value1 != null && value1.Equals(value2);
            }
            else if (value1 is IEnumerable)
            {
                return Enumerable.SequenceEqual((IEnumerable<object>)value1, (IEnumerable<object>)value2);
            } 
            else if (value1 is ViewResult && value2 is ViewResult)
            {
                var viewResult1 = ((ViewResult)value1);
                var viewResult2 = ((ViewResult)value2);

                return viewResult1.ContentType == viewResult2.ContentType &&
                    viewResult1.StatusCode == viewResult2.StatusCode &&
                    viewResult1.ViewName == viewResult2.ViewName;
            } 
            else if (value1 is PartialViewResult && value2 is PartialViewResult)
            {
                var viewResult1 = ((PartialViewResult)value1);
                var viewResult2 = ((PartialViewResult)value2);

                return viewResult1.ContentType == viewResult2.ContentType &&
                    viewResult1.StatusCode == viewResult2.StatusCode &&
                    viewResult1.ViewName == viewResult2.ViewName;
            } 
            else if (value1 is ViewComponentResult && value2 is ViewComponentResult)
            {
                var viewResult1 = ((ViewComponentResult)value1);
                var viewResult2 = ((ViewComponentResult)value2);

                return viewResult1.ContentType == viewResult2.ContentType &&
                    viewResult1.StatusCode == viewResult2.StatusCode &&
                    viewResult1.ViewComponentName == viewResult2.ViewComponentName &&
                    viewResult1.ViewComponentType == viewResult2.ViewComponentType;
            } 
            else
            {
                return value1 != null && value1.Equals(value2);
            }
        }

        private static object InvokeActionMethod(TypeInfo controllerTypeInfo, object[] arguments)
        {
            try
            {
                var instance = Activator.CreateInstance(controllerTypeInfo.UnderlyingSystemType);

                var mockedControllerContext = MockControllerContext();
                var setControllerContextMethod = typeof(Controller).GetProperty("ControllerContext").GetSetMethod();
                setControllerContextMethod.Invoke(instance, new object[] { mockedControllerContext });

                var mockedTempDataProvider = Mock.Of<ITempDataProvider>();
                var tempData = new TempDataDictionary(mockedControllerContext.HttpContext, mockedTempDataProvider);
                var setTempDataMethod = typeof(Controller).GetProperty("TempData").GetSetMethod();
                setTempDataMethod.Invoke(instance, new object[] { tempData });

                var method = controllerTypeInfo.GetMethod("HandlerAction");
                return method.Invoke(instance, arguments);
            } 
            catch (Exception exception)
            {
                throw new Exception($"Could not invoke action method for controller type {controllerTypeInfo.Name}.", exception);
            }
        }

        private static ControllerContext MockControllerContext()
        {
            var headerDictionary = new HeaderDictionary();
            var response = new Mock<HttpResponse>();
            response.SetupGet(r => r.Headers).Returns(headerDictionary);

            var httpContext = new Mock<HttpContext>();
            httpContext.SetupGet(a => a.Response).Returns(response.Object);

            var actionContext = new ActionContext(httpContext.Object, new RouteData(), new ControllerActionDescriptor());

            return new ControllerContext(actionContext);
        }

        public static void CompareBuiltControllerToStaticController(Type builtControllerType, Type staticControllerType)
        {
            var comparer = new TypeComparer(new TypeComparisonFeature[]
            {
                TypeComparisonFeature.ParentType,
                TypeComparisonFeature.HandlerActionMethod,
            }, new TypeComparerOptions());

            var comparisonResult = comparer.Compare(builtControllerType, staticControllerType);

            if (!comparisonResult.CompleteMatch)
            {
                throw new Exception(string.Format(
                    "Dynamically created controller {0} does not match statically defined controller {1}: {2}",
                    staticControllerType.Name,
                    builtControllerType.Name,
                    string.Join(" ", comparisonResult.MismatchingFeaturesResults
                        .Select(comparedFeaturesResult => comparedFeaturesResult.Message))));
            }
        }

        public static FluentActionControllerDefinition BuildAction(FluentActionBase fluentAction, ILogger logger = null)
        {
            var controllerBuilder = new FluentActionControllerDefinitionBuilder();
            return controllerBuilder.Build(fluentAction, logger);
        }
    }

    public class TestLogger : ILogger
    {
        private readonly ITestOutputHelper TestOutputHelper;

        public TestLogger(ITestOutputHelper testOutputHelper)
        {
            TestOutputHelper = testOutputHelper;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            TestOutputHelper.WriteLine(formatter(state, exception));
        }
    }
}
