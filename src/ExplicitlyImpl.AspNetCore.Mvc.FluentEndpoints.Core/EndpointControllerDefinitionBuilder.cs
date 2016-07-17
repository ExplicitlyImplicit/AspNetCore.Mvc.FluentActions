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

        private static TypeInfo DefineControllerTypeForHandler(EndpointHandlerDefinition handler, EndpointDefinition endpointDefinition)
        {
            var moduleBuilder = DefineModule();
            var typeBuilder = DefineType(moduleBuilder);

            DefineActionMethod(typeBuilder, handler, endpointDefinition);

            return typeBuilder.CreateTypeInfo();
        }

        private static ModuleBuilder DefineModule()
        {
            var assemblyName = new AssemblyName("FluentEndpointAssembly");
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);

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
            var genericFuncType = unspecifiedGenericFuncType.MakeGenericType(argumentTypes.ToArray());

            return genericFuncType;
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
                default: throw new Exception("GetGenericFuncType only supports up to 8 arguments.");
            }
        }

        private static Type GetHttpMethodAttribute(HttpMethod httpMethod)
        {
            switch (httpMethod)
            {
                case HttpMethod.Get: return typeof(HttpGetAttribute);
                case HttpMethod.Post: return typeof(HttpPostAttribute);
                case HttpMethod.Put: return typeof(HttpPutAttribute);
                case HttpMethod.Delete: return typeof(HttpDeleteAttribute);
            }

            throw new Exception($"Could not get corresponding attribute of HttpMethod {httpMethod}.");
        }

        private static void DefineActionMethod(TypeBuilder typeBuilder, EndpointHandlerDefinition handler, EndpointDefinition endpointDefinition)
        {
            var parameterTypes = handler.Usings
                .Select(@using => @using.Type)
                .ToArray();

            var returnType = handler.ReturnType;
            var func = handler.Func;
            var methodBuilder = typeBuilder.DefineMethod(ActionName, MethodAttributes.Public, returnType, parameterTypes);

            var attributeConstructorInfo = GetHttpMethodAttribute(endpointDefinition.HttpMethod).GetConstructor(new Type[0]);
            var attributeBuilder = new CustomAttributeBuilder(attributeConstructorInfo, new Type[0]);
            methodBuilder.SetCustomAttribute(attributeBuilder);

            var il = methodBuilder.GetILGenerator();

            var parameterIndex = 1;
            foreach (var usingDefinition in handler.Usings)
            {
                var parameterBuilder = methodBuilder.DefineParameter(parameterIndex, ParameterAttributes.None, $"parameter{parameterIndex}");

                if (usingDefinition is EndpointUsingModelFromBodyDefinition)
                {
                    var parameterAttributeBuilder = new CustomAttributeBuilder(typeof(FromBodyAttribute).GetConstructor(new Type[0]), new Type[0]);
                    parameterBuilder.SetCustomAttribute(parameterAttributeBuilder);
                } else if (usingDefinition is EndpointUsingServiceDefinition)
                {
                    var parameterAttributeBuilder = new CustomAttributeBuilder(typeof(FromServicesAttribute).GetConstructor(new Type[0]), new Type[0]);
                    parameterBuilder.SetCustomAttribute(parameterAttributeBuilder);
                }

                parameterIndex++;
            }

            var funcType = MakeGenericFuncType(handler);

            var funcKey = EndpointControllerDefinitionHandlerFuncs.Add(func.Compile());

            var fi = typeof(EndpointControllerDefinitionHandlerFuncs).GetField("All");

            var dictMethod = typeof(Dictionary<,>).MakeGenericType(typeof(string), typeof(Delegate)).GetMethod("get_Item");

            il.Emit(OpCodes.Ldsfld, fi);
            il.Emit(OpCodes.Ldstr, funcKey);
            il.Emit(OpCodes.Callvirt, dictMethod);

            var usingIndex = 1;
            foreach (var handlerUsing in handler.Usings)
            {
                il.Emit(OpCodes.Ldarg, usingIndex++);
            }

            il.Emit(OpCodes.Callvirt, funcType.GetMethod("Invoke"));
            il.Emit(OpCodes.Ret);
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

