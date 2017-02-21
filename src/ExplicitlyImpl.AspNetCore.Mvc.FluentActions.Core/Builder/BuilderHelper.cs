using System;
using System.Linq;
using System.Threading.Tasks;

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentActions.Core.Builder
{
    public static class BuilderHelper
    {
        public static Type GetDelegateType(FluentActionHandlerDefinition handler)
        {
            if (handler.Type == FluentActionHandlerType.Action && !handler.Async)
            {
                return GetActionType(handler);
            } else
            {
                return GetFuncType(handler);
            }
        }

        public static Type GetReturnTypeOrTaskType(FluentActionHandlerDefinition handler)
        {
            if (handler.Async && handler.ReturnType == null)
            {
                return typeof(Task);
            } 
            else if (handler.Async && handler.ReturnType != null)
            {
                return typeof(Task<>).MakeGenericType(handler.ReturnType);
            } 
            else
            {
                return handler.ReturnType;
            }
        }

        public static Type GetFuncType(FluentActionHandlerDefinition handler)
        {
            var argumentTypes = handler.Usings.Select(@using => @using.Type).ToList();

            var resultType = GetReturnTypeOrTaskType(handler);

            argumentTypes.Add(resultType);

            var unspecifiedGenericFuncType = GetUnspecifiedGenericFuncType(argumentTypes.Count);
            var specifiedGenericFuncType = unspecifiedGenericFuncType.MakeGenericType(argumentTypes.ToArray());

            return specifiedGenericFuncType;
        }

        public static Type GetUnspecifiedGenericFuncType(int arguments)
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

        public static Type GetActionType(FluentActionHandlerDefinition handler)
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

        public static Type GetUnspecifiedGenericActionType(int arguments)
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
    }
}
