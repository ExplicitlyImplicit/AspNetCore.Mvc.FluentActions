// Licensed under the MIT License. See LICENSE file in the root of the solution for license information.

using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection;
using System.Reflection.Emit;

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentActions
{
    public class FluentActionUsingRouteParameterDefinition : FluentActionUsingDefinition
    {
        public string Name { get; set; }

        public override bool IsMethodParameter => true;

        public override ParameterBuilder DefineMethodParameter(
            MethodBuilder methodBuilder,
            FluentActionDefinition actionDefinition,
            FluentActionUsingDefinition usingDefinition,
            int parameterIndex)
        {
            var parameterBuilder = base.DefineMethodParameter(methodBuilder, actionDefinition, usingDefinition, parameterIndex);

            var attributeType = typeof(FromRouteAttribute);
            var name = ((FluentActionUsingRouteParameterDefinition)usingDefinition).Name;

            if (!actionDefinition.RouteTemplate.Contains($"{{{name}}}", StringComparison.CurrentCultureIgnoreCase))
            {
                throw new Exception($"Route parameter {name} does not exist in routeTemplate {actionDefinition.RouteTemplate}.");
            }

            var attributeBuilder = new CustomAttributeBuilder(
                attributeType.GetConstructor(new Type[0]),
                new Type[0],
                new[] { attributeType.GetProperty("Name") },
                new object[] { name });

            parameterBuilder.SetCustomAttribute(attributeBuilder);

            return parameterBuilder;
        }

        public override int GetHashCode()
        {
            return Tuple.Create(GetType(), Type, Name.ToLowerInvariant()).GetHashCode();
        }
    }
}
