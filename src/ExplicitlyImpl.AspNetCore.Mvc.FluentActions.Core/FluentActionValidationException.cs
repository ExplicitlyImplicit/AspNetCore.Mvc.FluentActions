// Licensed under the MIT License. See LICENSE file in the root of the solution for license information.

using System;

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentActions
{
    public class FluentActionValidationException : Exception
    {
        public FluentActionValidationException() : base() { }
        public FluentActionValidationException(string message) : base(message) { }
    }
}

