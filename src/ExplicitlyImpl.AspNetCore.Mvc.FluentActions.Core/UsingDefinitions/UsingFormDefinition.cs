// Licensed under the MIT License. See LICENSE file in the root of the solution for license information.

using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection;
using System.Reflection.Emit;

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentActions
{
    public class FluentActionUsingFormDefinition : FluentActionUsingDefinition
    {
        public override bool IsMethodParameter => true;

        public override ParameterBuilder DefineMethodParameter(
            MethodBuilder methodBuilder,
            FluentActionDefinition actionDefinition,
            FluentActionUsingDefinition usingDefinition,
            int parameterIndex)
        {
            var parameterBuilder = base.DefineMethodParameter(methodBuilder, actionDefinition, usingDefinition, parameterIndex);

            var attributeBuilder = new CustomAttributeBuilder(typeof(FromFormAttribute)
                .GetConstructor(new Type[0]), new Type[0]);
            parameterBuilder.SetCustomAttribute(attributeBuilder);

            return parameterBuilder;
        }
    }
}
