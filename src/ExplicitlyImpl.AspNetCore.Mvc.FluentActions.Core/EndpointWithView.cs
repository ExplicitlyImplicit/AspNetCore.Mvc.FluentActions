using System;

// ReSharper disable InconsistentNaming

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentActions
{
    public class EndpointWithView : FluentActionBase
    {
        public EndpointWithView(FluentActionDefinition endpointDefinition, string pathToView) : base(endpointDefinition)
        {
            Definition.PathToView = pathToView;
        }
    }
}
