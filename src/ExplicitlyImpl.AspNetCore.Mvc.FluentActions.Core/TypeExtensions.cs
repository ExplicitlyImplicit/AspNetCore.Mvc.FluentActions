using System;

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentActions
{
    public static class TypeExtensions
    {
        public static bool IsAnonymous(this Type type)
        {
            // This is not so great
            return type.Namespace == null;
        }
    }
}
