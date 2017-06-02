// Licensed under the MIT License. See LICENSE file in the root of the solution for license information.

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentActions.Core.Builder
{
    public class ControllerMethodBuilderForFluentAction : ControllerMethodBuilder
    {
        public FluentActionDefinition FluentActionDefinition { get; set; }

        public ControllerMethodBuilderForFluentAction(FluentActionDefinition fluentActionDefinition)
        {
            FluentActionDefinition = fluentActionDefinition;
        }

        public override void Build()
        {
            var usingsForMethodParameters = FluentActionDefinition.Handlers
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

            var methodParameterTypes = usingsForMethodParameters
                .Select(@using => @using.Type)
                .ToArray();

            var returnType = FluentActionDefinition.Handlers.Last().ReturnType;
            if (FluentActionDefinition.IsAsync)
            {
                returnType = typeof(Task<>).MakeGenericType(returnType);
            }

            MethodBuilder.SetReturnType(returnType);
            MethodBuilder.SetParameters(methodParameterTypes);

            SetHttpMethodAttribute(FluentActionDefinition.HttpMethod);
            SetRouteAttribute(FluentActionDefinition.RouteTemplate);

            if (FluentActionDefinition.ValidateAntiForgeryToken)
            {
                SetValidateAntiForgeryTokenAttribute();
            }

            foreach (var usingDefinition in usingsForMethodParameters)
            {
                var methodParameterIndex = methodParameterIndices[usingDefinition.GetHashCode()];

                usingDefinition.DefineMethodParameter(MethodBuilder, FluentActionDefinition, usingDefinition, methodParameterIndex);
            }

            var ilGenerator = MethodBuilder.GetILGenerator();

            LocalBuilder localVariableForPreviousReturnValue = null;

            foreach (var handler in FluentActionDefinition.Handlers)
            {
                if (handler.Type == FluentActionHandlerType.Func)
                {

                    var handlerReturnType = BuilderHelper.GetReturnTypeOrTaskType(handler);
                    var localVariableForReturnValue = ilGenerator.DeclareLocal(handlerReturnType);

                    var funcType = BuilderHelper.GetFuncType(handler);
                    var delegateKey = FluentActionDelegates.Add(handler.Delegate);

                    // Push Delegate
                    ilGenerator.Emit(OpCodes.Ldsfld, FluentActionDelegates.FieldInfo);
                    ilGenerator.Emit(OpCodes.Ldstr, delegateKey);
                    ilGenerator.Emit(OpCodes.Callvirt, FluentActionDelegates.MethodInfo);

                    // Push arguments for Func
                    foreach (var usingDefinition in handler.Usings)
                    {
                        EmitUsingDefinitionValue(ilGenerator, usingDefinition, methodParameterIndices, localVariableForPreviousReturnValue);
                    }

                    // Push Func.Invoke
                    ilGenerator.Emit(OpCodes.Callvirt, funcType.GetMethod("Invoke"));

                    // Push storing result in local variable
                    ilGenerator.Emit(OpCodes.Stloc, localVariableForReturnValue);

                    // Make sure next handler has access to previous handler's return value
                    localVariableForPreviousReturnValue = localVariableForReturnValue;
                } 
                else if (handler.Type == FluentActionHandlerType.Action)
                {
                    var actionType = BuilderHelper.GetActionType(handler);
                    var delegateKey = FluentActionDelegates.Add(handler.Delegate);

                    // Push Delegate
                    ilGenerator.Emit(OpCodes.Ldsfld, FluentActionDelegates.FieldInfo);
                    ilGenerator.Emit(OpCodes.Ldstr, delegateKey);
                    ilGenerator.Emit(OpCodes.Callvirt, FluentActionDelegates.MethodInfo);

                    // Push arguments for Action
                    foreach (var usingDefinition in handler.Usings)
                    {
                        EmitUsingDefinitionValue(ilGenerator, usingDefinition, methodParameterIndices, localVariableForPreviousReturnValue);
                    }

                    // Push Action.Invoke
                    ilGenerator.Emit(OpCodes.Callvirt, actionType.GetMethod("Invoke"));

                    // This handler does not produce a result
                    localVariableForPreviousReturnValue = null;
                } 
                else if (handler.Type == FluentActionHandlerType.View 
                    || handler.Type == FluentActionHandlerType.PartialView
                    || (handler.Type == FluentActionHandlerType.ViewComponent && handler.ViewComponentType == null))
                {
                    if (handler.ViewTarget == null)
                    {
                        throw new Exception("Must specify a view target.");
                    }

                    var localVariableForReturnValue = ilGenerator.DeclareLocal(handler.ReturnType);

                    // Call one of the following controller methods:
                    //   Controller.View(string pathName, object model)
                    //   Controller.PartialView(string pathName, object model)
                    //   Controller.ViewComponent(string componentName, object arguments)

                    ilGenerator.Emit(OpCodes.Ldarg_0);
                    ilGenerator.Emit(OpCodes.Ldstr, handler.ViewTarget);

                    Type[] viewMethodParameterTypes = null;
                    if (localVariableForPreviousReturnValue != null)
                    {
                        ilGenerator.Emit(OpCodes.Ldloc, localVariableForPreviousReturnValue);
                        viewMethodParameterTypes = new[] { typeof(string), typeof(object) };
                    } 
                    else
                    {
                        viewMethodParameterTypes = new[] { typeof(string) };
                    }

                    MethodInfo viewMethod = null;
                    if (handler.Type == FluentActionHandlerType.View)
                    {
                        viewMethod = typeof(Controller).GetMethod("View", viewMethodParameterTypes);
                    }
                    else if (handler.Type == FluentActionHandlerType.PartialView)
                    {
                        viewMethod = typeof(Controller).GetMethod("PartialView", viewMethodParameterTypes);
                    }
                    else if (handler.Type == FluentActionHandlerType.ViewComponent)
                    {
                        viewMethod = typeof(Controller).GetMethod("ViewComponent", viewMethodParameterTypes);
                    }

                    ilGenerator.Emit(OpCodes.Callvirt, viewMethod);

                    // Push storing result in local variable
                    ilGenerator.Emit(OpCodes.Stloc, localVariableForReturnValue);

                    // Make sure next handler has access to previous handler's return value
                    localVariableForPreviousReturnValue = localVariableForReturnValue;
                } else if (handler.Type == FluentActionHandlerType.ViewComponent)
                {
                    if (handler.ViewComponentType == null)
                    {
                        throw new Exception("Must specify a target view component type.");
                    }

                    var localVariableForReturnValue = ilGenerator.DeclareLocal(handler.ReturnType);

                    // Call the following controller method:
                    //   Controller.ViewComponent(Type componentType, object arguments)

                    ilGenerator.Emit(OpCodes.Ldarg_0);

                    ilGenerator.Emit(OpCodes.Ldtoken, handler.ViewComponentType);
                    ilGenerator.Emit(OpCodes.Call, typeof(Type).GetMethod("GetTypeFromHandle", new[] { typeof(RuntimeTypeHandle) }));

                    Type[] viewMethodParameterTypes = null;
                    if (localVariableForPreviousReturnValue != null)
                    {
                        ilGenerator.Emit(OpCodes.Ldloc, localVariableForPreviousReturnValue);
                        viewMethodParameterTypes = new[] { typeof(Type), typeof(object) };
                    } 
                    else
                    {
                        viewMethodParameterTypes = new[] { typeof(Type) };
                    }

                    var viewMethod = typeof(Controller).GetMethod("ViewComponent", viewMethodParameterTypes);

                    ilGenerator.Emit(OpCodes.Callvirt, viewMethod);

                    // Push storing result in local variable
                    ilGenerator.Emit(OpCodes.Stloc, localVariableForReturnValue);

                    // Make sure next handler has access to previous handler's return value
                    localVariableForPreviousReturnValue = localVariableForReturnValue;
                }
            }
             
            // Return last handlers return value
            ilGenerator.Emit(OpCodes.Ldloc, localVariableForPreviousReturnValue);
            ilGenerator.Emit(OpCodes.Ret);
        }

        private void EmitUsingDefinitionValue(
            ILGenerator ilGenerator, 
            FluentActionUsingDefinition usingDefinition, 
            Dictionary<int, int> methodParameterIndices, 
            LocalBuilder localVariableForPreviousReturnValue)
        {
            var ilHandle = new IlHandle { Generator = ilGenerator };

            var usingDefinitionHash = usingDefinition.GetHashCode();
            var methodParameterIndex = methodParameterIndices.ContainsKey(usingDefinitionHash) ?
                methodParameterIndices[usingDefinitionHash] : -1;

            if (usingDefinition.IsMethodParameter)
            {
                ilHandle.Generator.Emit(OpCodes.Ldarg, methodParameterIndex);
            } else if (usingDefinition.IsControllerProperty)
            {
                ilHandle.Generator.Emit(OpCodes.Ldarg_0);
                ilHandle.Generator.Emit(OpCodes.Callvirt,
                    typeof(Controller).GetProperty(usingDefinition.ControllerPropertyName).GetGetMethod());
            } else if (usingDefinition is FluentActionUsingResultFromHandlerDefinition)
            {
                if (localVariableForPreviousReturnValue == null)
                {
                    throw new Exception("Cannot use previous result from handler as no previous result exists.");
                }

                ilHandle.Generator.Emit(OpCodes.Ldloc, localVariableForPreviousReturnValue);
            } else
            {
                throw new Exception($"Got unknown using definition: {usingDefinition.GetType()}");
            }
        }
    }
}

