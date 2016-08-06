
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

    public class FluentActionWithPartialView : FluentActionBase
    {
        public FluentActionWithPartialView(FluentActionDefinition fluentActionDefinition, string pathToView)
            : base(fluentActionDefinition)
        {
            Definition.CurrentHandler.Type = FluentActionHandlerType.PartialView;
            Definition.CurrentHandler.PathToView = pathToView;
            Definition.CurrentHandler.ReturnType = typeof(PartialViewResult);
        }
    }

    public class FluentActionWithViewComponent : FluentActionBase
    {
        public FluentActionWithViewComponent(FluentActionDefinition fluentActionDefinition, string pathToView)
            : base(fluentActionDefinition)
        {
            Definition.CurrentHandler.Type = FluentActionHandlerType.ViewComponent;
            Definition.CurrentHandler.PathToView = pathToView;
            Definition.CurrentHandler.ReturnType = typeof(ViewComponentResult);
        }
    }
}
