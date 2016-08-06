
// ReSharper disable InconsistentNaming

using Microsoft.AspNetCore.Mvc;

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentActions
{
    public class FluentActionWithView : FluentActionBase
    {
        public FluentActionWithView(FluentActionDefinition fluentActionDefinition, string pathToView) 
            : base(fluentActionDefinition)
        {
            Definition.CurrentHandler.Type = FluentActionHandlerType.View;
            Definition.CurrentHandler.PathToView = pathToView;
            Definition.CurrentHandler.ReturnType = typeof(ViewResult);
        }
    }
}
