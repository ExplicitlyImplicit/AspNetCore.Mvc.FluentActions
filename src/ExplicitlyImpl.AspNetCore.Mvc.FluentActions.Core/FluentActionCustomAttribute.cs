// Licensed under the MIT License. See LICENSE file in the root of the solution for license information.

using System;
using System.Reflection;

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentActions
{
    public class FluentActionCustomAttribute
    {
        public Type Type { get; internal set; }
        public ConstructorInfo Constructor { get; internal set; }
        public object[] ConstructorArgs { get; internal set; }
        public PropertyInfo[] NamedProperties { get; internal set; }
        public object[] PropertyValues { get; internal set; }
        public FieldInfo[] NamedFields { get; internal set; }
        public object[] FieldValues { get; internal set; }
    }
}
