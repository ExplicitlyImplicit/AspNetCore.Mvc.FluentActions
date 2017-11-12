// Licensed under the MIT License. See LICENSE file in the root of the solution for license information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentActions.Core.Builder
{
    public class AsyncStateMachineTypeBuilder
    {
        public TypeBuilder ParentType { get; set; }

        public TypeBuilder Type { get; set; }

        public FluentActionDefinition FluentActionDefinition { get; set; }

        public ConstructorBuilder Constructor { get; set; }

        public FieldBuilder ParentField { get; set; }

        public FieldBuilder AsyncTaskMethodBuilderField { get; set; }

        public FieldBuilder TaskDelayAwaiterField { get; set; }

        public FieldBuilder StateField { get; set; }

        public FieldBuilder[] MethodParameterFields { get; set; }

        public StateMachineState[] States { get; set; }

        public Type ReturnType { get; set; }

        public ILogger Logger { get; set; }

        public string LoggerKey { get; set; }

        public static AsyncStateMachineTypeBuilder Create(
            TypeBuilder parentTypeBuilder,
            FluentActionDefinition fluentActionDefinition, 
            ILogger logger = null)
        {
            var builder = new AsyncStateMachineTypeBuilder
            {
                ParentType = parentTypeBuilder,
                ReturnType = fluentActionDefinition.Handlers.Last().ReturnType,
                Logger = logger
            };

            if (logger != null)
            {
                builder.LoggerKey = FluentActionLoggers.Add(logger);
            }

            builder.DefineTypeAndDefaultConstructor(parentTypeBuilder);
            builder.DefineFields(fluentActionDefinition);
            builder.DefineMoveNextMethod(fluentActionDefinition);
            builder.DefineSetStateMachineMethod();

            return builder;
        }

        private void DefineTypeAndDefaultConstructor(TypeBuilder parentTypeBuilder)
        {
            Type = parentTypeBuilder.DefineNestedType(
                $"{parentTypeBuilder.Name}_AsyncStateMachine",
                    TypeAttributes.Class |
                    TypeAttributes.NestedPrivate |
                    TypeAttributes.Sealed |
                    TypeAttributes.AutoClass |
                    TypeAttributes.AnsiClass |
                    TypeAttributes.BeforeFieldInit);

            Type.AddInterfaceImplementation(typeof(IAsyncStateMachine));

            Constructor = Type.DefineDefaultConstructor(MethodAttributes.Public);
        }

        private void DefineFields(FluentActionDefinition fluentActionDefinition)
        {
            var asyncTaskMethodBuilderType = typeof(AsyncTaskMethodBuilder<>).MakeGenericType(ReturnType);

            ParentField = Type.DefineField("Parent", ParentType, FieldAttributes.Public);
            AsyncTaskMethodBuilderField = Type.DefineField("AsyncTaskMethodBuilder", asyncTaskMethodBuilderType, FieldAttributes.Public);
            StateField = Type.DefineField("State", typeof(int), FieldAttributes.Public);

            DefineMethodParameterFields(fluentActionDefinition);
            DefineStates(fluentActionDefinition);
        }

        private void DefineMethodParameterFields(FluentActionDefinition fluentActionDefinition)
        {
            var usingsForMethodParameters = fluentActionDefinition.Handlers
                .SelectMany(handler => handler.Usings)
                .Where(@using => @using.IsMethodParameter)
                .Distinct()
                .ToArray();

            var methodParameterIndices = usingsForMethodParameters
                .Select((@using, index) => new { Using = @using, Index = index })
                .ToDictionary(
                    indexedUsing => indexedUsing.Using.GetHashCode(),
                    indexedUsing => indexedUsing.Index + 1 // 1-based index
                );

            MethodParameterFields = new FieldBuilder[usingsForMethodParameters.Length];

            foreach (var usingDefinition in usingsForMethodParameters)
            {
                var methodParameterIndex = methodParameterIndices[usingDefinition.GetHashCode()];

                MethodParameterFields[methodParameterIndex - 1] = Type.DefineField(
                    $"parameter{methodParameterIndex}",
                    usingDefinition.Type,
                    FieldAttributes.Public);
            }
        }

        private void DefineStates(FluentActionDefinition fluentActionDefinition)
        {
            var handlersPerState = fluentActionDefinition.Handlers
                .Divide(handler => handler.Async)
                .Select(handlers => handlers.ToArray())
                .ToArray();

            States = new StateMachineState[handlersPerState.Length];

            for (var stateIndex = 0; stateIndex < handlersPerState.Length; stateIndex++)
            {
                var handlersInState = handlersPerState[stateIndex];
                var state = new StateMachineState
                {
                    Handlers = new StateMachineStateHandler[handlersInState.Length]
                };

                for (var handlerIndex = 0; handlerIndex < handlersInState.Length; handlerIndex++)
                {
                    var handlerDefinition = handlersInState[handlerIndex];

                    state.Handlers[handlerIndex] = new StateMachineStateHandler
                    {
                        Definition = handlerDefinition
                    };

                    if (handlerDefinition.Type == FluentActionHandlerType.Func)
                    {
                        state.Handlers[handlerIndex].DelegateField = Type.DefineField(
                            $"State{stateIndex}Handler{handlerIndex}Delegate",
                            BuilderHelper.GetDelegateType(handlerDefinition),
                            FieldAttributes.Public);

                        state.Handlers[handlerIndex].ResultField = Type.DefineField(
                            $"State{stateIndex}Handler{handlerIndex}Result",
                            handlerDefinition.ReturnType,
                            FieldAttributes.Public);
                    } 
                    else if (handlerDefinition.Type == FluentActionHandlerType.Action)
                    {
                        state.Handlers[handlerIndex].DelegateField = Type.DefineField(
                            $"State{stateIndex}Handler{handlerIndex}Delegate",
                            BuilderHelper.GetDelegateType(handlerDefinition),
                            FieldAttributes.Public);
                    }
                    else if (
                        handlerDefinition.Type == FluentActionHandlerType.View ||
                        handlerDefinition.Type == FluentActionHandlerType.PartialView ||
                        handlerDefinition.Type == FluentActionHandlerType.ViewComponent)
                    {
                        state.Handlers[handlerIndex].ResultField = Type.DefineField(
                            $"State{stateIndex}Handler{handlerIndex}Result",
                            handlerDefinition.ReturnType,
                            FieldAttributes.Public);
                    }
                }

                var lastHandlerInState = handlersInState.Last();
                if (lastHandlerInState.Type == FluentActionHandlerType.Func)
                {
                    state.ResultType = lastHandlerInState.ReturnType;

                    state.ResultField = Type.DefineField(
                        $"State{stateIndex}ReturnType",
                        state.ResultType,
                        FieldAttributes.Public);

                    state.TaskAwaiterType = typeof(TaskAwaiter<>)
                        .MakeGenericType(lastHandlerInState.ReturnType);

                    state.TaskAwaiterField = Type.DefineField(
                        $"State{stateIndex}Awaiter",
                        state.TaskAwaiterType,
                        FieldAttributes.Public);

                    state.WaitingField = Type.DefineField(
                        $"State{stateIndex}WaitingFlag",
                        typeof(bool),
                        FieldAttributes.Public);
                } 
                else if (lastHandlerInState.Type == FluentActionHandlerType.Action)
                {
                    state.ResultType = lastHandlerInState.ReturnType;

                    // state.ResultField is not used since an Action returns void

                    state.TaskAwaiterType = typeof(TaskAwaiter);

                    state.TaskAwaiterField = Type.DefineField(
                        $"State{stateIndex}Awaiter",
                        state.TaskAwaiterType,
                        FieldAttributes.Public);

                    state.WaitingField = Type.DefineField(
                        $"State{stateIndex}WaitingFlag",
                        typeof(bool),
                        FieldAttributes.Public);
                } 
                else if (lastHandlerInState.Type == FluentActionHandlerType.View
                    || lastHandlerInState.Type == FluentActionHandlerType.PartialView
                    || lastHandlerInState.Type == FluentActionHandlerType.ViewComponent)
                {
                    state.ResultType = lastHandlerInState.ReturnType;

                    state.ResultField = Type.DefineField(
                        $"State{stateIndex}ReturnType",
                        state.ResultType,
                        FieldAttributes.Public);
                } 

                States[stateIndex] = state;
            }
        }

        private void DefineMoveNextMethod(FluentActionDefinition fluentActionDefinition)
        {
            var usingsForMethodParameters = fluentActionDefinition.Handlers
                .SelectMany(handler => handler.Usings)
                .Where(@using => @using.IsMethodParameter)
                .Distinct()
                .ToArray();

            var methodParameterIndices = usingsForMethodParameters
                .Select((@using, index) => new { Using = @using, Index = index })
                .ToDictionary(
                    indexedUsing => indexedUsing.Using.GetHashCode(),
                    indexedUsing => indexedUsing.Index + 1 // 1-based index
                );

            var asyncTaskMethodBuilderType = typeof(AsyncTaskMethodBuilder<>).MakeGenericType(ReturnType);

            var handlersForEachState = fluentActionDefinition.Handlers
                .Divide(handler => handler.Async)
                .Select(handlers => handlers.ToArray())
                .ToArray();

            var moveNextBuilder = Type.DefineMethod("MoveNext", MethodAttributes.Public | MethodAttributes.Virtual);
            Type.DefineMethodOverride(moveNextBuilder, typeof(IAsyncStateMachine).GetMethod("MoveNext"));

            moveNextBuilder.SetImplementationFlags(MethodImplAttributes.Managed);

            var ilGenerator = moveNextBuilder.GetILGenerator();

            EmitDebugLog(ilGenerator, "Starting up...");

            for (var i = 0; i < handlersForEachState.Length; i++)
            {
                States[i].StartLabel = ilGenerator.DefineLabel();
                States[i].WaitLabel = ilGenerator.DefineLabel();
                States[i].FinishLabel = ilGenerator.DefineLabel();
            }

            var statesStartLabels = States.Select(state => state.StartLabel).ToArray();
            var leaveLabel = ilGenerator.DefineLabel();

            var localVariableForThis = ilGenerator.DeclareLocal(Type);
            var exceptionLocalVariable = ilGenerator.DeclareLocal(typeof(Exception));

            var exceptionBlock = ilGenerator.BeginExceptionBlock();

            EmitDebugLog(ilGenerator, "Checking State < 0");

            // If State < 0 it means we've got an exception and should leave
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldfld, StateField);
            ilGenerator.Emit(OpCodes.Ldc_I4_0);
            ilGenerator.Emit(OpCodes.Blt, leaveLabel);

            EmitDebugLog(ilGenerator, "Checking State >= handlersForEachState.Length");

            // If State >= handlersForEachState.Length it means we've got a result and should leave
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldfld, StateField);
            ilGenerator.Emit(OpCodes.Ldc_I4, handlersForEachState.Length);
            ilGenerator.Emit(OpCodes.Bge, leaveLabel);

            EmitDebugLog(ilGenerator, "Switching on State");

            // Jump to state (as long as 0 <= State && State < asyncHandlers.Length)
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldfld, StateField);
            ilGenerator.Emit(OpCodes.Switch, statesStartLabels);

            // ====== Leave-block ====== (With label so you can jump to it)
            ilGenerator.MarkLabel(leaveLabel);
            ilGenerator.Emit(OpCodes.Leave, exceptionBlock);

            // Define each state-block
            var stateIndex = 0;
            var lastStateIndex = States.Length - 1;
            foreach (var state in States)
            {
                // ====== State-block ====== (Run all handlers in state and, potentially, wait for result of last async handler)
                ilGenerator.MarkLabel(state.StartLabel);

                EmitDebugLog(ilGenerator, $"State{stateIndex}::Start");

                var taskAwaiterType = state.ResultType != null ? 
                    typeof(TaskAwaiter<>).MakeGenericType(state.ResultType)
                    : typeof(TaskAwaiter);
                var taskAwaiterIsCompletedGetMethod = taskAwaiterType.GetProperty("IsCompleted").GetGetMethod();
                var taskAwaiterGetResultMethod = taskAwaiterType.GetMethod("GetResult");

                EmitDebugLog(ilGenerator, $"State{stateIndex}::Checking wait-flag");

                if (state.Async)
                {
                    // If [Waiting]: jump to wait-block
                    ilGenerator.Emit(OpCodes.Ldarg_0);
                    ilGenerator.Emit(OpCodes.Ldfld, state.WaitingField);
                    ilGenerator.Emit(OpCodes.Brtrue, state.WaitLabel);
                }

                EmitDebugLog(ilGenerator, $"State{stateIndex}::Not waiting, running handlers");

                // Else: Run all handlers in state
                for (var handlerInStateIndex = 0; handlerInStateIndex < state.Handlers.Length; handlerInStateIndex++)
                {
                    var handlerInState = state.Handlers[handlerInStateIndex];
                    var handler = handlerInState.Definition;

                    EmitDebugLog(ilGenerator, $"State{stateIndex}::Handler::Start");

                    if (handler.Type == FluentActionHandlerType.Func)
                    {
                        EmitDebugLog(ilGenerator, $"State{stateIndex}::Handler::Beginning Func.Invoke");

                        var resultType = BuilderHelper.GetReturnTypeOrTaskType(handler);
                        var localVariableForResult = ilGenerator.DeclareLocal(resultType);
                        var funcType = BuilderHelper.GetFuncType(handler);

                        EmitDebugLog(ilGenerator, $"State{stateIndex}::Handler::Pushing Delegate");

                        // Push Delegate
                        ilGenerator.Emit(OpCodes.Ldarg_0);
                        ilGenerator.Emit(OpCodes.Ldfld, handlerInState.DelegateField);

                        EmitDebugLog(ilGenerator, $"State{stateIndex}::Handler::Pushing arguments for Delegate");

                        // Push arguments for Delegate
                        foreach (var usingDefinition in handler.Usings)
                        {
                            if (usingDefinition.IsMethodParameter)
                            {
                                var usingDefinitionHash = usingDefinition.GetHashCode();
                                var methodParameterIndex = methodParameterIndices[usingDefinitionHash];

                                ilGenerator.Emit(OpCodes.Ldarg_0);
                                ilGenerator.Emit(OpCodes.Ldfld, MethodParameterFields[methodParameterIndex - 1]);
                            } else if (usingDefinition.IsControllerProperty)
                            {
                                ilGenerator.Emit(OpCodes.Ldarg_0);
                                ilGenerator.Emit(OpCodes.Ldfld, ParentField);
                                ilGenerator.Emit(OpCodes.Callvirt,
                                    typeof(Controller).GetProperty(usingDefinition.ControllerPropertyName).GetGetMethod());
                            } else if (usingDefinition is FluentActionUsingPropertyDefinition)
                            {
                                var propertyName = ((FluentActionUsingPropertyDefinition)usingDefinition).PropertyName;
                                var parentType = fluentActionDefinition.ParentType ?? typeof(Controller);
                                var property = parentType.GetProperty(propertyName);
                                if (property == null)
                                {
                                    throw new Exception($"Could not find property {propertyName} on type {parentType.FullName}.");
                                }

                                var propertyGetMethod = property.GetGetMethod();
                                if (propertyGetMethod == null)
                                {
                                    throw new Exception($"Missing public get method on property {propertyName} on type {parentType.FullName}.");
                                }

                                ilGenerator.Emit(OpCodes.Ldarg_0);
                                ilGenerator.Emit(OpCodes.Ldfld, ParentField);
                                ilGenerator.Emit(OpCodes.Callvirt, propertyGetMethod);
                            } else if (usingDefinition is FluentActionUsingParentDefinition)
                            {
                                ilGenerator.Emit(OpCodes.Ldarg_0);
                                ilGenerator.Emit(OpCodes.Ldfld, ParentField);
                            } else if (usingDefinition is FluentActionUsingResultDefinition)
                            {
                                FieldBuilder resultFieldToLoad;
                                if (handlerInStateIndex > 0)
                                {
                                    resultFieldToLoad = state.Handlers[handlerInStateIndex - 1].ResultField;
                                }
                                else
                                {
                                    resultFieldToLoad = States[stateIndex - 1].ResultField;
                                }

                                ilGenerator.Emit(OpCodes.Ldarg_0);
                                ilGenerator.Emit(OpCodes.Ldfld, resultFieldToLoad);

                                if (resultFieldToLoad.FieldType.IsValueType)
                                {
                                    ilGenerator.Emit(OpCodes.Box, resultFieldToLoad.FieldType);
                                }
                            } else
                            {
                                throw new Exception($"Got unknown using definition: {usingDefinition.GetType()}");
                            }
                        }

                        EmitDebugLog(ilGenerator, $"State{stateIndex}::Handler::Invoking Delegate");

                        // Push Func.Invoke
                        ilGenerator.Emit(OpCodes.Callvirt, funcType.GetMethod("Invoke"));

                        EmitDebugLog(ilGenerator, $"State{stateIndex}::Handler::Result in local variable");

                        // Push result in local variable
                        ilGenerator.Emit(OpCodes.Stloc, localVariableForResult);

                        if (handler.Async)
                        {
                            EmitDebugLog(ilGenerator, $"State{stateIndex}::Handler::State.Awaiter = result.GetAwaiter();");

                            // State.Awaiter = result.GetAwaiter();
                            ilGenerator.Emit(OpCodes.Ldarg_0);
                            ilGenerator.Emit(OpCodes.Ldloc, localVariableForResult);
                            ilGenerator.Emit(OpCodes.Call, resultType.GetMethod("GetAwaiter"));
                            ilGenerator.Emit(OpCodes.Stfld, state.TaskAwaiterField);

                            EmitDebugLog(ilGenerator, $"State{stateIndex}::Handler::State.Waiting = true;");

                            // State.Waiting = true;
                            ilGenerator.Emit(OpCodes.Ldarg_0);
                            ilGenerator.Emit(OpCodes.Ldc_I4_1);
                            ilGenerator.Emit(OpCodes.Stfld, state.WaitingField);

                            EmitDebugLog(ilGenerator, $"State{stateIndex}::Handler::If [result.IsCompleted]: Goto finish;");
                            // If [result.IsCompleted]: Goto finish
                            ilGenerator.Emit(OpCodes.Ldloca, localVariableForResult);
                            ilGenerator.Emit(OpCodes.Call, resultType.GetProperty("IsCompleted").GetGetMethod());
                            ilGenerator.Emit(OpCodes.Brtrue, state.FinishLabel);

                            // Else: AwaitUnsafeOnCompleted(ref Awaiter, ref self)

                            EmitDebugLog(ilGenerator, $"State{stateIndex}::Handler::Else: AwaitUnsafeOnCompleted(ref Awaiter, ref self)");

                            // Store executing instance ("this") in a local variable
                            ilGenerator.Emit(OpCodes.Ldarg_0);
                            ilGenerator.Emit(OpCodes.Stloc, localVariableForThis);

                            ilGenerator.Emit(OpCodes.Ldarg_0);
                            ilGenerator.Emit(OpCodes.Ldflda, AsyncTaskMethodBuilderField);
                            ilGenerator.Emit(OpCodes.Ldarg_0);
                            ilGenerator.Emit(OpCodes.Ldflda, state.TaskAwaiterField);
                            ilGenerator.Emit(OpCodes.Ldloca, localVariableForThis);
                            ilGenerator.Emit(OpCodes.Call, asyncTaskMethodBuilderType
                                .GetMethod("AwaitUnsafeOnCompleted")
                                .MakeGenericMethod(state.TaskAwaiterType, Type));

                            EmitDebugLog(ilGenerator, $"State{stateIndex}::Handler::Called AwaitUnsafeOnCompleted.");
                        } 
                        else if (handlerInStateIndex == state.Handlers.Length - 1)
                        {
                            ilGenerator.Emit(OpCodes.Ldarg_0);
                            ilGenerator.Emit(OpCodes.Ldloc, localVariableForResult);
                            ilGenerator.Emit(OpCodes.Stfld, state.ResultField);

                            ilGenerator.Emit(OpCodes.Br, state.FinishLabel);
                        } 
                        else
                        {
                            ilGenerator.Emit(OpCodes.Ldarg_0);
                            ilGenerator.Emit(OpCodes.Ldloc, localVariableForResult);
                            ilGenerator.Emit(OpCodes.Stfld, handlerInState.ResultField);
                        }
                    }
                    else if (handler.Type == FluentActionHandlerType.Action)
                    {
                        EmitDebugLog(ilGenerator, $"State{stateIndex}::Handler::Beginning Action.Invoke");

                        var resultType = typeof(Task); 
                        var localVariableForResult = ilGenerator.DeclareLocal(resultType);
                        var funcType = BuilderHelper.GetDelegateType(handler);

                        EmitDebugLog(ilGenerator, $"State{stateIndex}::Handler::Pushing Delegate");

                        // Push Delegate
                        ilGenerator.Emit(OpCodes.Ldarg_0);
                        ilGenerator.Emit(OpCodes.Ldfld, handlerInState.DelegateField);

                        EmitDebugLog(ilGenerator, $"State{stateIndex}::Handler::Pushing arguments for Delegate");

                        // Push arguments for Delegate
                        foreach (var usingDefinition in handler.Usings)
                        {
                            if (usingDefinition.IsMethodParameter)
                            {
                                var usingDefinitionHash = usingDefinition.GetHashCode();
                                var methodParameterIndex = methodParameterIndices[usingDefinitionHash];

                                ilGenerator.Emit(OpCodes.Ldarg_0);
                                ilGenerator.Emit(OpCodes.Ldfld, MethodParameterFields[methodParameterIndex - 1]);
                            } else if (usingDefinition.IsControllerProperty)
                            {
                                ilGenerator.Emit(OpCodes.Ldarg_0);
                                ilGenerator.Emit(OpCodes.Ldfld, ParentField);
                                ilGenerator.Emit(OpCodes.Callvirt,
                                    typeof(Controller).GetProperty(usingDefinition.ControllerPropertyName).GetGetMethod());
                            } else if (usingDefinition is FluentActionUsingPropertyDefinition)
                            {
                                var propertyName = ((FluentActionUsingPropertyDefinition)usingDefinition).PropertyName;
                                var parentType = fluentActionDefinition.ParentType ?? typeof(Controller);
                                var property = parentType.GetProperty(propertyName);
                                if (property == null)
                                {
                                    throw new Exception($"Could not find property {propertyName} on type {parentType.FullName}.");
                                }

                                var propertyGetMethod = property.GetGetMethod();
                                if (propertyGetMethod == null)
                                {
                                    throw new Exception($"Missing public get method on property {propertyName} on type {parentType.FullName}.");
                                }

                                ilGenerator.Emit(OpCodes.Ldarg_0);
                                ilGenerator.Emit(OpCodes.Ldfld, ParentField);
                                ilGenerator.Emit(OpCodes.Callvirt, propertyGetMethod);
                            } else if (usingDefinition is FluentActionUsingParentDefinition)
                            {
                                ilGenerator.Emit(OpCodes.Ldarg_0);
                                ilGenerator.Emit(OpCodes.Ldfld, ParentField);
                            } else if (usingDefinition is FluentActionUsingResultDefinition)
                            {
                                FieldBuilder resultFieldToLoad;
                                if (handlerInStateIndex > 0)
                                {
                                    resultFieldToLoad = state.Handlers[handlerInStateIndex - 1].ResultField;
                                } else
                                {
                                    resultFieldToLoad = States[stateIndex - 1].ResultField;
                                }

                                ilGenerator.Emit(OpCodes.Ldarg_0);
                                ilGenerator.Emit(OpCodes.Ldfld, resultFieldToLoad);

                                if (resultFieldToLoad.FieldType.IsValueType)
                                {
                                    ilGenerator.Emit(OpCodes.Box, resultFieldToLoad.FieldType);
                                }
                            } else
                            {
                                throw new Exception($"Got unknown using definition: {usingDefinition.GetType()}");
                            }
                        }

                        EmitDebugLog(ilGenerator, $"State{stateIndex}::Handler::Invoking Delegate");

                        // Push Func.Invoke
                        ilGenerator.Emit(OpCodes.Callvirt, funcType.GetMethod("Invoke"));

                        if (handler.Async)
                        {
                            EmitDebugLog(ilGenerator, $"State{stateIndex}::Handler::Result in local variable");

                            // Push result in local variable
                            ilGenerator.Emit(OpCodes.Stloc, localVariableForResult);

                            EmitDebugLog(ilGenerator, $"State{stateIndex}::Handler::State.Awaiter = result.GetAwaiter();");

                            // State.Awaiter = result.GetAwaiter();
                            ilGenerator.Emit(OpCodes.Ldarg_0);
                            ilGenerator.Emit(OpCodes.Ldloc, localVariableForResult);
                            ilGenerator.Emit(OpCodes.Call, resultType.GetMethod("GetAwaiter"));
                            ilGenerator.Emit(OpCodes.Stfld, state.TaskAwaiterField);

                            EmitDebugLog(ilGenerator, $"State{stateIndex}::Handler::State.Waiting = true;");

                            // State.Waiting = true;
                            ilGenerator.Emit(OpCodes.Ldarg_0);
                            ilGenerator.Emit(OpCodes.Ldc_I4_1);
                            ilGenerator.Emit(OpCodes.Stfld, state.WaitingField);

                            EmitDebugLog(ilGenerator, $"State{stateIndex}::Handler::If [result.IsCompleted]: Goto finish;");
                            // If [result.IsCompleted]: Goto finish
                            ilGenerator.Emit(OpCodes.Ldloca, localVariableForResult);
                            ilGenerator.Emit(OpCodes.Call, resultType.GetProperty("IsCompleted").GetGetMethod());
                            ilGenerator.Emit(OpCodes.Brtrue, state.FinishLabel);

                            // Else: AwaitUnsafeOnCompleted(ref Awaiter, ref self)

                            EmitDebugLog(ilGenerator, $"S☺tate{stateIndex}::Handler::Else: AwaitUnsafeOnCompleted(ref Awaiter, ref self)");

                            // Store executing instance ("this") in a local variable
                            ilGenerator.Emit(OpCodes.Ldarg_0);
                            ilGenerator.Emit(OpCodes.Stloc, localVariableForThis);

                            ilGenerator.Emit(OpCodes.Ldarg_0);
                            ilGenerator.Emit(OpCodes.Ldflda, AsyncTaskMethodBuilderField);
                            ilGenerator.Emit(OpCodes.Ldarg_0);
                            ilGenerator.Emit(OpCodes.Ldflda, state.TaskAwaiterField);
                            ilGenerator.Emit(OpCodes.Ldloca, localVariableForThis);
                            ilGenerator.Emit(OpCodes.Call, asyncTaskMethodBuilderType
                                .GetMethod("AwaitUnsafeOnCompleted")
                                .MakeGenericMethod(state.TaskAwaiterType, Type));

                            EmitDebugLog(ilGenerator, $"State{stateIndex}::Handler::Called AwaitUnsafeOnCompleted.");
                        } 
                        else if (handlerInStateIndex == state.Handlers.Length - 1)
                        {
                            ilGenerator.Emit(OpCodes.Br, state.FinishLabel);
                        } 
                    } 
                    else if (handler.Type == FluentActionHandlerType.View
                        || handler.Type == FluentActionHandlerType.PartialView
                        || handler.Type == FluentActionHandlerType.ViewComponent)
                    {
                        EmitDebugLog(ilGenerator, $"State{stateIndex}::Handler::Beginning View call");

                        if (handler.ViewTarget == null)
                        {
                            throw new Exception("Must specify a view target.");
                        }

                        // Call one of the following controller methods:
                        //   Controller.View(string pathName, object model)
                        //   Controller.PartialView(string pathName, object model)
                        //   Controller.ViewComponent(string componentName, object arguments)

                        ilGenerator.Emit(OpCodes.Ldarg_0);
                        ilGenerator.Emit(OpCodes.Ldarg_0);
                        ilGenerator.Emit(OpCodes.Ldfld, ParentField);
                        ilGenerator.Emit(OpCodes.Ldstr, handler.ViewTarget);

                        Type[] viewMethodParameterTypes = null;
                        if (stateIndex > 0 && States[stateIndex - 1].ResultField != null)
                        {
                            ilGenerator.Emit(OpCodes.Ldarg_0);
                            ilGenerator.Emit(OpCodes.Ldfld, States[stateIndex - 1].ResultField);

                            viewMethodParameterTypes = new[] { typeof(string), typeof(object) };
                        } else
                        {
                            viewMethodParameterTypes = new[] { typeof(string) };
                        }

                        MethodInfo viewMethod = null;
                        if (handler.Type == FluentActionHandlerType.View)
                        {
                            viewMethod = typeof(Controller).GetMethod("View", viewMethodParameterTypes);
                        } else if (handler.Type == FluentActionHandlerType.PartialView)
                        {
                            viewMethod = typeof(Controller).GetMethod("PartialView", viewMethodParameterTypes);
                        } else if (handler.Type == FluentActionHandlerType.ViewComponent)
                        {
                            viewMethod = typeof(Controller).GetMethod("ViewComponent", viewMethodParameterTypes);
                        }

                        ilGenerator.Emit(OpCodes.Callvirt, viewMethod);

                        if (handlerInStateIndex == state.Handlers.Length - 1)
                        {
                            ilGenerator.Emit(OpCodes.Stfld, state.ResultField);
                            ilGenerator.Emit(OpCodes.Br, state.FinishLabel);
                        }
                        else
                        {
                            ilGenerator.Emit(OpCodes.Stfld, handlerInState.ResultField);
                        }
                    }
                }

                EmitDebugLog(ilGenerator, $"State{stateIndex}::Leaving exception-block.");

                ilGenerator.Emit(OpCodes.Leave, exceptionBlock);

                if (state.Async)
                {
                    // ====== Wait-block ====== (Check Awaiter if it is done and then save result)
                    ilGenerator.MarkLabel(state.WaitLabel);

                    EmitDebugLog(ilGenerator, $"State{stateIndex}::Wait-block");

                    // If [!State.Awaiter.IsComplete]: leave
                    ilGenerator.Emit(OpCodes.Ldarg_0);
                    ilGenerator.Emit(OpCodes.Ldflda, state.TaskAwaiterField);
                    ilGenerator.Emit(OpCodes.Call, taskAwaiterIsCompletedGetMethod);
                    ilGenerator.Emit(OpCodes.Brfalse, leaveLabel);
                }

                // ====== Finish-block ====== (Save result, update state and, potentially, set final result)
                ilGenerator.MarkLabel(state.FinishLabel);

                EmitDebugLog(ilGenerator, $"State{stateIndex}::Finish-block");

                if (state.Async && state.ResultType != null)
                {
                    // State.Result = State.Awaiter.GetResult()
                    ilGenerator.Emit(OpCodes.Ldarg_0);
                    ilGenerator.Emit(OpCodes.Ldarg_0);
                    ilGenerator.Emit(OpCodes.Ldflda, state.TaskAwaiterField);
                    ilGenerator.Emit(OpCodes.Call, taskAwaiterGetResultMethod);
                    ilGenerator.Emit(OpCodes.Stfld, state.ResultField);
                }

                // State + 1
                ilGenerator.Emit(OpCodes.Ldarg_0);
                ilGenerator.Emit(OpCodes.Ldc_I4, stateIndex + 1);
                ilGenerator.Emit(OpCodes.Stfld, StateField);

                if (stateIndex == lastStateIndex)
                {
                    // MethodSetResult(StateResult)
                    ilGenerator.Emit(OpCodes.Ldarg_0);
                    ilGenerator.Emit(OpCodes.Ldflda, AsyncTaskMethodBuilderField);
                    ilGenerator.Emit(OpCodes.Ldarg_0);
                    ilGenerator.Emit(OpCodes.Ldfld, state.ResultField);
                    ilGenerator.Emit(OpCodes.Call, asyncTaskMethodBuilderType.GetMethod("SetResult"));
                } else
                {
                    ilGenerator.Emit(OpCodes.Br, statesStartLabels[stateIndex + 1]);
                }

                ilGenerator.Emit(OpCodes.Leave, exceptionBlock);

                stateIndex++;
            }

            ilGenerator.BeginCatchBlock(typeof(Exception));

            EmitDebugLog(ilGenerator, $"Catch-block.");

            // Store exception locally
            ilGenerator.Emit(OpCodes.Stloc, exceptionLocalVariable);

            // State = -1 
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldc_I4_M1);
            ilGenerator.Emit(OpCodes.Stfld, StateField);

            // StateMachine.SetException(exception)
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldflda, AsyncTaskMethodBuilderField);
            ilGenerator.Emit(OpCodes.Ldloc, exceptionLocalVariable);
            ilGenerator.Emit(OpCodes.Call, asyncTaskMethodBuilderType.GetMethod("SetException"));

            EmitDebugLog(ilGenerator, $"Got exception.");

            ilGenerator.Emit(OpCodes.Leave, exceptionBlock);

            EmitDebugLog(ilGenerator, $"Ending exception block...");

            ilGenerator.EndExceptionBlock();

            EmitDebugLog(ilGenerator, $"Ended exception block.");
            EmitDebugLog(ilGenerator, $"Returning...");

            ilGenerator.Emit(OpCodes.Ret);
        }

        private void DefineSetStateMachineMethod()
        {
            var setStateMachineMethodBuilder = Type.DefineMethod("SetStateMachine", MethodAttributes.Public | MethodAttributes.Virtual);
            setStateMachineMethodBuilder.SetParameters(new[] { typeof(IAsyncStateMachine) });
            setStateMachineMethodBuilder.GetILGenerator().Emit(OpCodes.Ret);
            Type.DefineMethodOverride(setStateMachineMethodBuilder, typeof(IAsyncStateMachine).GetMethod("SetStateMachine"));
        }

        private void EmitDebugLog(ILGenerator ilGenerator, string message)
        {
            if (LoggerKey != null)
            {
                FluentActionLoggers.PushDebugLogOntoStack(ilGenerator, LoggerKey, $"[MoveNext] {message}");
            }
        }
    }

    public class StateMachineState
    {
        public bool Async => Handlers?.LastOrDefault()?.Definition.Async ?? false;

        public Type ResultType { get; set; }

        public Label StartLabel { get; set; }

        public Label WaitLabel { get; set; }

        public Label FinishLabel { get; set; }

        public FieldBuilder ResultField { get; set; }

        public Type TaskAwaiterType { get; set; }

        public FieldBuilder TaskAwaiterField { get; set; }

        public FieldBuilder WaitingField { get; set; }

        public StateMachineStateHandler[] Handlers { get; set; }

    }

    public class StateMachineStateHandler
    {
        public FluentActionHandlerDefinition Definition { get; set; }

        public FieldBuilder DelegateField { get; set; }

        public FieldBuilder ResultField { get; set; }
    }
}
