using System;

// ReSharper disable InconsistentNaming

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentEndpoints
{
    public class EndpointWithView : EndpointBase
    {
        public EndpointWithView(EndpointDefinition endpointDefinition, string pathToView) : base(endpointDefinition)
        {
            EndpointDefinition.PathToView = pathToView;
        }
    }
}
