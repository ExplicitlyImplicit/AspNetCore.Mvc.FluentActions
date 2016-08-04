using System;

// ReSharper disable InconsistentNaming

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentActions
{
    public class EndpointWithView : EndpointBase
    {
        public EndpointWithView(EndpointDefinition endpointDefinition, string pathToView) : base(endpointDefinition)
        {
            EndpointDefinition.PathToView = pathToView;
        }
    }
}
