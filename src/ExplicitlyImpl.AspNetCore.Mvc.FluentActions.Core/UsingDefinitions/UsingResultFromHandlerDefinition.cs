// Licensed under the MIT License. See LICENSE file in the root of the solution for license information.

using System;
using System.Reflection.Emit;

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentActions
{
    public class FluentActionUsingResultFromHandlerDefinition : FluentActionUsingDefinition
    {
        public override bool IsMethodParameter => false;

        public override void GenerateIl(
            IlHandle ilHandle,
            FluentActionUsingDefinition usingDefinition,
            int methodParameterIndex,
            LocalBuilder localVariableForPreviousReturnValue)
        {
            if (localVariableForPreviousReturnValue == null)
            {
                throw new Exception("Cannot use previous result from handler as no previous result exists.");
            }

            ilHandle.Generator.Emit(OpCodes.Ldloc, localVariableForPreviousReturnValue);
        }
    }
}
