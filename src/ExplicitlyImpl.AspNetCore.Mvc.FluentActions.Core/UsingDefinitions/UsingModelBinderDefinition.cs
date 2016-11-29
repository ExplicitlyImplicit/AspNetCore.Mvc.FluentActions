// Licensed under the MIT License. See LICENSE file in the root of the solution for license information.

using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection;
using System.Reflection.Emit;

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentActions
{
    public class FluentActionUsingModelBinderDefinition : FluentActionUsingDefinition
    {
        public Type ModelBinderType { get; set; }

        public string ParameterName { get; set; }

        public override bool IsMethodParameter => true;

        public override ParameterBuilder DefineMethodParameter(
            MethodBuilder methodBuilder,
            FluentActionDefinition actionDefinition,
            FluentActionUsingDefinition usingDefinition,
            int parameterIndex)
        {
            var parameterBuilder = base.DefineMethodParameter(methodBuilder, actionDefinition, usingDefinition, parameterIndex);

            var attributeType = typeof(ModelBinderAttribute);
            var modelBinderDefinition = ((FluentActionUsingModelBinderDefinition)usingDefinition);
            var modelBinderType = modelBinderDefinition.ModelBinderType;

            PropertyInfo[] propertyTypes = null;
            object[] propertyValues = null;
            if (!string.IsNullOrWhiteSpace(modelBinderDefinition.ParameterName))
            {
                propertyTypes = new[]
                {
                    attributeType.GetProperty("BinderType"),
                    attributeType.GetProperty("Name")
                };
                propertyValues = new object[] 
                {
                    modelBinderType,
                    modelBinderDefinition.ParameterName
                };
            } 
            else
            {
                propertyTypes = new[]
                {
                    attributeType.GetProperty("BinderType")
                };
                propertyValues = new object[]
                {
                    modelBinderType
                };
            }

            var attributeBuilder = new CustomAttributeBuilder(
                attributeType.GetConstructor(new Type[0]),
                new Type[0],
                propertyTypes,
                propertyValues);

            parameterBuilder.SetCustomAttribute(attributeBuilder);

            return parameterBuilder;
        }

        public override int GetHashCode()
        {
            return Tuple.Create(GetType(), Type, ModelBinderType).GetHashCode();
        }
    }
}
