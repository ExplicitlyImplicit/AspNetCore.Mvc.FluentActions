
using System.Collections.Generic;

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentEndpoints
{
    public class EndpointControllerFeatureProviderContext
    {
        public IEnumerable<EndpointControllerDefinition> ControllerDefinitions { get; set; }
    }
}
