using System;

// ReSharper disable InconsistentNaming

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentEndpoints
{
    public class EndpointWithResult<TR> : Endpoint
    {
        public EndpointWithResult(EndpointDefinition endpointDefinition, Delegate handlerFunc) : base(endpointDefinition)
        {
            EndpointDefinition.CurrentHandler.Delegate = handlerFunc;
            EndpointDefinition.CurrentHandler.ReturnType = typeof(TR);
        }
    }
}
