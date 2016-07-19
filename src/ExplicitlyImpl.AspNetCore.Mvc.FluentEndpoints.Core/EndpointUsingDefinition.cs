using System;

// ReSharper disable InconsistentNaming

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentEndpoints
{
    public class EndpointUsingDefinition
    {
        public Type Type { get; set; }
    }

    public class EndpointUsingServiceDefinition : EndpointUsingDefinition
    {
    }

    public class EndpointUsingRouteParameterDefinition : EndpointUsingDefinition
    {
        public string Name { get; set; }
    }

    public class EndpointUsingQueryStringParameterDefinition : EndpointUsingDefinition
    {
        public string Name { get; set; }
    }

    public class EndpointUsingHeaderParameterDefinition : EndpointUsingDefinition
    {
        public string Name { get; set; }
    }

    public class EndpointUsingBodyDefinition : EndpointUsingDefinition
    {
    }

    public class EndpointUsingFormDefinition : EndpointUsingDefinition
    {
    }

    public class EndpointUsingFormValueDefinition : EndpointUsingDefinition
    {
        public string Key { get; set; }
    }

    public class EndpointUsingModelBinderDefinition : EndpointUsingDefinition
    {
        public Type ModelBinderType { get; set; }
    }
}
