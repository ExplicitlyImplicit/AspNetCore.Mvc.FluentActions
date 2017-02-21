// Licensed under the MIT License. See LICENSE file in the root of the solution for license information.

using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Reflection.Emit;

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentActions.Core.Builder
{
    public static class FluentActionDelegates
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

        public static FieldInfo FieldInfo => typeof(FluentActionDelegates).GetField("All");

        public static MethodInfo MethodInfo => typeof(ConcurrentDictionary<string, Delegate>)
            .GetMethod("get_Item");

        public static void PushDelegateOntoStack(ILGenerator ilGenerator, string delegateKey)
        {
            ilGenerator.Emit(OpCodes.Ldsfld, FieldInfo);
            ilGenerator.Emit(OpCodes.Ldstr, delegateKey);
            ilGenerator.Emit(OpCodes.Callvirt, MethodInfo);
        }
    }
}

