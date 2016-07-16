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
            // TODO handle parameter usings
            // TODO handle model usings
            // TODO handle multiple usings
            // TODO handle controller usings

            var moduleBuilder = DefineModule();
            var typeBuilder = DefineType(moduleBuilder);
            var serviceFields = DefineServiceFields(typeBuilder, handler);

            DefineConstructor(typeBuilder, handler, serviceFields);
            DefineActionMethod(typeBuilder, handler, endpointDefinition, serviceFields);

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

            return moduleBuilder.DefineType(
                    name + "Controller",
                    TypeAttributes.Class | TypeAttributes.Public,
                    typeof(Controller)); ;
        }

        private static FieldBuilder[] DefineServiceFields(TypeBuilder typeBuilder, EndpointHandlerDefinition handler)
        {
            var serviceTypes = handler.Usings
                .OfType<EndpointHandlerServiceDefinition>()
                .Select(@using => @using.Type)
                .ToArray(); ;

            var definedServiceFields = new FieldBuilder[serviceTypes.Length];
            for(var index=0; index<serviceTypes.Length; index++)
            {
                var serviceType = serviceTypes[index];
                var fieldName = $"Parameter{index}{serviceType.Name}";
                definedServiceFields[index] = typeBuilder.DefineField(fieldName, serviceType, FieldAttributes.Public);
            }

            return definedServiceFields;
        }

        private static void DefineConstructor(TypeBuilder typeBuilder, EndpointHandlerDefinition handler, FieldBuilder[] fields)
        {
            var serviceTypes = handler.Usings
                .OfType<EndpointHandlerServiceDefinition>()
                .Select(@using => @using.Type)
                .ToArray(); ;

            var constructor = typeBuilder.DefineConstructor(
                MethodAttributes.Public, CallingConventions.Standard, serviceTypes);

            var constructorIL = constructor.GetILGenerator();

            for (var index = 0; index < serviceTypes.Length; index++)
            {
                constructorIL.Emit(OpCodes.Ldarg_0);
                constructorIL.Emit(OpCodes.Ldarg_1);
                constructorIL.Emit(OpCodes.Stfld, fields[index]);
            }

            constructorIL.Emit(OpCodes.Ret);
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
                default: throw new Exception("GetGenericFuncType only supports 1-8 arguments");
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

        private static void DefineActionMethod(TypeBuilder typeBuilder, EndpointHandlerDefinition handler, EndpointDefinition endpointDefinition, FieldBuilder[] fields)
        {
            var serviceTypes = handler.Usings
                .OfType<EndpointHandlerServiceDefinition>()
                .Select(@using => @using.Type)
                .ToArray();

            var parameterTypes = handler.Usings
                .OfType<EndpointHandlerParameterDefinition>()
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
            foreach (var parameter in handler.Usings.OfType<EndpointHandlerParameterDefinition>())
            {
                methodBuilder.DefineParameter(parameterIndex, ParameterAttributes.None, parameter.Name ?? $"parameter{parameterIndex}");
                parameterIndex++;
            }
            //var funcType = typeof(Func<,>).MakeGenericType(serviceTypes[0], returnType);
            var funcType = MakeGenericFuncType(handler);

            var funcKey = EndpointControllerDefinitionHandlerFuncs.Add(func.Compile());

            var fi = typeof(EndpointControllerDefinitionHandlerFuncs).GetField("All");

            var dictMethod = typeof(Dictionary<,>).MakeGenericType(typeof(string), typeof(Delegate)).GetMethod("get_Item");

            il.Emit(OpCodes.Ldsfld, fi);
            il.Emit(OpCodes.Ldstr, funcKey);
            il.Emit(OpCodes.Callvirt, dictMethod);

            var serviceFieldIndex = 0;
            var parameterUsingIndex = 1;
            foreach (var handlerUsing in handler.Usings)
            {
                if (handlerUsing is EndpointHandlerServiceDefinition)
                {
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Ldfld, fields[serviceFieldIndex++]);
                } else if (handlerUsing is EndpointHandlerParameterDefinition)
                {
                    il.Emit(OpCodes.Ldarg, parameterUsingIndex++);
                } else
                {
                    throw new Exception("Using type not yet supported");
                }
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

