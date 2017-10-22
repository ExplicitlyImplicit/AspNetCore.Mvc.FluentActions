// Licensed under the MIT License. See LICENSE file in the root of the solution for license information.

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentActions.Core.Builder
{
    public class ControllerMethodBuilderForFluentActionAsync : ControllerMethodBuilder
    {
        public FluentActionDefinition FluentActionDefinition { get; set; }

        public ILogger Logger { get; set; }

        internal AsyncStateMachineTypeBuilder StateMachineBuilder { get; set; }

        public ControllerMethodBuilderForFluentActionAsync(FluentActionDefinition fluentActionDefinition, ILogger logger = null)
        {
            FluentActionDefinition = fluentActionDefinition;
            Logger = logger;
        }

        public override void Build()
        {
            StateMachineBuilder = AsyncStateMachineTypeBuilder.Create(TypeBuilder, FluentActionDefinition, Logger);

            NestedTypes.Add(StateMachineBuilder.Type);

            var usingsForMethodParameters = FluentActionDefinition.Handlers
                .SelectMany(usingHandler => usingHandler.Usings)
                .Where(@using => @using.IsMethodParameter)
                .Distinct()
                .ToArray();

            var methodParameterIndices = usingsForMethodParameters
                .Select((@using, index) => new { Using = @using, Index = index })
                .ToDictionary(
                    indexedUsing => indexedUsing.Using.GetHashCode(),
                    indexedUsing => indexedUsing.Index + 1 // 1-based index
                );

            var methodParameterTypes = usingsForMethodParameters
                .Select(@using => @using.Type)
                .ToArray();

            var returnType = FluentActionDefinition.Handlers.Last().ReturnType;
            var returnTypeTask = typeof(Task<>).MakeGenericType(returnType);

            MethodBuilder.SetReturnType(returnTypeTask);
            MethodBuilder.SetParameters(methodParameterTypes);

            SetHttpMethodAttribute(FluentActionDefinition.HttpMethod);
            SetRouteAttribute(FluentActionDefinition.RouteTemplate);

            foreach (var customAttribute in FluentActionDefinition.CustomAttributes)
            {
                SetCustomAttribute(customAttribute);
            }

            foreach (var usingDefinition in usingsForMethodParameters)
            {
                var methodParameterIndex = methodParameterIndices[usingDefinition.GetHashCode()];

                usingDefinition.DefineMethodParameter(MethodBuilder, FluentActionDefinition, usingDefinition, methodParameterIndex);
            }

            var dictionaryField = typeof(FluentActionDelegates)
                .GetField("All");
            var dictionaryGetMethod = typeof(ConcurrentDictionary<,>)
                .MakeGenericType(typeof(string), typeof(Delegate))
                .GetMethod("get_Item");

            var ilGenerator = MethodBuilder.GetILGenerator();

            // === Generate IL for action method ==========================

            var asyncTaskMethodBuilderType = typeof(AsyncTaskMethodBuilder<>).MakeGenericType(returnType);

            // Declare local variables
            var stateMachineLocalVariable = ilGenerator.DeclareLocal(StateMachineBuilder.Type);

            // Create a StateMachine and store it locally
            ilGenerator.Emit(OpCodes.Newobj, StateMachineBuilder.Constructor);
            ilGenerator.Emit(OpCodes.Stloc, stateMachineLocalVariable);

            // Store reference to parent in field StateMachine.Parent
            ilGenerator.Emit(OpCodes.Ldloc, stateMachineLocalVariable);
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Stfld, StateMachineBuilder.ParentField);

            // Create an AsyncTaskMethodBuilder and store it in field StateMachine.AsyncTaskMethodBuilder
            ilGenerator.Emit(OpCodes.Ldloc, stateMachineLocalVariable);
            ilGenerator.Emit(OpCodes.Call, asyncTaskMethodBuilderType.GetMethod("Create"));
            ilGenerator.Emit(OpCodes.Stfld, StateMachineBuilder.AsyncTaskMethodBuilderField);

            // Set field StateMachine.State = 0
            ilGenerator.Emit(OpCodes.Ldloc, stateMachineLocalVariable);
            ilGenerator.Emit(OpCodes.Ldc_I4_0);
            ilGenerator.Emit(OpCodes.Stfld, StateMachineBuilder.StateField);

            // Store parameters to fields in StateMachine
            foreach (var usingDefinition in usingsForMethodParameters)
            {
                var methodParameterIndex = methodParameterIndices[usingDefinition.GetHashCode()];
                var methodParameterField = StateMachineBuilder.MethodParameterFields[methodParameterIndex - 1];

                ilGenerator.Emit(OpCodes.Ldloc, stateMachineLocalVariable);
                ilGenerator.Emit(OpCodes.Ldarg, methodParameterIndex);
                ilGenerator.Emit(OpCodes.Stfld, methodParameterField);
            }

            // Store delegates to fields in StateMachine
            foreach (var handler in StateMachineBuilder.States
                .SelectMany(state => state.Handlers)
                .Where(handler => 
                    handler.Definition.Type == FluentActionHandlerType.Func ||
                    handler.Definition.Type == FluentActionHandlerType.Action))
            {
                var delegateKey = FluentActionDelegates.Add(handler.Definition.Delegate);

                // Push Delegate
                ilGenerator.Emit(OpCodes.Ldloc, stateMachineLocalVariable);
                ilGenerator.Emit(OpCodes.Ldsfld, FluentActionDelegates.FieldInfo);
                ilGenerator.Emit(OpCodes.Ldstr, delegateKey);
                ilGenerator.Emit(OpCodes.Callvirt, FluentActionDelegates.MethodInfo);

                // Store in field StateMachine.StateXHandlerYDelegate
                ilGenerator.Emit(OpCodes.Stfld, handler.DelegateField);
            }

            // Start the AsyncTaskMethodBuilder
            ilGenerator.Emit(OpCodes.Ldloc, stateMachineLocalVariable);
            ilGenerator.Emit(OpCodes.Ldflda, StateMachineBuilder.AsyncTaskMethodBuilderField);
            ilGenerator.Emit(OpCodes.Ldloca, stateMachineLocalVariable);
            ilGenerator.Emit(OpCodes.Call, asyncTaskMethodBuilderType.GetMethod("Start").MakeGenericMethod(StateMachineBuilder.Type));

            // Return the Task of AsyncTaskMethodBuilder
            ilGenerator.Emit(OpCodes.Ldloc, stateMachineLocalVariable);
            ilGenerator.Emit(OpCodes.Ldflda, StateMachineBuilder.AsyncTaskMethodBuilderField);
            ilGenerator.Emit(OpCodes.Call, asyncTaskMethodBuilderType.GetProperty("Task").GetGetMethod());

            ilGenerator.Emit(OpCodes.Ret);
        }
    }
}

