// Licensed under the MIT License. See LICENSE file in the root of the solution for license information.

using System;
using System.Reflection;
using System.Reflection.Emit;

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentActions
{
    public abstract class FluentActionUsingDefinition
    {
        public Type Type { get; set; }

        public bool HasDefaultValue { get; set; }

        public object DefaultValue { get; set; }

        public abstract bool IsMethodParameter { get; }

        public virtual string MethodParameterName { get; }

        public override bool Equals(object other)
        {
            return other is FluentActionUsingDefinition && other.GetHashCode() == GetHashCode();
        }

        public virtual ParameterBuilder DefineMethodParameter(MethodBuilder methodBuilder,
            FluentActionDefinition actionDefinition, 
            FluentActionUsingDefinition usingDefinition, 
            int parameterIndex)
        {
            var parameterBuilder = methodBuilder.DefineParameter(
                parameterIndex,
                usingDefinition.HasDefaultValue ? ParameterAttributes.HasDefault : ParameterAttributes.None,
                usingDefinition.MethodParameterName ?? $"parameter{parameterIndex}");

            if (usingDefinition.HasDefaultValue)
            {
                parameterBuilder.SetConstant(usingDefinition.DefaultValue);
            }

            return parameterBuilder;
        }

        public virtual void GenerateIl(
            IlHandle ilHandle, 
            FluentActionUsingDefinition usingDefinition,
            int methodParameterIndex, 
            LocalBuilder localVariableForPreviousReturnValue)
        {
            if (usingDefinition.IsMethodParameter)
            {
                ilHandle.Generator.Emit(OpCodes.Ldarg, methodParameterIndex);
            }
        }

        public override int GetHashCode()
        {
            return Tuple.Create(GetType(), Type).GetHashCode();
        }
    }
}
