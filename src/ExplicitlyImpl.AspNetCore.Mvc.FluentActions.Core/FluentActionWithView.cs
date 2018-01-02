// Licensed under the MIT License. See LICENSE file in the root of the solution for license information.

using Microsoft.AspNetCore.Mvc;
using System;

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentActions
{
    public class FluentActionWithView<TP> : FluentAction<TP, ViewResult>
    {
        public FluentActionWithView(FluentActionDefinition fluentActionDefinition, string pathToView) 
            : base(fluentActionDefinition)
        {
            Definition.ExistingOrNewHandlerDraft.Type = FluentActionHandlerType.View;
            Definition.ExistingOrNewHandlerDraft.ViewTarget = pathToView;
            Definition.ExistingOrNewHandlerDraft.ReturnType = typeof(ViewResult);
            Definition.ExistingOrNewHandlerDraft.Async = false;
            Definition.CommitHandlerDraft();
        }
    }

    public class FluentActionWithPartialView<TP> : FluentAction<TP, ViewResult>
    {
        public FluentActionWithPartialView(FluentActionDefinition fluentActionDefinition, string pathToView)
            : base(fluentActionDefinition)
        {
            Definition.ExistingOrNewHandlerDraft.Type = FluentActionHandlerType.PartialView;
            Definition.ExistingOrNewHandlerDraft.ViewTarget = pathToView;
            Definition.ExistingOrNewHandlerDraft.ReturnType = typeof(PartialViewResult);
            Definition.ExistingOrNewHandlerDraft.Async = false;
            Definition.CommitHandlerDraft();
        }
    }

    public class FluentActionWithViewComponent<TP> : FluentAction<TP, ViewResult>
    {
        public FluentActionWithViewComponent(FluentActionDefinition fluentActionDefinition, string viewComponentName)
            : base(fluentActionDefinition)
        {
            Definition.ExistingOrNewHandlerDraft.Type = FluentActionHandlerType.ViewComponent;
            Definition.ExistingOrNewHandlerDraft.ViewTarget = viewComponentName;
            Definition.ExistingOrNewHandlerDraft.ReturnType = typeof(ViewComponentResult);
            Definition.ExistingOrNewHandlerDraft.Async = false;
            Definition.CommitHandlerDraft();
        }

        public FluentActionWithViewComponent(FluentActionDefinition fluentActionDefinition, Type viewComponentType)
            : base(fluentActionDefinition)
        {
            Definition.ExistingOrNewHandlerDraft.Type = FluentActionHandlerType.ViewComponent;
            Definition.ExistingOrNewHandlerDraft.ViewComponentType = viewComponentType;
            Definition.ExistingOrNewHandlerDraft.ReturnType = typeof(ViewComponentResult);
            Definition.ExistingOrNewHandlerDraft.Async = false;
            Definition.CommitHandlerDraft();
        }
    }
}
