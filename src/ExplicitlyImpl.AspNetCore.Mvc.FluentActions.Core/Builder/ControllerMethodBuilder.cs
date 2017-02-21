// Licensed under the MIT License. See LICENSE file in the root of the solution for license information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentActions.Core.Builder
{
    public abstract class ControllerMethodBuilder
    {
        public TypeBuilder TypeBuilder { get; internal set; }
        public MethodBuilder MethodBuilder { get; internal set; }
        public List<TypeBuilder> NestedTypes { get; internal set; }

        public abstract void Build();

        public void SetHttpMethodAttribute(HttpMethod httpMethod)
        {
            var attributeConstructorInfo = GetHttpMethodAttribute(httpMethod)
                .GetConstructor(new Type[0]);
            var attributeBuilder = new CustomAttributeBuilder(attributeConstructorInfo, new Type[0]);
            MethodBuilder.SetCustomAttribute(attributeBuilder);
        }

        public void SetRouteAttribute(string routeTemplate)
        {
            var attributeConstructorInfo = typeof(RouteAttribute)
                .GetConstructor(new Type[] { typeof(string) });
            var attributeBuilder = new CustomAttributeBuilder(attributeConstructorInfo, new[] { routeTemplate });
            MethodBuilder.SetCustomAttribute(attributeBuilder);
        }

        public void SetValidateAntiForgeryTokenAttribute()
        {
            var attributeConstructorInfo = typeof(ValidateAntiForgeryTokenAttribute)
                .GetConstructor(new Type[0]);
            var attributeBuilder = new CustomAttributeBuilder(attributeConstructorInfo, new object[0]);
            MethodBuilder.SetCustomAttribute(attributeBuilder);
        }

        private static Type GetHttpMethodAttribute(HttpMethod httpMethod)
        {
            switch (httpMethod)
            {
                case HttpMethod.Delete: return typeof(HttpDeleteAttribute);
                case HttpMethod.Get: return typeof(HttpGetAttribute);
                case HttpMethod.Head: return typeof(HttpHeadAttribute);
                case HttpMethod.Options: return typeof(HttpOptionsAttribute);
                case HttpMethod.Patch: return typeof(HttpPatchAttribute);
                case HttpMethod.Post: return typeof(HttpPostAttribute);
                case HttpMethod.Put: return typeof(HttpPutAttribute);
            }

            throw new Exception($"Could not get corresponding attribute of {nameof(HttpMethod)} {httpMethod}.");
        }
    }
}
