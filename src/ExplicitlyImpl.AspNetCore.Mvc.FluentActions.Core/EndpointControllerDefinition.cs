using System.Reflection;

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentActions
{
    public class EndpointControllerDefinition
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ActionName { get; set; }

        public TypeInfo TypeInfo { get; set; }

        public string Url => Endpoint.Url;

        public FluentActionBase Endpoint { get; set; }
    }
}
