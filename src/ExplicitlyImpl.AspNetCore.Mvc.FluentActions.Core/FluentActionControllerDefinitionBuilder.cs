using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentActions
{
    public class FluentActionControllerDefinitionBuilder
    {
        private const string ActionName = "HandlerAction";

        public FluentActionControllerDefinition Create(FluentActionBase fluentAction)
        {
            if (fluentAction == null)
            {
                throw new ArgumentNullException(nameof(fluentAction));
            }

            if (!fluentAction.Definition.Handlers.Any())
            {
                throw new ArgumentException($"Missing handler for action {fluentAction}.");
            }

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

        public static TypeInfo DefineControllerType(
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
                    name + "Controller",
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
                    ParameterAttributes.None, 
                    $"parameter{methodParameterIndex}");

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
            var dictionaryGetMethod = typeof(Dictionary<,>)
                .MakeGenericType(typeof(string), typeof(Delegate))
                .GetMethod("get_Item");

            var httpContextControllerProperty = typeof(Controller).GetProperty("HttpContext");

            LocalBuilder localVariableForPreviousReturnValue = null;

            foreach (var handler in fluentActionDefinition.Handlers)
            {
                var customFuncType = MakeGenericFuncType(handler);
                var customFuncKey = FluentActionControllerDefinitionHandlerFuncs.Add(handler.Delegate);
                var localVariableForReturnValue = ilGenerator.DeclareLocal(handler.ReturnType);

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

                // Push storing result in local variable
                ilGenerator.Emit(OpCodes.Stloc, localVariableForReturnValue);

                // Make sure next handler has access to previous handler's return value
                localVariableForPreviousReturnValue = localVariableForReturnValue;
            }

            if (fluentActionDefinition.PathToView != null)
            {
                // Call Controller.View(string pathName, object model) and return the results

                ilGenerator.Emit(OpCodes.Ldarg_0);
                ilGenerator.Emit(OpCodes.Ldstr, fluentActionDefinition.PathToView);
                ilGenerator.Emit(OpCodes.Ldloc, localVariableForPreviousReturnValue);

                var viewMethod = typeof(Controller).GetMethod("View", new[] { typeof(string), typeof(object) });
                ilGenerator.Emit(OpCodes.Callvirt, viewMethod);
            } else
            {
                // Return last handlers return value
                ilGenerator.Emit(OpCodes.Ldloc, localVariableForPreviousReturnValue);
            }

            ilGenerator.Emit(OpCodes.Ret);
        }
    }

    public static class FluentActionControllerDefinitionHandlerFuncs
    {
        public static Dictionary<string, Delegate> All = new Dictionary<string, Delegate>();

        public static string Add(Delegate value)
        {
            var funcKey = Guid.NewGuid().ToString();
            All.Add(funcKey, value);
            return funcKey;
        }
    }
}

