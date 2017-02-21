// Licensed under the MIT License. See LICENSE file in the root of the solution for license information.

using Microsoft.AspNetCore.Mvc;

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentActions
{
    public class FluentActionWithView : FluentActionBase
    {
        public FluentActionWithView(FluentActionDefinition fluentActionDefinition, string pathToView) 
            : base(fluentActionDefinition)
        {
            Definition.ExistingOrNewHandlerDraft.Type = FluentActionHandlerType.View;
            Definition.ExistingOrNewHandlerDraft.ViewTarget = pathToView;
            Definition.ExistingOrNewHandlerDraft.ReturnType = typeof(ViewResult);
            Definition.CommitHandlerDraft();
        }
    }

    public class FluentActionWithPartialView : FluentActionBase
    {
        public FluentActionWithPartialView(FluentActionDefinition fluentActionDefinition, string pathToView)
            : base(fluentActionDefinition)
        {
            Definition.ExistingOrNewHandlerDraft.Type = FluentActionHandlerType.PartialView;
            Definition.ExistingOrNewHandlerDraft.ViewTarget = pathToView;
            Definition.ExistingOrNewHandlerDraft.ReturnType = typeof(PartialViewResult);
            Definition.CommitHandlerDraft();
        }
    }

    public class FluentActionWithViewComponent : FluentActionBase
    {
        public FluentActionWithViewComponent(FluentActionDefinition fluentActionDefinition, string viewComponentName)
            : base(fluentActionDefinition)
        {
            Definition.ExistingOrNewHandlerDraft.Type = FluentActionHandlerType.ViewComponent;
            Definition.ExistingOrNewHandlerDraft.ViewTarget = viewComponentName;
            Definition.ExistingOrNewHandlerDraft.ReturnType = typeof(ViewComponentResult);
            Definition.CommitHandlerDraft();
        }
    }
}
