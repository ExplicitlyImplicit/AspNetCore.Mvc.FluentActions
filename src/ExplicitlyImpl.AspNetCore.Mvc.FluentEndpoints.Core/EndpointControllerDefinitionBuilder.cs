using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentEndpoints
{
    public class EndpointControllerDefinitionBuilder
    {
        private const string ActionName = "HandlerAction";

        public EndpointControllerDefinition Create(EndpointBase endpoint)
        {
            if (endpoint == null)
            {
                throw new ArgumentNullException(nameof(endpoint));
            }

            if (!endpoint.EndpointDefinition.Handlers.Any())
            {
                throw new ArgumentException($"Missing handler for endpoint {endpoint}.");
            }

            var controllerTypeInfo = DefineControllerType(endpoint.EndpointDefinition);

            return new EndpointControllerDefinition()
            {
                Id = controllerTypeInfo.Name,
                Name = controllerTypeInfo.Name,
                ActionName = ActionName,
                Endpoint = endpoint,
                TypeInfo = controllerTypeInfo
            };
        }

        public static TypeInfo DefineControllerType(
            EndpointDefinition endpointDefinition)
        {
            if (endpointDefinition == null)
            {
                throw new ArgumentNullException(nameof(endpointDefinition));
            }

            var moduleBuilder = DefineModule();
            var typeBuilder = DefineType(moduleBuilder);

            DefineActionMethod(typeBuilder, endpointDefinition);

            return typeBuilder.CreateTypeInfo();
        }

        private static ModuleBuilder DefineModule()
        {
            var assemblyName = new AssemblyName("FluentEndpointAssembly");
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(
                assemblyName, 
                AssemblyBuilderAccess.Run);

            return assemblyBuilder.DefineDynamicModule("FluentEndpointModule");
        }

        private static TypeBuilder DefineType(ModuleBuilder moduleBuilder)
        {
            var guid = Guid.NewGuid().ToString().Replace("-", "");
            var name = $"FluentEndpoint{guid}Controller";

            var typeBuilder = moduleBuilder.DefineType(
                    name + "Controller",
                    TypeAttributes.Class | TypeAttributes.Public,
                    typeof(Controller));

            typeBuilder.DefineDefaultConstructor(MethodAttributes.Public);

            return typeBuilder;
        }

        private static Type MakeGenericFuncType(EndpointHandlerDefinition handler)
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
            EndpointDefinition endpointDefinition)
        {
            var usingsForMethodParameters = endpointDefinition.Handlers
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

            var returnType = endpointDefinition.Handlers.Last().ReturnType;
            var methodBuilder = typeBuilder.DefineMethod(ActionName, MethodAttributes.Public, returnType, methodParameterTypes);

            var attributeConstructorInfo = GetHttpMethodAttribute(endpointDefinition.HttpMethod)
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

                if (usingDefinition is EndpointUsingServiceDefinition)
                {
                    var parameterAttributeBuilder = new CustomAttributeBuilder(typeof(FromServicesAttribute)
                        .GetConstructor(new Type[0]), new Type[0]);
                    methodParameterBuilder.SetCustomAttribute(parameterAttributeBuilder);
                } 
                else if (usingDefinition is EndpointUsingRouteParameterDefinition)
                {
                    var attributeType = typeof(FromRouteAttribute);
                    var name = ((EndpointUsingRouteParameterDefinition)usingDefinition).Name;

                    if (!endpointDefinition.Url.Contains($"{{{name}}}", StringComparison.CurrentCultureIgnoreCase))
                    {
                        throw new Exception($"Route parameter {name} does not exist in url {endpointDefinition.Url}.");
                    }

                    var parameterAttributeBuilder = new CustomAttributeBuilder(
                        attributeType.GetConstructor(new Type[0]), 
                        new Type[0],
                        new[] { attributeType.GetProperty("Name") },
                        new object[] { name });

                    methodParameterBuilder.SetCustomAttribute(parameterAttributeBuilder);
                }
                else if (usingDefinition is EndpointUsingQueryStringParameterDefinition)
                {
                    var attributeType = typeof(FromQueryAttribute);
                    var name = ((EndpointUsingQueryStringParameterDefinition)usingDefinition).Name;

                    var parameterAttributeBuilder = new CustomAttributeBuilder(
                        attributeType.GetConstructor(new Type[0]),
                        new Type[0],
                        new[] { attributeType.GetProperty("Name") },
                        new object[] { name });

                    methodParameterBuilder.SetCustomAttribute(parameterAttributeBuilder);
                }
                else if (usingDefinition is EndpointUsingHeaderParameterDefinition)
                {
                    var attributeType = typeof(FromHeaderAttribute);
                    var name = ((EndpointUsingHeaderParameterDefinition)usingDefinition).Name;

                    var parameterAttributeBuilder = new CustomAttributeBuilder(
                        attributeType.GetConstructor(new Type[0]),
                        new Type[0],
                        new[] { attributeType.GetProperty("Name") },
                        new object[] { name });

                    methodParameterBuilder.SetCustomAttribute(parameterAttributeBuilder);
                }
                else if (usingDefinition is EndpointUsingBodyDefinition)
                {
                    var parameterAttributeBuilder = new CustomAttributeBuilder(typeof(FromBodyAttribute)
                        .GetConstructor(new Type[0]), new Type[0]);
                    methodParameterBuilder.SetCustomAttribute(parameterAttributeBuilder);
                } 
                else if (usingDefinition is EndpointUsingFormDefinition)
                {
                    var parameterAttributeBuilder = new CustomAttributeBuilder(typeof(FromFormAttribute)
                        .GetConstructor(new Type[0]), new Type[0]);
                    methodParameterBuilder.SetCustomAttribute(parameterAttributeBuilder);
                }
                else if (usingDefinition is EndpointUsingFormValueDefinition)
                {
                    var attributeType = typeof(FromFormAttribute);
                    var key = ((EndpointUsingFormValueDefinition)usingDefinition).Key;

                    var parameterAttributeBuilder = new CustomAttributeBuilder(
                        attributeType.GetConstructor(new Type[0]),
                        new Type[0],
                        new[] { attributeType.GetProperty("Name") },
                        new object[] { key });

                    methodParameterBuilder.SetCustomAttribute(parameterAttributeBuilder);
                }
                else if (usingDefinition is EndpointUsingModelBinderDefinition)
                {
                    var attributeType = typeof(ModelBinderAttribute);
                    var modelBinderType = ((EndpointUsingModelBinderDefinition)usingDefinition).ModelBinderType;

                    var parameterAttributeBuilder = new CustomAttributeBuilder(
                        attributeType.GetConstructor(new Type[0]),
                        new Type[0],
                        new[] { attributeType.GetProperty("BinderType") },
                        new object[] { modelBinderType });

                    methodParameterBuilder.SetCustomAttribute(parameterAttributeBuilder);
                }
            }

            var dictionaryField = typeof(EndpointControllerDefinitionHandlerFuncs)
                .GetField("All");
            var dictionaryGetMethod = typeof(Dictionary<,>)
                .MakeGenericType(typeof(string), typeof(Delegate))
                .GetMethod("get_Item");

            var httpContextControllerProperty = typeof(Controller).GetProperty("HttpContext");

            LocalBuilder localVariableForPreviousReturnValue = null;

            foreach (var handler in endpointDefinition.Handlers)
            {
                var customFuncType = MakeGenericFuncType(handler);
                var customFuncKey = EndpointControllerDefinitionHandlerFuncs.Add(handler.Delegate);
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
                    } else if (handlerUsing is EndpointUsingResultFromHandlerDefinition)
                    {
                        ilGenerator.Emit(OpCodes.Ldloc, localVariableForPreviousReturnValue);
                    } else if (handlerUsing is EndpointUsingHttpContextDefinition)
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

            if (endpointDefinition.PathToView != null)
            {
                // Call Controller.View(string pathName, object model) and return the results

                ilGenerator.Emit(OpCodes.Ldarg_0);
                ilGenerator.Emit(OpCodes.Ldstr, endpointDefinition.PathToView);
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

    public static class EndpointControllerDefinitionHandlerFuncs
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

