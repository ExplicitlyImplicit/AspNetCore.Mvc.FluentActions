using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Collections.Generic;

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentEndpoints
{
    public class EndpointControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        public EndpointControllerFeatureProviderContext Context { get; set; }

        public EndpointControllerFeatureProvider(EndpointControllerFeatureProviderContext context)
        {
            Context = context;
        }

        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            foreach (var controllerDefinition in Context.ControllerDefinitions)
            {
                feature.Controllers.Add(controllerDefinition.TypeInfo);
            }
        }
    }

    public class EndpointControllerFeatureProviderContext
    {
        public IEnumerable<EndpointControllerDefinition> ControllerDefinitions { get; set; }
    }
}
