using System;
using System.Linq.Expressions;

// ReSharper disable InconsistentNaming

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentActions
{
    public class FluentActionWithController<TU1> : FluentActionBase
    {
        public FluentActionWithController(FluentActionDefinition fluentActionDefinition, FluentActionUsingDefinition usingDefinition) : base(fluentActionDefinition)
        {
            Definition.CurrentHandler.Usings.Add(usingDefinition);
        }

        public FluentActionWithControllerResult<TR> ToAction<TR>(Expression<Func<TU1, TR>> actionExpression)
        {
            return new FluentActionWithControllerResult<TR>(Definition, actionExpression);
        }
    }

    public class FluentActionWithController<TU1, TU2> : FluentActionBase
    {
        public FluentActionWithController(FluentActionDefinition fluentActionDefinition, FluentActionUsingDefinition usingDefinition) : base(fluentActionDefinition)
        {
            Definition.CurrentHandler.Usings.Add(usingDefinition);
        }

        public FluentActionWithControllerResult<TR> ToAction<TR>(Expression<Func<TU1, TU2, TR>> actionExpression)
        {
            return new FluentActionWithControllerResult<TR>(Definition, actionExpression);
        }
    }

    public class FluentActionWithController<TU1, TU2, TU3> : FluentActionBase
    {
        public FluentActionWithController(FluentActionDefinition fluentActionDefinition, FluentActionUsingDefinition usingDefinition) : base(fluentActionDefinition)
        {
            Definition.CurrentHandler.Usings.Add(usingDefinition);
        }

        public FluentActionWithControllerResult<TR> ToAction<TR>(Expression<Func<TU1, TU2, TU3, TR>> actionExpression)
        {
            return new FluentActionWithControllerResult<TR>(Definition, actionExpression);
        }
    }

    public class FluentActionWithController<TU1, TU2, TU3, TU4> : FluentActionBase
    {
        public FluentActionWithController(FluentActionDefinition fluentActionDefinition, FluentActionUsingDefinition usingDefinition) : base(fluentActionDefinition)
        {
            Definition.CurrentHandler.Usings.Add(usingDefinition);
        }

        public FluentActionWithControllerResult<TR> ToAction<TR>(Expression<Func<TU1, TU2, TU3, TU4, TR>> actionExpression)
        {
            return new FluentActionWithControllerResult<TR>(Definition, actionExpression);
        }
    }

    public class FluentActionWithController<TU1, TU2, TU3, TU4, TU5> : FluentActionBase
    {
        public FluentActionWithController(FluentActionDefinition fluentActionDefinition, FluentActionUsingDefinition usingDefinition) : base(fluentActionDefinition)
        {
            Definition.CurrentHandler.Usings.Add(usingDefinition);
        }

        public FluentActionWithControllerResult<TR> ToAction<TR>(Expression<Func<TU1, TU2, TU3, TU4, TU5, TR>> actionExpression)
        {
            return new FluentActionWithControllerResult<TR>(Definition, actionExpression);
        }
    }

    public class FluentActionWithController<TU1, TU2, TU3, TU4, TU5, TU6> : FluentActionBase
    {
        public FluentActionWithController(FluentActionDefinition fluentActionDefinition, FluentActionUsingDefinition usingDefinition) : base(fluentActionDefinition)
        {
            Definition.CurrentHandler.Usings.Add(usingDefinition);
        }

        public FluentActionWithControllerResult<TR> ToAction<TR>(Expression<Func<TU1, TU2, TU3, TU4, TU5, TU6, TR>> actionExpression)
        {
            return new FluentActionWithControllerResult<TR>(Definition, actionExpression);
        }
    }

    public class FluentActionWithController<TU1, TU2, TU3, TU4, TU5, TU6, TU7> : FluentActionBase
    {
        public FluentActionWithController(FluentActionDefinition fluentActionDefinition, FluentActionUsingDefinition usingDefinition) : base(fluentActionDefinition)
        {
            Definition.CurrentHandler.Usings.Add(usingDefinition);
        }

        public FluentActionWithControllerResult<TR> ToAction<TR>(Expression<Func<TU1, TU2, TU3, TU4, TU5, TU6, TU7, TR>> actionExpression)
        {
            return new FluentActionWithControllerResult<TR>(Definition, actionExpression);
        }
    }

    public class FluentActionWithController<TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> : FluentAction
    {
        public FluentActionWithController(FluentActionDefinition fluentActionDefinition, FluentActionUsingDefinition usingDefinition) : base(fluentActionDefinition)
        {
            Definition.CurrentHandler.Usings.Add(usingDefinition);
        }

        public FluentActionWithControllerResult<TR> ToAction<TR>(Expression<Func<TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8, TR>> actionExpression)
        {
            return new FluentActionWithControllerResult<TR>(Definition, actionExpression);
        }
    }

    public class FluentActionWithController<TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8, TU9> : FluentAction
    {
        public FluentActionWithController(FluentActionDefinition fluentActionDefinition, FluentActionUsingDefinition usingDefinition) : base(fluentActionDefinition)
        {
            Definition.CurrentHandler.Usings.Add(usingDefinition);
        }

        public FluentActionWithControllerResult<TR> ToAction<TR>(Expression<Func<TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8, TU9, TR>> actionExpression)
        {
            return new FluentActionWithControllerResult<TR>(Definition, actionExpression);
        }
    }

    public class FluentActionWithControllerResult<TR> : FluentActionBase
    {
        public FluentActionWithControllerResult(FluentActionDefinition fluentActionDefinition, LambdaExpression actionExpression) : base(fluentActionDefinition)
        {
            var returnType = typeof(TR);

            Definition.CurrentHandler.Type = FluentActionHandlerType.Controller;
            Definition.CurrentHandler.Expression = actionExpression;
            Definition.CurrentHandler.ReturnType = returnType.IsAnonymous() ? typeof(object) : returnType;
        }
    }
}
