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

        public EndpointControllerDefinition Create(Endpoint endpoint)
        {
            if (endpoint == null)
            {
                throw new ArgumentNullException(nameof(endpoint));
            }

            if (!endpoint.EndpointDefinition.Handlers.Any())
            {
                throw new ArgumentException($"Missing handler for endpoint {endpoint}.");
            }

            if (endpoint.EndpointDefinition.Handlers.Count() > 1)
            {
                throw new ArgumentException($"More than one handler is not currently supported (seen in endpoint {endpoint}).");
            }

            var handler = endpoint.EndpointDefinition.Handlers.First();

            var controllerTypeInfo = DefineControllerTypeForHandler(handler, endpoint.EndpointDefinition);

            return new EndpointControllerDefinition()
            {
                Id = controllerTypeInfo.Name,
                Name = controllerTypeInfo.Name,
                ActionName = ActionName,
                Endpoint = endpoint,
                TypeInfo = controllerTypeInfo
            };
        }

        private static TypeInfo DefineControllerTypeForHandler(
            EndpointHandlerDefinition handler, 
            EndpointDefinition endpointDefinition)
        {
            var moduleBuilder = DefineModule();
            var typeBuilder = DefineType(moduleBuilder);

            DefineActionMethod(typeBuilder, handler, endpointDefinition);

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
            EndpointHandlerDefinition handler, 
            EndpointDefinition endpointDefinition)
        {
            var parameterTypes = handler.Usings
                .Select(@using => @using.Type)
                .ToArray();

            var returnType = handler.ReturnType;
            var methodBuilder = typeBuilder.DefineMethod(ActionName, MethodAttributes.Public, returnType, parameterTypes);

            var attributeConstructorInfo = GetHttpMethodAttribute(endpointDefinition.HttpMethod)
                .GetConstructor(new Type[0]);
            var attributeBuilder = new CustomAttributeBuilder(attributeConstructorInfo, new Type[0]);
            methodBuilder.SetCustomAttribute(attributeBuilder);

            var ilGenerator = methodBuilder.GetILGenerator();

            var parameterIndex = 1;
            foreach (var usingDefinition in handler.Usings)
            {
                var parameterBuilder = methodBuilder.DefineParameter(
                    parameterIndex, 
                    ParameterAttributes.None, 
                    $"parameter{parameterIndex}");

                if (usingDefinition is EndpointUsingServiceDefinition)
                {
                    var parameterAttributeBuilder = new CustomAttributeBuilder(typeof(FromServicesAttribute)
                        .GetConstructor(new Type[0]), new Type[0]);
                    parameterBuilder.SetCustomAttribute(parameterAttributeBuilder);
                } 
                else if (usingDefinition is EndpointUsingRouteParameterDefinition)
                {
                    var parameterAttributeBuilder = new CustomAttributeBuilder(typeof(FromRouteAttribute)
                        .GetConstructor(new Type[0]), new Type[0]);
                    parameterBuilder.SetCustomAttribute(parameterAttributeBuilder);
                }
                else if (usingDefinition is EndpointUsingQueryStringParameterDefinition)
                {
                    var parameterAttributeBuilder = new CustomAttributeBuilder(typeof(FromQueryAttribute)
                        .GetConstructor(new Type[0]), new Type[0]);
                    parameterBuilder.SetCustomAttribute(parameterAttributeBuilder);
                }
                else if (usingDefinition is EndpointUsingBodyDefinition)
                {
                    var parameterAttributeBuilder = new CustomAttributeBuilder(typeof(FromBodyAttribute)
                        .GetConstructor(new Type[0]), new Type[0]);
                    parameterBuilder.SetCustomAttribute(parameterAttributeBuilder);
                } 
                else if (usingDefinition is EndpointUsingFormDefinition)
                {
                    var parameterAttributeBuilder = new CustomAttributeBuilder(typeof(FromFormAttribute)
                        .GetConstructor(new Type[0]), new Type[0]);
                    parameterBuilder.SetCustomAttribute(parameterAttributeBuilder);
                }
                else if (usingDefinition is EndpointUsingFormValueDefinition)
                {
                    var parameterAttributeBuilder = new CustomAttributeBuilder(typeof(FromFormAttribute)
                        .GetConstructor(new Type[0]), new Type[0]);
                    parameterBuilder.SetCustomAttribute(parameterAttributeBuilder);
                }

                parameterIndex++;
            }

            var customFuncType = MakeGenericFuncType(handler);
            var customFuncKey = EndpointControllerDefinitionHandlerFuncs.Add(handler.Func.Compile());

            var dictionaryField = typeof(EndpointControllerDefinitionHandlerFuncs)
                .GetField("All");
            var dictionaryGetMethod = typeof(Dictionary<,>)
                .MakeGenericType(typeof(string), typeof(Delegate))
                .GetMethod("get_Item");

            ilGenerator.Emit(OpCodes.Ldsfld, dictionaryField);
            ilGenerator.Emit(OpCodes.Ldstr, customFuncKey);
            ilGenerator.Emit(OpCodes.Callvirt, dictionaryGetMethod);

            for (var usingIndex = 1; usingIndex <= handler.Usings.Count; usingIndex++)
            {
                ilGenerator.Emit(OpCodes.Ldarg, usingIndex);
            }

            ilGenerator.Emit(OpCodes.Callvirt, customFuncType.GetMethod("Invoke"));
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

