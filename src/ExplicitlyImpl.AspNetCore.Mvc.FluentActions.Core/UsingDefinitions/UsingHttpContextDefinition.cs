// Licensed under the MIT License. See LICENSE file in the root of the solution for license information.

using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Reflection.Emit;

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentActions
{
    public class FluentActionUsingHttpContextDefinition : FluentActionUsingDefinition
    {
        public override bool IsMethodParameter => false;

        public override void GenerateIl(
            IlHandle ilHandle,
            FluentActionUsingDefinition usingDefinition,
            int methodParameterIndex,
            LocalBuilder localVariableForPreviousReturnValue)
        {
            ilHandle.Generator.Emit(OpCodes.Ldarg_0);
            ilHandle.Generator.Emit(OpCodes.Callvirt, 
                typeof(Controller).GetProperty("HttpContext").GetGetMethod());
        }
    }
}
