using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

            if (fluentAction.Definition.Handlers.Count() == 1 && 
                fluentAction.Definition.Handlers.First().Type == FluentActionHandlerType.Controller)
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

                if (!(controllerTypeInfo.IsAssignableFrom(typeof(Controller))))
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
                var controllerTypeInfo = DefineControllerType(fluentAction.Definition);

                return new FluentActionControllerDefinition()
                {
                    Id = controllerTypeInfo.Name,
                    Name = controllerTypeInfo.Name,
                    ActionName = ActionName,
                    FluentAction = fluentAction,
                    TypeInfo = controllerTypeInfo
                };
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
                .Where(handler => handler.ReturnType == null))
            {
                validationResult.AddValidationError("Missing return type for handler.");
            }

            //foreach (var routeParameterNotInRoute in handlers
            //    .SelectMany(handler => handler.Usings)
            //    .OfType<FluentActionUsingRouteParameterDefinition>()
            //    .Where(@using => @using.Name))
            //{
            //    validationResult.AddValidationError("Missing return type for handler.");
            //}

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

        private static Type MakeGenericFuncType(FluentActionHandlerDefinition handler)
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

            throw new Exception($"{nameof(GetUnspecifiedGenericFuncType)} only supports up to eight arguments.");
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

            var attributeConstructorInfo = GetHttpMethodAttribute(fluentActionDefinition.HttpMethod)
                .GetConstructor(new Type[0]);
            var attributeBuilder = new CustomAttributeBuilder(attributeConstructorInfo, new Type[0]);
            methodBuilder.SetCustomAttribute(attributeBuilder);

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

                    if (!fluentActionDefinition.Url.Contains($"{{{name}}}", StringComparison.CurrentCultureIgnoreCase))
                    {
                        throw new Exception($"Route parameter {name} does not exist in url {fluentActionDefinition.Url}.");
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
                    var modelBinderType = ((FluentActionUsingModelBinderDefinition)usingDefinition).ModelBinderType;

                    var parameterAttributeBuilder = new CustomAttributeBuilder(
                        attributeType.GetConstructor(new Type[0]),
                        new Type[0],
                        new[] { attributeType.GetProperty("BinderType") },
                        new object[] { modelBinderType });

                    methodParameterBuilder.SetCustomAttribute(parameterAttributeBuilder);
                }
            }

            var dictionaryField = typeof(FluentActionControllerDefinitionHandlerFuncs)
                .GetField("All");
            var dictionaryGetMethod = typeof(ConcurrentDictionary<,>)
                .MakeGenericType(typeof(string), typeof(Delegate))
                .GetMethod("get_Item");

            var httpContextControllerProperty = typeof(Controller).GetProperty("HttpContext");

            LocalBuilder localVariableForPreviousReturnValue = null;

            foreach (var handler in fluentActionDefinition.Handlers)
            {
                var localVariableForReturnValue = ilGenerator.DeclareLocal(handler.ReturnType);

                if (handler.Type == FluentActionHandlerType.Func)
                {

                    var customFuncType = MakeGenericFuncType(handler);
                    var customFuncKey = FluentActionControllerDefinitionHandlerFuncs.Add(handler.Delegate);

                    // Push Func
                    ilGenerator.Emit(OpCodes.Ldsfld, dictionaryField);
                    ilGenerator.Emit(OpCodes.Ldstr, customFuncKey);
                    ilGenerator.Emit(OpCodes.Callvirt, dictionaryGetMethod);

                    // Push arguments for Func
                    foreach (var handlerUsing in handler.Usings)
                    {
                        if (handlerUsing.IsMethodParameter)
                        {
                            ilGenerator.Emit(OpCodes.Ldarg, methodParameterIndicesForUsings[handlerUsing.GetHashCode()]);
                        } else if (handlerUsing is FluentActionUsingResultFromHandlerDefinition)
                        {
                            ilGenerator.Emit(OpCodes.Ldloc, localVariableForPreviousReturnValue);
                        } else if (handlerUsing is FluentActionUsingHttpContextDefinition)
                        {
                            ilGenerator.Emit(OpCodes.Ldarg_0);
                            ilGenerator.Emit(OpCodes.Callvirt, httpContextControllerProperty.GetGetMethod());
                        }
                    }

                    // Push Func.Invoke
                    ilGenerator.Emit(OpCodes.Callvirt, customFuncType.GetMethod("Invoke"));
                } 
                else if (handler.Type == FluentActionHandlerType.View || handler.Type == FluentActionHandlerType.PartialView)
                {
                    if (handler.PathToView == null)
                    {
                        throw new Exception("Must specify a path to a view.");
                    }

                    // Call one of the following controller methods:
                    //   Controller.View(string pathName, object model)
                    //   Controller.PartialView(string pathName, object model)

                    ilGenerator.Emit(OpCodes.Ldarg_0);
                    ilGenerator.Emit(OpCodes.Ldstr, handler.PathToView);
                    ilGenerator.Emit(OpCodes.Ldloc, localVariableForPreviousReturnValue);

                    MethodInfo viewMethod = null;
                    if (handler.Type == FluentActionHandlerType.View)
                    {
                        viewMethod = typeof(Controller).GetMethod("View", new[] { typeof(string), typeof(object) });
                    }
                    else if (handler.Type == FluentActionHandlerType.PartialView)
                    {
                        viewMethod = typeof(Controller).GetMethod("PartialView", new[] { typeof(string), typeof(object) });
                    }
                    else if (handler.Type == FluentActionHandlerType.ViewComponent)
                    {
                        viewMethod = typeof(Controller).GetMethod("ViewComponent", new[] { typeof(string), typeof(object) });
                    }

                    ilGenerator.Emit(OpCodes.Callvirt, viewMethod);
                }

                // Push storing result in local variable
                ilGenerator.Emit(OpCodes.Stloc, localVariableForReturnValue);

                // Make sure next handler has access to previous handler's return value
                localVariableForPreviousReturnValue = localVariableForReturnValue;
            }
             
            // Return last handlers return value
            ilGenerator.Emit(OpCodes.Ldloc, localVariableForPreviousReturnValue);
            ilGenerator.Emit(OpCodes.Ret);
        }
    }

    public static class FluentActionControllerDefinitionHandlerFuncs
    {
        public static ConcurrentDictionary<string, Delegate> All = new ConcurrentDictionary<string, Delegate>();

        public static string Add(Delegate value)
        {
            var funcKey = Guid.NewGuid().ToString();

            if (!All.TryAdd(funcKey, value))
            {
                throw new Exception($"Tried to add a fluent action delegate but key already exists in dictionary ({funcKey}).");
            }

            return funcKey;
        }
    }
}

