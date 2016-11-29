// Licensed under the MIT License. See LICENSE file in the root of the solution for license information.

using System;

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentActions
{
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
}
