// Licensed under the MIT License. See LICENSE file in the root of the solution for license information.

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentActions
{
    public class FluentActionControllerDefinitionBuilder
    {
        private const string ActionName = "HandlerAction";

        public FluentActionControllerDefinition Build(FluentActionBase fluentAction)
        {
            if (fluentAction == null)
            {
                throw new ArgumentNullException(nameof(fluentAction));
            }

            var validationResult = ValidateFluentActionForBuilding(fluentAction);

            if (!validationResult.Valid)
            {
                throw new FluentActionValidationException($"Could not validate fluent action {fluentAction}: {validationResult}");
            }

            if (fluentAction.Definition.IsMapRoute)
            {
                var handler = fluentAction.Definition.Handlers.First();

                if (handler.Expression == null)
                {
                    throw new ArgumentException(
                        $"Missing action expression for {fluentAction}.");
                }

                if (!(handler.Expression.Body is MethodCallExpression))
                {
                    throw new ArgumentException(
                        $"Expression for {fluentAction} must be a single method call expression.");
                }

                var method = ((MethodCallExpression)handler.Expression.Body).Method;
                var controllerTypeInfo = method.DeclaringType.GetTypeInfo();

                if (!(typeof(Controller).IsAssignableFrom(controllerTypeInfo.UnderlyingSystemType)))
                {
                    throw new ArgumentException(
                        $"Method call for {fluentAction} must come from a controller.");
                }

                var guid = Guid.NewGuid().ToString().Without("-");

                return new FluentActionControllerDefinition()
                {
                    Id = controllerTypeInfo.Name + "_" + method.Name + "_" + guid,
                    Name = controllerTypeInfo.Name,
                    ActionName = method.Name,
                    FluentAction = fluentAction,
                    TypeInfo = controllerTypeInfo
                };
            } else
            {
                try
                {
                    var controllerTypeInfo = DefineControllerType(fluentAction.Definition);

                    return new FluentActionControllerDefinition()
                    {
                        Id = controllerTypeInfo.Name,
                        Name = controllerTypeInfo.Name,
                        ActionName = ActionName,
                        FluentAction = fluentAction,
                        TypeInfo = controllerTypeInfo
                    };
                } catch (Exception buildException)
                {
                    throw new Exception($"Could not build controller type for {fluentAction}: {buildException.Message}", buildException);
                }
            }
        }

        private FluentActionValidationResult ValidateFluentActionForBuilding(FluentActionBase fluentAction)
        {
            if (fluentAction == null)
            {
                throw new ArgumentNullException(nameof(fluentAction));
            }

            var validationResult = new FluentActionValidationResult
            {
                Valid = true,
                ValidationErrorMessages = new List<string>()
            };

            if (fluentAction.Definition == null)
            {
                validationResult.AddValidationError($"{nameof(fluentAction.Definition)} is null.");
                return validationResult;
            }

            if (fluentAction.Definition.Handlers == null)
            {
                validationResult.AddValidationError($"{nameof(fluentAction.Definition.Handlers)} is null.");
                return validationResult;
            }

            var handlers = fluentAction.Definition.Handlers;

            if (!handlers.Any())
            {
                validationResult.AddValidationError("At least one handler is required.");
            }

            foreach (var handlerWithNoReturnType in handlers
                .Where(handler => handler.Type != FluentActionHandlerType.Action && handler.ReturnType == null))
            {
                validationResult.AddValidationError("Missing return type for handler.");
            }

            return validationResult;
        }

        public class FluentActionValidationResult
        {
            public FluentActionValidationResult()
            {
                ValidationErrorMessages = new List<string>();
            }

            public bool Valid { get; set; }

            public IList<string> ValidationErrorMessages { get; set; }

            public void AddValidationError(string errorMessage)
            {
                Valid = false;
                ValidationErrorMessages.Add(errorMessage);
            }

            public override string ToString()
            {
                return Valid ? "This fluent action is valid" : string.Join(Environment.NewLine, ValidationErrorMessages);
            }
        }

        public class FluentActionValidationException : Exception
        {
            public FluentActionValidationException() : base() { }
            public FluentActionValidationException(string message) : base(message) { }
        }

        private static TypeInfo DefineControllerType(
            FluentActionDefinition fluentActionDefinition)
        {
            if (fluentActionDefinition == null)
            {
                throw new ArgumentNullException(nameof(fluentActionDefinition));
            }

            var moduleBuilder = DefineModule();
            var typeBuilder = DefineType(moduleBuilder);

            DefineActionMethod(typeBuilder, fluentActionDefinition);

            return typeBuilder.CreateTypeInfo();
        }

        private static ModuleBuilder DefineModule()
        {
            var assemblyName = new AssemblyName("FluentActionAssembly");
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(
                assemblyName, 
                AssemblyBuilderAccess.Run);

            return assemblyBuilder.DefineDynamicModule("FluentActionModule");
        }

        private static TypeBuilder DefineType(ModuleBuilder moduleBuilder)
        {
            var guid = Guid.NewGuid().ToString().Replace("-", "");
            var name = $"FluentAction{guid}Controller";

            var typeBuilder = moduleBuilder.DefineType(
                    name,
                    TypeAttributes.Class | TypeAttributes.Public,
                    typeof(Controller));

            typeBuilder.DefineDefaultConstructor(MethodAttributes.Public);

            return typeBuilder;
        }

        private static Type GetFuncType(FluentActionHandlerDefinition handler)
        {
            var argumentTypes = handler.Usings.Select(@using => @using.Type).ToList();
            argumentTypes.Add(handler.ReturnType);

            var unspecifiedGenericFuncType = GetUnspecifiedGenericFuncType(argumentTypes.Count);
            var specifiedGenericFuncType = unspecifiedGenericFuncType.MakeGenericType(argumentTypes.ToArray());

            return specifiedGenericFuncType;
        }

        private static Type GetUnspecifiedGenericFuncType(int arguments)
        {
            switch (arguments)
            {
                case 1: return typeof(Func<>);
                case 2: return typeof(Func<,>);
                case 3: return typeof(Func<,,>);
                case 4: return typeof(Func<,,,>);
                case 5: return typeof(Func<,,,,>);
                case 6: return typeof(Func<,,,,,>);
                case 7: return typeof(Func<,,,,,,>);
                case 8: return typeof(Func<,,,,,,,>);
            }

            throw new Exception($"Fluent actions supports only up to eight arguments.");
        }

        private static Type GetActionType(FluentActionHandlerDefinition handler)
        {
            var argumentTypes = handler.Usings.Select(@using => @using.Type).ToList();

            if (argumentTypes.Count == 0)
            {
                return typeof(Action);
            }
            else
            {
                var unspecifiedGenericActionType = GetUnspecifiedGenericActionType(argumentTypes.Count);
                var specifiedGenericActionType = unspecifiedGenericActionType.MakeGenericType(argumentTypes.ToArray());

                return specifiedGenericActionType;
            }
        }

        private static Type GetUnspecifiedGenericActionType(int arguments)
        {
            switch (arguments)
            {
                case 1: return typeof(Action<>);
                case 2: return typeof(Action<,>);
                case 3: return typeof(Action<,,>);
                case 4: return typeof(Action<,,,>);
                case 5: return typeof(Action<,,,,>);
                case 6: return typeof(Action<,,,,,>);
                case 7: return typeof(Action<,,,,,,>);
                case 8: return typeof(Action<,,,,,,,>);
            }

            throw new Exception($"Fluent actions supports only up to eight arguments.");
        }

        private static Type GetHttpMethodAttribute(HttpMethod httpMethod)
        {
            switch (httpMethod)
            {
                case HttpMethod.Delete:     return typeof(HttpDeleteAttribute);
                case HttpMethod.Get:        return typeof(HttpGetAttribute);
                case HttpMethod.Head:       return typeof(HttpHeadAttribute);
                case HttpMethod.Options:    return typeof(HttpOptionsAttribute);
                case HttpMethod.Patch:      return typeof(HttpPatchAttribute);
                case HttpMethod.Post:       return typeof(HttpPostAttribute);
                case HttpMethod.Put:        return typeof(HttpPutAttribute);
            }

            throw new Exception($"Could not get corresponding attribute of {nameof(HttpMethod)} {httpMethod}.");
        }

        private static void DefineActionMethod(
            TypeBuilder typeBuilder, 
            FluentActionDefinition fluentActionDefinition)
        {
            var usingsForMethodParameters = fluentActionDefinition.Handlers
                .SelectMany(handler => handler.Usings)
                .Where(@using => @using.IsMethodParameter)
                .Distinct()
                .ToArray();

            var methodParameterIndicesForUsings = usingsForMethodParameters
                .Select((@using, index) => new { Using = @using, Index = index })
                .ToDictionary(
                    indexedUsing => indexedUsing.Using.GetHashCode(),
                    indexedUsing => indexedUsing.Index + 1 // 1-based index
                );

            var methodParameterTypes = usingsForMethodParameters
                .Select(@using => @using.Type)
                .ToArray();

            var returnType = fluentActionDefinition.Handlers.Last().ReturnType;
            var methodBuilder = typeBuilder.DefineMethod(ActionName, MethodAttributes.Public, returnType, methodParameterTypes);

            SetHttpMethodAttribute(methodBuilder, fluentActionDefinition.HttpMethod);
            SetRouteAttribute(methodBuilder, fluentActionDefinition.RouteTemplate);

            var ilGenerator = methodBuilder.GetILGenerator();

            foreach (var usingDefinition in usingsForMethodParameters)
            {
                var methodParameterIndex = methodParameterIndicesForUsings[usingDefinition.GetHashCode()];

                var methodParameterBuilder = methodBuilder.DefineParameter(
                    methodParameterIndex,
                    usingDefinition.HasDefaultValue ? ParameterAttributes.HasDefault : ParameterAttributes.None, 
                    $"parameter{methodParameterIndex}");

                if (usingDefinition.HasDefaultValue)
                {
                    methodParameterBuilder.SetConstant(usingDefinition.DefaultValue);
                }

                if (usingDefinition is FluentActionUsingServiceDefinition)
                {
                    var parameterAttributeBuilder = new CustomAttributeBuilder(typeof(FromServicesAttribute)
                        .GetConstructor(new Type[0]), new Type[0]);
                    methodParameterBuilder.SetCustomAttribute(parameterAttributeBuilder);
                } 
                else if (usingDefinition is FluentActionUsingRouteParameterDefinition)
                {
                    var attributeType = typeof(FromRouteAttribute);
                    var name = ((FluentActionUsingRouteParameterDefinition)usingDefinition).Name;

                    if (!fluentActionDefinition.RouteTemplate.Contains($"{{{name}}}", StringComparison.CurrentCultureIgnoreCase))
                    {
                        throw new Exception($"Route parameter {name} does not exist in routeTemplate {fluentActionDefinition.RouteTemplate}.");
                    }

                    var parameterAttributeBuilder = new CustomAttributeBuilder(
                        attributeType.GetConstructor(new Type[0]), 
                        new Type[0],
                        new[] { attributeType.GetProperty("Name") },
                        new object[] { name });

                    methodParameterBuilder.SetCustomAttribute(parameterAttributeBuilder);
                }
                else if (usingDefinition is FluentActionUsingQueryStringParameterDefinition)
                {
                    var attributeType = typeof(FromQueryAttribute);
                    var name = ((FluentActionUsingQueryStringParameterDefinition)usingDefinition).Name;

                    var parameterAttributeBuilder = new CustomAttributeBuilder(
                        attributeType.GetConstructor(new Type[0]),
                        new Type[0],
                        new[] { attributeType.GetProperty("Name") },
                        new object[] { name });

                    methodParameterBuilder.SetCustomAttribute(parameterAttributeBuilder);
                }
                else if (usingDefinition is FluentActionUsingHeaderParameterDefinition)
                {
                    var attributeType = typeof(FromHeaderAttribute);
                    var name = ((FluentActionUsingHeaderParameterDefinition)usingDefinition).Name;

                    var parameterAttributeBuilder = new CustomAttributeBuilder(
                        attributeType.GetConstructor(new Type[0]),
                        new Type[0],
                        new[] { attributeType.GetProperty("Name") },
                        new object[] { name });

                    methodParameterBuilder.SetCustomAttribute(parameterAttributeBuilder);
                }
                else if (usingDefinition is FluentActionUsingBodyDefinition)
                {
                    var parameterAttributeBuilder = new CustomAttributeBuilder(typeof(FromBodyAttribute)
                        .GetConstructor(new Type[0]), new Type[0]);
                    methodParameterBuilder.SetCustomAttribute(parameterAttributeBuilder);
                } 
                else if (usingDefinition is FluentActionUsingFormDefinition)
                {
                    var parameterAttributeBuilder = new CustomAttributeBuilder(typeof(FromFormAttribute)
                        .GetConstructor(new Type[0]), new Type[0]);
                    methodParameterBuilder.SetCustomAttribute(parameterAttributeBuilder);
                }
                else if (usingDefinition is FluentActionUsingFormValueDefinition)
                {
                    var attributeType = typeof(FromFormAttribute);
                    var key = ((FluentActionUsingFormValueDefinition)usingDefinition).Key;

                    var parameterAttributeBuilder = new CustomAttributeBuilder(
                        attributeType.GetConstructor(new Type[0]),
                        new Type[0],
                        new[] { attributeType.GetProperty("Name") },
                        new object[] { key });

                    methodParameterBuilder.SetCustomAttribute(parameterAttributeBuilder);
                }
                else if (usingDefinition is FluentActionUsingModelBinderDefinition)
                {
                    var attributeType = typeof(ModelBinderAttribute);
                    var modelBinderDefinition = ((FluentActionUsingModelBinderDefinition)usingDefinition);
                    var modelBinderType = modelBinderDefinition.ModelBinderType;

                    PropertyInfo[] propertyTypes = null;
                    object[] propertyValues = null;
                    if (!string.IsNullOrWhiteSpace(modelBinderDefinition.ParameterName))
                    {
                        propertyTypes = new[]
                        {
                            attributeType.GetProperty("BinderType"),
                            attributeType.GetProperty("Name")
                        };
                        propertyValues = new object[] 
                        {
                            modelBinderType,
                            modelBinderDefinition.ParameterName
                        };
                    } 
                    else
                    {
                        propertyTypes = new[]
                        {
                            attributeType.GetProperty("BinderType")
                        };
                        propertyValues = new object[]
                        {
                            modelBinderType
                        };
                    }

                    var parameterAttributeBuilder = new CustomAttributeBuilder(
                        attributeType.GetConstructor(new Type[0]),
                        new Type[0],
                        propertyTypes,
                        propertyValues);

                    methodParameterBuilder.SetCustomAttribute(parameterAttributeBuilder);
                }
            }

            var dictionaryField = typeof(FluentActionControllerDefinitionHandlerDelegates)
                .GetField("All");
            var dictionaryGetMethod = typeof(ConcurrentDictionary<,>)
                .MakeGenericType(typeof(string), typeof(Delegate))
                .GetMethod("get_Item");

            var httpContextControllerProperty = typeof(Controller).GetProperty("HttpContext");
            var viewBagControllerProperty = typeof(Controller).GetProperty("ViewBag");
            var viewDataControllerProperty = typeof(Controller).GetProperty("ViewData");
            var tempDataControllerProperty = typeof(Controller).GetProperty("TempData");

            LocalBuilder localVariableForPreviousReturnValue = null;

            foreach (var handler in fluentActionDefinition.Handlers)
            {
                if (handler.Type == FluentActionHandlerType.Func)
                {
                   var localVariableForReturnValue = ilGenerator.DeclareLocal(handler.ReturnType);

                    var funcType = GetFuncType(handler);
                    var delegateKey = FluentActionControllerDefinitionHandlerDelegates.Add(handler.Delegate);

                    // Push Func
                    ilGenerator.Emit(OpCodes.Ldsfld, dictionaryField);
                    ilGenerator.Emit(OpCodes.Ldstr, delegateKey);
                    ilGenerator.Emit(OpCodes.Callvirt, dictionaryGetMethod);

                    // Push arguments for Func
                    foreach (var handlerUsing in handler.Usings)
                    {
                        if (handlerUsing.IsMethodParameter)
                        {
                            ilGenerator.Emit(OpCodes.Ldarg, methodParameterIndicesForUsings[handlerUsing.GetHashCode()]);
                        } 
                        else if (handlerUsing is FluentActionUsingResultFromHandlerDefinition)
                        {
                            if (localVariableForPreviousReturnValue == null)
                            {
                                throw new Exception("Cannot use previous result from handler as no previous result exists.");
                            }
                            ilGenerator.Emit(OpCodes.Ldloc, localVariableForPreviousReturnValue);
                        } 
                        else if (handlerUsing is FluentActionUsingHttpContextDefinition)
                        {
                            ilGenerator.Emit(OpCodes.Ldarg_0);
                            ilGenerator.Emit(OpCodes.Callvirt, httpContextControllerProperty.GetGetMethod());
                        } 
                        else if (handlerUsing is FluentActionUsingViewBagDefinition)
                        {
                            ilGenerator.Emit(OpCodes.Ldarg_0);
                            ilGenerator.Emit(OpCodes.Call, viewBagControllerProperty.GetGetMethod());
                        }
                        else if (handlerUsing is FluentActionUsingViewDataDefinition)
                        {
                            ilGenerator.Emit(OpCodes.Ldarg_0);
                            ilGenerator.Emit(OpCodes.Call, viewDataControllerProperty.GetGetMethod());
                        }
                        else if (handlerUsing is FluentActionUsingTempDataDefinition)
                        {
                            ilGenerator.Emit(OpCodes.Ldarg_0);
                            ilGenerator.Emit(OpCodes.Call, tempDataControllerProperty.GetGetMethod());
                        }
                    }

                    // Push Func.Invoke
                    ilGenerator.Emit(OpCodes.Callvirt, funcType.GetMethod("Invoke"));

                    // Push storing result in local variable
                    ilGenerator.Emit(OpCodes.Stloc, localVariableForReturnValue);

                    // Make sure next handler has access to previous handler's return value
                    localVariableForPreviousReturnValue = localVariableForReturnValue;
                } 
                else if (handler.Type == FluentActionHandlerType.Action)
                {
                    var actionType = GetActionType(handler);
                    var delegateKey = FluentActionControllerDefinitionHandlerDelegates.Add(handler.Delegate);

                    // Push Func
                    ilGenerator.Emit(OpCodes.Ldsfld, dictionaryField);
                    ilGenerator.Emit(OpCodes.Ldstr, delegateKey);
                    ilGenerator.Emit(OpCodes.Callvirt, dictionaryGetMethod);

                    // Push arguments for Action
                    foreach (var handlerUsing in handler.Usings)
                    {
                        if (handlerUsing.IsMethodParameter)
                        {
                            ilGenerator.Emit(OpCodes.Ldarg, methodParameterIndicesForUsings[handlerUsing.GetHashCode()]);
                        } 
                        else if (handlerUsing is FluentActionUsingResultFromHandlerDefinition)
                        {
                            if (localVariableForPreviousReturnValue == null)
                            {
                                throw new Exception("Cannot use previous result from handler as no previous result exists.");
                            }
                            ilGenerator.Emit(OpCodes.Ldloc, localVariableForPreviousReturnValue);
                        }
                        else if (handlerUsing is FluentActionUsingHttpContextDefinition)
                        {
                            ilGenerator.Emit(OpCodes.Ldarg_0);
                            ilGenerator.Emit(OpCodes.Callvirt, httpContextControllerProperty.GetGetMethod());
                        }
                        else if (handlerUsing is FluentActionUsingViewBagDefinition)
                        {
                            ilGenerator.Emit(OpCodes.Ldarg_0);
                            ilGenerator.Emit(OpCodes.Call, viewBagControllerProperty.GetGetMethod());
                        }
                        else if (handlerUsing is FluentActionUsingViewDataDefinition)
                        {
                            ilGenerator.Emit(OpCodes.Ldarg_0);
                            ilGenerator.Emit(OpCodes.Callvirt, viewDataControllerProperty.GetGetMethod());
                        }
                        else if (handlerUsing is FluentActionUsingTempDataDefinition)
                        {
                            ilGenerator.Emit(OpCodes.Ldarg_0);
                            ilGenerator.Emit(OpCodes.Call, tempDataControllerProperty.GetGetMethod());
                        }
                    }

                    // Push Action.Invoke
                    ilGenerator.Emit(OpCodes.Callvirt, actionType.GetMethod("Invoke"));

                    // This handler does not produce a result
                    localVariableForPreviousReturnValue = null;
                } 
                else if (handler.Type == FluentActionHandlerType.View 
                    || handler.Type == FluentActionHandlerType.PartialView
                    || handler.Type == FluentActionHandlerType.ViewComponent)
                {
                    if (handler.PathToView == null)
                    {
                        throw new Exception("Must specify a path to a view.");
                    }

                    var localVariableForReturnValue = ilGenerator.DeclareLocal(handler.ReturnType);

                    // Call one of the following controller methods:
                    //   Controller.View(string pathName, object model)
                    //   Controller.PartialView(string pathName, object model)
                    //   Controller.PartialView(string pathName, object arguments)

                    ilGenerator.Emit(OpCodes.Ldarg_0);
                    ilGenerator.Emit(OpCodes.Ldstr, handler.PathToView);

                    Type[] viewMethodParameterTypes = null;
                    if (localVariableForPreviousReturnValue != null)
                    {
                        ilGenerator.Emit(OpCodes.Ldloc, localVariableForPreviousReturnValue);
                        viewMethodParameterTypes = new[] { typeof(string), typeof(object) };
                    } 
                    else
                    {
                        viewMethodParameterTypes = new[] { typeof(string) };
                    }

                    MethodInfo viewMethod = null;
                    if (handler.Type == FluentActionHandlerType.View)
                    {
                        viewMethod = typeof(Controller).GetMethod("View", viewMethodParameterTypes);
                    }
                    else if (handler.Type == FluentActionHandlerType.PartialView)
                    {
                        viewMethod = typeof(Controller).GetMethod("PartialView", viewMethodParameterTypes);
                    }
                    else if (handler.Type == FluentActionHandlerType.ViewComponent)
                    {
                        viewMethod = typeof(Controller).GetMethod("ViewComponent", viewMethodParameterTypes);
                    }

                    ilGenerator.Emit(OpCodes.Callvirt, viewMethod);

                    // Push storing result in local variable
                    ilGenerator.Emit(OpCodes.Stloc, localVariableForReturnValue);

                    // Make sure next handler has access to previous handler's return value
                    localVariableForPreviousReturnValue = localVariableForReturnValue;
                }
            }
             
            // Return last handlers return value
            ilGenerator.Emit(OpCodes.Ldloc, localVariableForPreviousReturnValue);
            ilGenerator.Emit(OpCodes.Ret);
        }

        private static void SetHttpMethodAttribute(MethodBuilder methodBuilder, HttpMethod httpMethod)
        {
            var attributeConstructorInfo = GetHttpMethodAttribute(httpMethod)
                .GetConstructor(new Type[0]);
            var attributeBuilder = new CustomAttributeBuilder(attributeConstructorInfo, new Type[0]);
            methodBuilder.SetCustomAttribute(attributeBuilder);
        }

        private static void SetRouteAttribute(MethodBuilder methodBuilder, string routeTemplate)
        {
            var attributeConstructorInfo = typeof(RouteAttribute)
                .GetConstructor(new Type[] { typeof(string) });
            var attributeBuilder = new CustomAttributeBuilder(attributeConstructorInfo, new[] { routeTemplate });
            methodBuilder.SetCustomAttribute(attributeBuilder);
        }
    }

    public static class FluentActionControllerDefinitionHandlerDelegates
    {
        public static ConcurrentDictionary<string, Delegate> All = new ConcurrentDictionary<string, Delegate>();

        public static string Add(Delegate value)
        {
            var key = Guid.NewGuid().ToString();

            if (!All.TryAdd(key, value))
            {
                throw new Exception($"Tried to add a fluent action delegate but key already exists in dictionary ({key}).");
            }

            return key;
        }
    }
}

