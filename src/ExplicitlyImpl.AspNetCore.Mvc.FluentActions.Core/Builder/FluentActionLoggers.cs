// Licensed under the MIT License. See LICENSE file in the root of the solution for license information.

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentActions.Core.Builder
{
    public static class FluentActionLoggers
    {
        public static ConcurrentDictionary<string, ILogger> All = new ConcurrentDictionary<string, ILogger>();

        public static string Add(ILogger logger)
        {
            var key = Guid.NewGuid().ToString();

            if (!All.TryAdd(key, logger))
            {
                throw new Exception($"Tried to add a fluent action logger but key already exists in dictionary ({key}).");
            }

            return key;
        }

        public static FieldInfo FieldInfo => typeof(FluentActionLoggers).GetField("All");

        public static MethodInfo MethodInfo => typeof(ConcurrentDictionary<string, ILogger>)
            .GetMethod("get_Item");

        private static MethodInfo EmptyArrayMethod = typeof(Array)
            .GetMethod("Empty")
            .MakeGenericMethod(typeof(object));

        private static MethodInfo LogDebugMethod = typeof(LoggerExtensions)
            .GetMethod("LogDebug", new Type[] { typeof(ILogger), typeof(string), typeof(object[]) });

        public static void PushDebugLogOntoStack(ILGenerator ilGenerator, string loggerKey, string message)
        {
            // Push the logger from FluentActionLoggers.All[loggerKey]
            ilGenerator.Emit(OpCodes.Ldsfld, FieldInfo);
            ilGenerator.Emit(OpCodes.Ldstr, loggerKey);
            ilGenerator.Emit(OpCodes.Callvirt, MethodInfo);

            // Push the message
            ilGenerator.Emit(OpCodes.Ldstr, message);

            // Push an empty array
            ilGenerator.Emit(OpCodes.Call, EmptyArrayMethod);

            // Call LogDebug(logger, message, object[0])
            ilGenerator.Emit(OpCodes.Call, LogDebugMethod);
        }
    }
}

