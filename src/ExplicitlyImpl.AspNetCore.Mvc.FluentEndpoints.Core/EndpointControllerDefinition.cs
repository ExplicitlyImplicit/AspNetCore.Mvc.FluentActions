using System.Reflection;

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentEndpoints
{
    public class EndpointControllerDefinition
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ActionName { get; set; }

        public TypeInfo TypeInfo { get; set; }

        public string Url => Endpoint.Url;

        public EndpointBase Endpoint { get; set; }
    }
}
