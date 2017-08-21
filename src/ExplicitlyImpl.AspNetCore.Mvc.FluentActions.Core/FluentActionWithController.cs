// Licensed under the MIT License. See LICENSE file in the root of the solution for license information.

using System;
using System.Linq.Expressions;

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentActions
{
    public class FluentActionWithMvcController<TU1> : FluentActionBase
    {
        public FluentActionWithMvcController(FluentActionDefinition fluentActionDefinition, FluentActionUsingDefinition usingDefinition) : base(fluentActionDefinition)
        {
            Definition.ExistingOrNewHandlerDraft.Usings.Add(usingDefinition);
        }

        public FluentActionWithControllerResult<TR> ToMvcAction<TR>(Expression<Func<TU1, TR>> actionExpression)
        {
            return new FluentActionWithControllerResult<TR>(Definition, actionExpression);
        }
    }

    public class FluentActionWithMvcController<TU1, TU2> : FluentActionBase
    {
        public FluentActionWithMvcController(FluentActionDefinition fluentActionDefinition, FluentActionUsingDefinition usingDefinition) : base(fluentActionDefinition)
        {
            Definition.ExistingOrNewHandlerDraft.Usings.Add(usingDefinition);
        }

        public FluentActionWithControllerResult<TR> ToMvcAction<TR>(Expression<Func<TU1, TU2, TR>> actionExpression)
        {
            return new FluentActionWithControllerResult<TR>(Definition, actionExpression);
        }
    }

    public class FluentActionWithMvcController<TU1, TU2, TU3> : FluentActionBase
    {
        public FluentActionWithMvcController(FluentActionDefinition fluentActionDefinition, FluentActionUsingDefinition usingDefinition) : base(fluentActionDefinition)
        {
            Definition.ExistingOrNewHandlerDraft.Usings.Add(usingDefinition);
        }

        public FluentActionWithControllerResult<TR> ToMvcAction<TR>(Expression<Func<TU1, TU2, TU3, TR>> actionExpression)
        {
            return new FluentActionWithControllerResult<TR>(Definition, actionExpression);
        }
    }

    public class FluentActionWithMvcController<TU1, TU2, TU3, TU4> : FluentActionBase
    {
        public FluentActionWithMvcController(FluentActionDefinition fluentActionDefinition, FluentActionUsingDefinition usingDefinition) : base(fluentActionDefinition)
        {
            Definition.ExistingOrNewHandlerDraft.Usings.Add(usingDefinition);
        }

        public FluentActionWithControllerResult<TR> ToMvcAction<TR>(Expression<Func<TU1, TU2, TU3, TU4, TR>> actionExpression)
        {
            return new FluentActionWithControllerResult<TR>(Definition, actionExpression);
        }
    }

    public class FluentActionWithMvcController<TU1, TU2, TU3, TU4, TU5> : FluentActionBase
    {
        public FluentActionWithMvcController(FluentActionDefinition fluentActionDefinition, FluentActionUsingDefinition usingDefinition) : base(fluentActionDefinition)
        {
            Definition.ExistingOrNewHandlerDraft.Usings.Add(usingDefinition);
        }

        public FluentActionWithControllerResult<TR> ToMvcAction<TR>(Expression<Func<TU1, TU2, TU3, TU4, TU5, TR>> actionExpression)
        {
            return new FluentActionWithControllerResult<TR>(Definition, actionExpression);
        }
    }

    public class FluentActionWithMvcController<TU1, TU2, TU3, TU4, TU5, TU6> : FluentActionBase
    {
        public FluentActionWithMvcController(FluentActionDefinition fluentActionDefinition, FluentActionUsingDefinition usingDefinition) : base(fluentActionDefinition)
        {
            Definition.ExistingOrNewHandlerDraft.Usings.Add(usingDefinition);
        }

        public FluentActionWithControllerResult<TR> ToMvcAction<TR>(Expression<Func<TU1, TU2, TU3, TU4, TU5, TU6, TR>> actionExpression)
        {
            return new FluentActionWithControllerResult<TR>(Definition, actionExpression);
        }
    }

    public class FluentActionWithMvcController<TU1, TU2, TU3, TU4, TU5, TU6, TU7> : FluentActionBase
    {
        public FluentActionWithMvcController(FluentActionDefinition fluentActionDefinition, FluentActionUsingDefinition usingDefinition) : base(fluentActionDefinition)
        {
            Definition.ExistingOrNewHandlerDraft.Usings.Add(usingDefinition);
        }

        public FluentActionWithControllerResult<TR> ToMvcAction<TR>(Expression<Func<TU1, TU2, TU3, TU4, TU5, TU6, TU7, TR>> actionExpression)
        {
            return new FluentActionWithControllerResult<TR>(Definition, actionExpression);
        }
    }

    public class FluentActionWithMvcController<TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> : FluentActionBase
    {
        public FluentActionWithMvcController(FluentActionDefinition fluentActionDefinition, FluentActionUsingDefinition usingDefinition) 
            : base(fluentActionDefinition)
        {
            Definition.ExistingOrNewHandlerDraft.Usings.Add(usingDefinition);
        }

        public FluentActionWithControllerResult<TR> ToMvcAction<TR>(Expression<Func<TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8, TR>> actionExpression)
        {
            return new FluentActionWithControllerResult<TR>(Definition, actionExpression);
        }
    }

    public class FluentActionWithMvcController<TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8, TU9> : FluentActionBase
    {
        public FluentActionWithMvcController(FluentActionDefinition fluentActionDefinition, FluentActionUsingDefinition usingDefinition)
            : base(fluentActionDefinition)
        {
            Definition.ExistingOrNewHandlerDraft.Usings.Add(usingDefinition);
        }

        public FluentActionWithControllerResult<TR> ToMvcAction<TR>(Expression<Func<TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8, TU9, TR>> actionExpression)
        {
            return new FluentActionWithControllerResult<TR>(Definition, actionExpression);
        }
    }

    public class FluentActionWithControllerResult<TR> : FluentActionBase
    {
        public FluentActionWithControllerResult(FluentActionDefinition fluentActionDefinition, LambdaExpression actionExpression)
            : base(fluentActionDefinition)
        {
            var returnType = typeof(TR);

            Definition.ExistingOrNewHandlerDraft.Type = FluentActionHandlerType.Controller;
            Definition.ExistingOrNewHandlerDraft.Expression = actionExpression;
            Definition.ExistingOrNewHandlerDraft.ReturnType = returnType.IsAnonymous() ? typeof(object) : returnType;
            Definition.CommitHandlerDraft();
        }
    }
}
