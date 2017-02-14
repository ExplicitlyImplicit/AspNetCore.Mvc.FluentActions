// Licensed under the MIT License. See LICENSE file in the root of the solution for license information.

using System.Reflection.Emit;

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentActions
{
    public class IlHandle
    {
        public ILGenerator Generator { get; internal set; }
    }
}
