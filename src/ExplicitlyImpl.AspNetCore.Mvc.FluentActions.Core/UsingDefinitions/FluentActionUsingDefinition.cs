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

        public virtual bool IsMethodParameter => false;

        public virtual string MethodParameterName => null;

        public virtual bool IsControllerProperty => false;

        public virtual string ControllerPropertyName => null;

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

        public override int GetHashCode()
        {
            return Tuple.Create(GetType(), Type).GetHashCode();
        }
    }
}
