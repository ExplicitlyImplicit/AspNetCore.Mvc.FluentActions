using System.Reflection;

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentActions
{
    public class FluentActionControllerDefinition
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ActionName { get; set; }

        public TypeInfo TypeInfo { get; set; }

        public string RouteTemplate => FluentAction.RouteTemplate;

        public FluentActionBase FluentAction { get; set; }
    }
}
