// Licensed under the MIT License. See LICENSE file in the root of the solution for license information.

using Microsoft.AspNetCore.Mvc;
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

        public override int GetHashCode()
        {
            return Tuple.Create(GetType(), Type).GetHashCode();
        }
    }

    public class FluentActionUsingServiceDefinition : FluentActionUsingDefinition
    {
        public override bool IsMethodParameter => true;

        public override ParameterBuilder DefineMethodParameter(
            MethodBuilder methodBuilder,
            FluentActionDefinition actionDefinition,
            FluentActionUsingDefinition usingDefinition, 
            int parameterIndex)
        {
            var parameterBuilder = base.DefineMethodParameter(methodBuilder, actionDefinition, usingDefinition, parameterIndex);

            var attributeBuilder = new CustomAttributeBuilder(typeof(FromServicesAttribute)
                .GetConstructor(new Type[0]), new Type[0]);

            parameterBuilder.SetCustomAttribute(attributeBuilder);

            return parameterBuilder;
        }
    }

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

    public class FluentActionUsingQueryStringParameterDefinition : FluentActionUsingDefinition
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

            var attributeType = typeof(FromQueryAttribute);
            var name = ((FluentActionUsingQueryStringParameterDefinition)usingDefinition).Name;

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

    public class FluentActionUsingHeaderParameterDefinition : FluentActionUsingDefinition
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

            var attributeType = typeof(FromHeaderAttribute);
            var name = ((FluentActionUsingHeaderParameterDefinition)usingDefinition).Name;

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

    public class FluentActionUsingBodyDefinition : FluentActionUsingDefinition
    {
        public override bool IsMethodParameter => true;

        public override ParameterBuilder DefineMethodParameter(
            MethodBuilder methodBuilder,
            FluentActionDefinition actionDefinition,
            FluentActionUsingDefinition usingDefinition,
            int parameterIndex)
        {
            var parameterBuilder = base.DefineMethodParameter(methodBuilder, actionDefinition, usingDefinition, parameterIndex);

            var attributeBuilder = new CustomAttributeBuilder(typeof(FromBodyAttribute)
                .GetConstructor(new Type[0]), new Type[0]);
            parameterBuilder.SetCustomAttribute(attributeBuilder);

            return parameterBuilder;
        }
    }

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
    
    public class FluentActionUsingFormFileDefinition : FluentActionUsingDefinition
    {
        public string Name { get; set; }

        public override bool IsMethodParameter => true;

        public override string MethodParameterName => Name;

        public override int GetHashCode()
        {
            return Tuple.Create(GetType(), Type, Name.ToLowerInvariant()).GetHashCode();
        }
    }

    public class FluentActionUsingFormFilesDefinition : FluentActionUsingFormFileDefinition
    {
    }

    public class FluentActionUsingFormValueDefinition : FluentActionUsingDefinition
    {
        public string Key { get; set; }

        public override bool IsMethodParameter => true;

        public override ParameterBuilder DefineMethodParameter(
            MethodBuilder methodBuilder,
            FluentActionDefinition actionDefinition,
            FluentActionUsingDefinition usingDefinition,
            int parameterIndex)
        {
            var parameterBuilder = base.DefineMethodParameter(methodBuilder, actionDefinition, usingDefinition, parameterIndex);

            var attributeType = typeof(FromFormAttribute);
            var key = ((FluentActionUsingFormValueDefinition)usingDefinition).Key;

            var attributeBuilder = new CustomAttributeBuilder(
                attributeType.GetConstructor(new Type[0]),
                new Type[0],
                new[] { attributeType.GetProperty("Name") },
                new object[] { key });

            parameterBuilder.SetCustomAttribute(attributeBuilder);

            return parameterBuilder;
        }

        public override int GetHashCode()
        {
            return Tuple.Create(GetType(), Type, Key.ToLowerInvariant()).GetHashCode();
        }
    }

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

    public class FluentActionUsingResultFromHandlerDefinition : FluentActionUsingDefinition
    {
        public override bool IsMethodParameter => false;
    }
    
    public class FluentActionUsingModelStateDefinition : FluentActionUsingDefinition
    {
        public override bool IsMethodParameter => false;
    }

    public class FluentActionUsingHttpContextDefinition : FluentActionUsingDefinition
    {
        public override bool IsMethodParameter => false;
    }

    public class FluentActionUsingMvcControllerDefinition : FluentActionUsingDefinition
    {
        public override bool IsMethodParameter => false;
    }

    public class FluentActionUsingViewBagDefinition : FluentActionUsingDefinition
    {
        public override bool IsMethodParameter => false;
    }

    public class FluentActionUsingViewDataDefinition : FluentActionUsingDefinition
    {
        public override bool IsMethodParameter => false;
    }

    public class FluentActionUsingTempDataDefinition : FluentActionUsingDefinition
    {
        public override bool IsMethodParameter => false;
    }
}
