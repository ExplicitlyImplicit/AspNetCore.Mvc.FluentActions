using System;

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentActions
{
    public static class StringExtensions
    {
        public static string WithoutTrailing(this string originalValue, 
            string trailingValue, 
            StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase)
        {
            if (originalValue.Length < trailingValue.Length)
            {
                throw new ArgumentException($"Trailing string {trailingValue} is longer than original string {originalValue}");
            }

            if (originalValue.EndsWith(trailingValue, stringComparison))
            {
                return originalValue.Substring(0, originalValue.Length - trailingValue.Length);
            } 
            else
            {
                return originalValue;
            }
        }

        public static string WithoutLeading(this string originalValue,
            string leadingValue,
            StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase)
        {
            if (originalValue.Length < leadingValue.Length)
            {
                throw new ArgumentException($"Leading string {leadingValue} is longer than original string {originalValue}");
            }

            if (originalValue.StartsWith(leadingValue, stringComparison))
            {
                return originalValue.Substring(leadingValue.Length, originalValue.Length - leadingValue.Length);
            } 
            else
            {
                return originalValue;
            }
        }

        public static bool Contains(this string originalValue, string valueToCheck, StringComparison comparison)
        {
            return originalValue.IndexOf(valueToCheck, comparison) >= 0;
        }
    }
}
