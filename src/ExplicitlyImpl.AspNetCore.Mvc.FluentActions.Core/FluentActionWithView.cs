
// ReSharper disable InconsistentNaming

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentActions
{
    public class FluentActionWithView : FluentActionBase
    {
        public FluentActionWithView(FluentActionDefinition fluentActionDefinition, string pathToView) : base(fluentActionDefinition)
        {
            Definition.PathToView = pathToView;
        }
    }
}
