using Microsoft.AspNetCore.Http;
using System;

// ReSharper disable InconsistentNaming

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentActions
{
    public class FluentActionWithResult<TR> : FluentActionBase
    {
        public FluentActionWithResult(FluentActionDefinition fluentActionDefinition, Delegate handlerFunc) : base(fluentActionDefinition)
        {
            var returnType = typeof(TR);

            Definition.CurrentHandler.Type = FluentActionHandlerType.Func;
            Definition.CurrentHandler.Delegate = handlerFunc;
            Definition.CurrentHandler.ReturnType = returnType.IsAnonymous() ? typeof(object) : returnType;
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1> Using<TU1>(FluentActionUsingDefinition usingDefinition)
        {
            return new FluentActionWithResultAndUsing<TR, TU1>(Definition, usingDefinition);
        }

        public virtual FluentActionWithResultAndUsing<TR, TR> UsingResultFromHandler()
        {
            return new FluentActionWithResultAndUsing<TR, TR>(Definition, new FluentActionUsingResultFromHandlerDefinition
            {
                Type = typeof(TR)
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1> UsingService<TU1>()
        {
            return new FluentActionWithResultAndUsing<TR, TU1>(Definition, new FluentActionUsingServiceDefinition
            {
                Type = typeof(TU1)
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1> UsingService<TU1>(TU1 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1>(Definition, new FluentActionUsingServiceDefinition
            {
                Type = typeof(TU1),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1> UsingRouteParameter<TU1>(string name)
        {
            return new FluentActionWithResultAndUsing<TR, TU1>(Definition, new FluentActionUsingRouteParameterDefinition
            {
                Type = typeof(TU1),
                Name = name
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1> UsingRouteParameter<TU1>(string name, TU1 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1>(Definition, new FluentActionUsingRouteParameterDefinition
            {
                Type = typeof(TU1),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1> UsingQueryStringParameter<TU1>(string name)
        {
            return new FluentActionWithResultAndUsing<TR, TU1>(Definition, new FluentActionUsingQueryStringParameterDefinition
            {
                Type = typeof(TU1),
                Name = name
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1> UsingQueryStringParameter<TU1>(string name, TU1 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1>(Definition, new FluentActionUsingQueryStringParameterDefinition
            {
                Type = typeof(TU1),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1> UsingHeader<TU1>(string name)
        {
            return new FluentActionWithResultAndUsing<TR, TU1>(Definition, new FluentActionUsingHeaderParameterDefinition
            {
                Type = typeof(TU1),
                Name = name
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1> UsingHeader<TU1>(string name, TU1 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1>(Definition, new FluentActionUsingHeaderParameterDefinition
            {
                Type = typeof(TU1),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1> UsingBody<TU1>()
        {
            return new FluentActionWithResultAndUsing<TR, TU1>(Definition, new FluentActionUsingBodyDefinition
            {
                Type = typeof(TU1)
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1> UsingBody<TU1>(TU1 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1>(Definition, new FluentActionUsingBodyDefinition
            {
                Type = typeof(TU1),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1> UsingForm<TU1>()
        {
            return new FluentActionWithResultAndUsing<TR, TU1>(Definition, new FluentActionUsingFormDefinition
            {
                Type = typeof(TU1)
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1> UsingForm<TU1>(TU1 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1>(Definition, new FluentActionUsingFormDefinition
            {
                Type = typeof(TU1),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1> UsingFormValue<TU1>(string key)
        {
            return new FluentActionWithResultAndUsing<TR, TU1>(Definition, new FluentActionUsingFormValueDefinition
            {
                Type = typeof(TU1),
                Key = key
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1> UsingFormValue<TU1>(string key, TU1 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1>(Definition, new FluentActionUsingFormValueDefinition
            {
                Type = typeof(TU1),
                Key = key,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1> UsingModelBinder<TU1>(Type modelBinderType)
        {
            return new FluentActionWithResultAndUsing<TR, TU1>(Definition, new FluentActionUsingModelBinderDefinition
            {
                Type = typeof(TU1),
                ModelBinderType = modelBinderType
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1> UsingModelBinder<TU1>(Type modelBinderType, TU1 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1>(Definition, new FluentActionUsingModelBinderDefinition
            {
                Type = typeof(TU1),
                ModelBinderType = modelBinderType,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, HttpContext> UsingHttpContext()
        {
            return new FluentActionWithResultAndUsing<TR, HttpContext>(Definition, new FluentActionUsingHttpContextDefinition
            {
                Type = typeof(HttpContext)
            });
        }

        public FluentActionWithResult<TR2> To<TR2>(Func<TR2> handlerFuncAsync)
        {
            return new FluentActionWithResult<TR2>(Definition, handlerFuncAsync);
        }

        public FluentActionWithView ToView(string pathToView)
        {
            Definition.Handlers.Add(new FluentActionHandlerDefinition());
            return new FluentActionWithView(Definition, pathToView);
        }

        public FluentActionWithPartialView ToPartialView(string pathToView)
        {
            return new FluentActionWithPartialView(Definition, pathToView);
        }

        public FluentActionWithViewComponent ToViewComponent(string pathToView)
        {
            return new FluentActionWithViewComponent(Definition, pathToView);
        }
    }

    public class FluentActionWithResultAndUsing<TR, TU1> : FluentActionBase
    {
        public FluentActionWithResultAndUsing(FluentActionDefinition fluentActionDefinition, FluentActionUsingDefinition usingDefinition) : base(fluentActionDefinition)
        {
            Definition.Handlers.Add(new FluentActionHandlerDefinition());
            Definition.CurrentHandler.Usings.Add(usingDefinition);
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2> Using<TU2>(FluentActionUsingDefinition usingDefinition)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2>(Definition, usingDefinition);
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TR> UsingResultFromHandler()
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TR>(Definition, new FluentActionUsingResultFromHandlerDefinition
            {
                Type = typeof(TR)
            });
        }

        public FluentActionWithResultAndUsing<TR, TU1, TU2> UsingService<TU2>()
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2>(Definition, new FluentActionUsingServiceDefinition
            {
                Type = typeof(TU2)
            });
        }

        public FluentActionWithResultAndUsing<TR, TU1, TU2> UsingService<TU2>(TU2 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2>(Definition, new FluentActionUsingServiceDefinition
            {
                Type = typeof(TU2),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public FluentActionWithResultAndUsing<TR, TU1, TU2> UsingRouteParameter<TU2>(string name)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2>(Definition, new FluentActionUsingRouteParameterDefinition
            {
                Type = typeof(TU2),
                Name = name
            });
        }

        public FluentActionWithResultAndUsing<TR, TU1, TU2> UsingRouteParameter<TU2>(string name, TU2 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2>(Definition, new FluentActionUsingRouteParameterDefinition
            {
                Type = typeof(TU2),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2> UsingQueryStringParameter<TU2>(string name)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2>(Definition, new FluentActionUsingQueryStringParameterDefinition
            {
                Type = typeof(TU2),
                Name = name
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2> UsingQueryStringParameter<TU2>(string name, TU2 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2>(Definition, new FluentActionUsingQueryStringParameterDefinition
            {
                Type = typeof(TU2),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2> UsingHeader<TU2>(string name)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2>(Definition, new FluentActionUsingHeaderParameterDefinition
            {
                Type = typeof(TU2),
                Name = name
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2> UsingHeader<TU2>(string name, TU2 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2>(Definition, new FluentActionUsingHeaderParameterDefinition
            {
                Type = typeof(TU2),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public FluentActionWithResultAndUsing<TR, TU1, TU2> UsingBody<TU2>()
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2>(Definition, new FluentActionUsingBodyDefinition
            {
                Type = typeof(TU2)
            });
        }

        public FluentActionWithResultAndUsing<TR, TU1, TU2> UsingBody<TU2>(TU2 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2>(Definition, new FluentActionUsingBodyDefinition
            {
                Type = typeof(TU2),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2> UsingForm<TU2>()
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2>(Definition, new FluentActionUsingFormDefinition
            {
                Type = typeof(TU2)
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2> UsingForm<TU2>(TU2 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2>(Definition, new FluentActionUsingFormDefinition
            {
                Type = typeof(TU2),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2> UsingFormValue<TU2>(string key)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2>(Definition, new FluentActionUsingFormValueDefinition
            {
                Type = typeof(TU2),
                Key = key
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2> UsingFormValue<TU2>(string key, TU2 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2>(Definition, new FluentActionUsingFormValueDefinition
            {
                Type = typeof(TU2),
                Key = key,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2> UsingModelBinder<TU2>(Type modelBinderType)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2>(Definition, new FluentActionUsingModelBinderDefinition
            {
                Type = typeof(TU2),
                ModelBinderType = modelBinderType
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2> UsingModelBinder<TU2>(Type modelBinderType, TU2 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2>(Definition, new FluentActionUsingModelBinderDefinition
            {
                Type = typeof(TU2),
                ModelBinderType = modelBinderType,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, HttpContext> UsingHttpContext()
        {
            return new FluentActionWithResultAndUsing<TR, TU1, HttpContext>(Definition, new FluentActionUsingHttpContextDefinition
            {
                Type = typeof(HttpContext)
            });
        }

        public FluentActionWithResult<TR2> To<TR2>(Func<TU1, TR2> handlerFuncAsync)
        {
            return new FluentActionWithResult<TR2>(Definition, handlerFuncAsync);
        }
    }

    public class FluentActionWithResultAndUsing<TR, TU1, TU2> : FluentActionBase
    {
        public FluentActionWithResultAndUsing(FluentActionDefinition fluentActionDefinition, FluentActionUsingDefinition usingDefinition) : base(fluentActionDefinition)
        {
            Definition.CurrentHandler.Usings.Add(usingDefinition);
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3> Using<TU3>(FluentActionUsingDefinition usingDefinition)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3>(Definition, usingDefinition);
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TR> UsingResultFromHandler()
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TR>(Definition, new FluentActionUsingResultFromHandlerDefinition
            {
                Type = typeof(TR)
            });
        }

        public FluentActionWithResultAndUsing<TR, TU1, TU2, TU3> UsingService<TU3>()
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3>(Definition, new FluentActionUsingServiceDefinition
            {
                Type = typeof(TU3)
            });
        }

        public FluentActionWithResultAndUsing<TR, TU1, TU2, TU3> UsingService<TU3>(TU3 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3>(Definition, new FluentActionUsingServiceDefinition
            {
                Type = typeof(TU3),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public FluentActionWithResultAndUsing<TR, TU1, TU2, TU3> UsingRouteParameter<TU3>(string name)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3>(Definition, new FluentActionUsingRouteParameterDefinition
            {
                Type = typeof(TU3),
                Name = name
            });
        }

        public FluentActionWithResultAndUsing<TR, TU1, TU2, TU3> UsingRouteParameter<TU3>(string name, TU3 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3>(Definition, new FluentActionUsingRouteParameterDefinition
            {
                Type = typeof(TU3),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3> UsingQueryStringParameter<TU3>(string name)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3>(Definition, new FluentActionUsingQueryStringParameterDefinition
            {
                Type = typeof(TU3),
                Name = name
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3> UsingQueryStringParameter<TU3>(string name, TU3 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3>(Definition, new FluentActionUsingQueryStringParameterDefinition
            {
                Type = typeof(TU3),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3> UsingHeader<TU3>(string name)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3>(Definition, new FluentActionUsingHeaderParameterDefinition
            {
                Type = typeof(TU3),
                Name = name
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3> UsingHeader<TU3>(string name, TU3 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3>(Definition, new FluentActionUsingHeaderParameterDefinition
            {
                Type = typeof(TU3),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public FluentActionWithResultAndUsing<TR, TU1, TU2, TU3> UsingBody<TU3>()
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3>(Definition, new FluentActionUsingBodyDefinition
            {
                Type = typeof(TU3)
            });
        }

        public FluentActionWithResultAndUsing<TR, TU1, TU2, TU3> UsingBody<TU3>(TU3 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3>(Definition, new FluentActionUsingBodyDefinition
            {
                Type = typeof(TU3),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3> UsingForm<TU3>()
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3>(Definition, new FluentActionUsingFormDefinition
            {
                Type = typeof(TU3)
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3> UsingForm<TU3>(TU3 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3>(Definition, new FluentActionUsingFormDefinition
            {
                Type = typeof(TU3),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3> UsingFormValue<TU3>(string key)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3>(Definition, new FluentActionUsingFormValueDefinition
            {
                Type = typeof(TU3),
                Key = key
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3> UsingFormValue<TU3>(string key, TU3 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3>(Definition, new FluentActionUsingFormValueDefinition
            {
                Type = typeof(TU3),
                Key = key,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3> UsingModelBinder<TU3>(Type modelBinderType)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3>(Definition, new FluentActionUsingModelBinderDefinition
            {
                Type = typeof(TU3),
                ModelBinderType = modelBinderType
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3> UsingModelBinder<TU3>(Type modelBinderType, TU3 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3>(Definition, new FluentActionUsingModelBinderDefinition
            {
                Type = typeof(TU3),
                ModelBinderType = modelBinderType,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, HttpContext> UsingHttpContext()
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, HttpContext>(Definition, new FluentActionUsingHttpContextDefinition
            {
                Type = typeof(HttpContext)
            });
        }

        public FluentActionWithResult<TR2> To<TR2>(Func<TU1, TU2, TR2> handlerFuncAsync)
        {
            return new FluentActionWithResult<TR2>(Definition, handlerFuncAsync);
        }
    }

    public class FluentActionWithResultAndUsing<TR, TU1, TU2, TU3> : FluentActionBase
    {
        public FluentActionWithResultAndUsing(FluentActionDefinition fluentActionDefinition, FluentActionUsingDefinition usingDefinition) : base(fluentActionDefinition)
        {
            Definition.CurrentHandler.Usings.Add(usingDefinition);
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4> Using<TU4>(FluentActionUsingDefinition usingDefinition)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4>(Definition, usingDefinition);
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TR> UsingResultFromHandler()
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TR>(Definition, new FluentActionUsingResultFromHandlerDefinition
            {
                Type = typeof(TR)
            });
        }

        public FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4> UsingService<TU4>()
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4>(Definition, new FluentActionUsingServiceDefinition
            {
                Type = typeof(TU4)
            });
        }

        public FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4> UsingService<TU4>(TU4 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4>(Definition, new FluentActionUsingServiceDefinition
            {
                Type = typeof(TU4),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4> UsingRouteParameter<TU4>(string name)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4>(Definition, new FluentActionUsingRouteParameterDefinition
            {
                Type = typeof(TU4),
                Name = name
            });
        }

        public FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4> UsingRouteParameter<TU4>(string name, TU4 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4>(Definition, new FluentActionUsingRouteParameterDefinition
            {
                Type = typeof(TU4),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4> UsingQueryStringParameter<TU4>(string name)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4>(Definition, new FluentActionUsingQueryStringParameterDefinition
            {
                Type = typeof(TU4),
                Name = name
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4> UsingQueryStringParameter<TU4>(string name, TU4 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4>(Definition, new FluentActionUsingQueryStringParameterDefinition
            {
                Type = typeof(TU4),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4> UsingHeader<TU4>(string name)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4>(Definition, new FluentActionUsingHeaderParameterDefinition
            {
                Type = typeof(TU4),
                Name = name
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4> UsingHeader<TU4>(string name, TU4 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4>(Definition, new FluentActionUsingHeaderParameterDefinition
            {
                Type = typeof(TU4),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4> UsingBody<TU4>()
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4>(Definition, new FluentActionUsingBodyDefinition
            {
                Type = typeof(TU4)
            });
        }

        public FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4> UsingBody<TU4>(TU4 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4>(Definition, new FluentActionUsingBodyDefinition
            {
                Type = typeof(TU4),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4> UsingForm<TU4>()
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4>(Definition, new FluentActionUsingFormDefinition
            {
                Type = typeof(TU4)
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4> UsingForm<TU4>(TU4 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4>(Definition, new FluentActionUsingFormDefinition
            {
                Type = typeof(TU4),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4> UsingFormValue<TU4>(string key)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4>(Definition, new FluentActionUsingFormValueDefinition
            {
                Type = typeof(TU4),
                Key = key
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4> UsingFormValue<TU4>(string key, TU4 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4>(Definition, new FluentActionUsingFormValueDefinition
            {
                Type = typeof(TU4),
                Key = key,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4> UsingModelBinder<TU4>(Type modelBinderType)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4>(Definition, new FluentActionUsingModelBinderDefinition
            {
                Type = typeof(TU4),
                ModelBinderType = modelBinderType
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4> UsingModelBinder<TU4>(Type modelBinderType, TU4 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4>(Definition, new FluentActionUsingModelBinderDefinition
            {
                Type = typeof(TU4),
                ModelBinderType = modelBinderType,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, HttpContext> UsingHttpContext()
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, HttpContext>(Definition, new FluentActionUsingHttpContextDefinition
            {
                Type = typeof(HttpContext)
            });
        }

        public FluentActionWithResult<TR2> To<TR2>(Func<TU1, TU2, TU3, TR2> handlerFuncAsync)
        {
            return new FluentActionWithResult<TR2>(Definition, handlerFuncAsync);
        }
    }

    public class FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4> : FluentActionBase
    {
        public FluentActionWithResultAndUsing(FluentActionDefinition fluentActionDefinition, FluentActionUsingDefinition usingDefinition) : base(fluentActionDefinition)
        {
            Definition.CurrentHandler.Usings.Add(usingDefinition);
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5> Using<TU5>(FluentActionUsingDefinition usingDefinition)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5>(Definition, usingDefinition);
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TR> UsingResultFromHandler()
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TR>(Definition, new FluentActionUsingResultFromHandlerDefinition
            {
                Type = typeof(TR)
            });
        }

        public FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5> UsingService<TU5>()
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5>(Definition, new FluentActionUsingServiceDefinition
            {
                Type = typeof(TU5)
            });
        }

        public FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5> UsingService<TU5>(TU5 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5>(Definition, new FluentActionUsingServiceDefinition
            {
                Type = typeof(TU5),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5> UsingRouteParameter<TU5>(string name)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5>(Definition, new FluentActionUsingRouteParameterDefinition
            {
                Type = typeof(TU5),
                Name = name
            });
        }

        public FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5> UsingRouteParameter<TU5>(string name, TU5 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5>(Definition, new FluentActionUsingRouteParameterDefinition
            {
                Type = typeof(TU5),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5> UsingQueryStringParameter<TU5>(string name)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5>(Definition, new FluentActionUsingQueryStringParameterDefinition
            {
                Type = typeof(TU5),
                Name = name
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5> UsingQueryStringParameter<TU5>(string name, TU5 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5>(Definition, new FluentActionUsingQueryStringParameterDefinition
            {
                Type = typeof(TU5),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5> UsingHeader<TU5>(string name)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5>(Definition, new FluentActionUsingHeaderParameterDefinition
            {
                Type = typeof(TU5),
                Name = name
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5> UsingHeader<TU5>(string name, TU5 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5>(Definition, new FluentActionUsingHeaderParameterDefinition
            {
                Type = typeof(TU5),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5> UsingBody<TU5>()
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5>(Definition, new FluentActionUsingBodyDefinition
            {
                Type = typeof(TU5)
            });
        }

        public FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5> UsingBody<TU5>(TU5 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5>(Definition, new FluentActionUsingBodyDefinition
            {
                Type = typeof(TU5),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5> UsingForm<TU5>()
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5>(Definition, new FluentActionUsingFormDefinition
            {
                Type = typeof(TU5)
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5> UsingForm<TU5>(TU5 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5>(Definition, new FluentActionUsingFormDefinition
            {
                Type = typeof(TU5),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5> UsingFormValue<TU5>(string key)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5>(Definition, new FluentActionUsingFormValueDefinition
            {
                Type = typeof(TU5),
                Key = key
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5> UsingFormValue<TU5>(string key, TU5 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5>(Definition, new FluentActionUsingFormValueDefinition
            {
                Type = typeof(TU5),
                Key = key,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5> UsingModelBinder<TU5>(Type modelBinderType)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5>(Definition, new FluentActionUsingModelBinderDefinition
            {
                Type = typeof(TU5),
                ModelBinderType = modelBinderType
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5> UsingModelBinder<TU5>(Type modelBinderType, TU5 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5>(Definition, new FluentActionUsingModelBinderDefinition
            {
                Type = typeof(TU5),
                ModelBinderType = modelBinderType,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, HttpContext> UsingHttpContext()
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, HttpContext>(Definition, new FluentActionUsingHttpContextDefinition
            {
                Type = typeof(HttpContext)
            });
        }

        public FluentActionWithResult<TR2> To<TR2>(Func<TU1, TU2, TU3, TU4, TR2> handlerFuncAsync)
        {
            return new FluentActionWithResult<TR2>(Definition, handlerFuncAsync);
        }
    }

    public class FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5> : FluentActionBase
    {
        public FluentActionWithResultAndUsing(FluentActionDefinition fluentActionDefinition, FluentActionUsingDefinition usingDefinition) : base(fluentActionDefinition)
        {
            Definition.CurrentHandler.Usings.Add(usingDefinition);
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6> Using<TU6>(FluentActionUsingDefinition usingDefinition)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6>(Definition, usingDefinition);
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TR> UsingResultFromHandler()
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TR>(Definition, new FluentActionUsingResultFromHandlerDefinition
            {
                Type = typeof(TR)
            });
        }

        public FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6> UsingService<TU6>()
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6>(Definition, new FluentActionUsingServiceDefinition
            {
                Type = typeof(TU6)
            });
        }

        public FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6> UsingService<TU6>(TU6 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6>(Definition, new FluentActionUsingServiceDefinition
            {
                Type = typeof(TU6),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6> UsingRouteParameter<TU6>(string name)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6>(Definition, new FluentActionUsingRouteParameterDefinition
            {
                Type = typeof(TU6),
                Name = name
            });
        }

        public FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6> UsingRouteParameter<TU6>(string name, TU6 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6>(Definition, new FluentActionUsingRouteParameterDefinition
            {
                Type = typeof(TU6),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6> UsingQueryStringParameter<TU6>(string name)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6>(Definition, new FluentActionUsingQueryStringParameterDefinition
            {
                Type = typeof(TU6),
                Name = name
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6> UsingQueryStringParameter<TU6>(string name, TU6 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6>(Definition, new FluentActionUsingQueryStringParameterDefinition
            {
                Type = typeof(TU6),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6> UsingHeader<TU6>(string name)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6>(Definition, new FluentActionUsingHeaderParameterDefinition
            {
                Type = typeof(TU6),
                Name = name
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6> UsingHeader<TU6>(string name, TU6 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6>(Definition, new FluentActionUsingHeaderParameterDefinition
            {
                Type = typeof(TU6),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6> UsingBody<TU6>()
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6>(Definition, new FluentActionUsingBodyDefinition
            {
                Type = typeof(TU6)
            });
        }

        public FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6> UsingBody<TU6>(TU6 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6>(Definition, new FluentActionUsingBodyDefinition
            {
                Type = typeof(TU6),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6> UsingForm<TU6>()
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6>(Definition, new FluentActionUsingFormDefinition
            {
                Type = typeof(TU6)
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6> UsingForm<TU6>(TU6 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6>(Definition, new FluentActionUsingFormDefinition
            {
                Type = typeof(TU6),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6> UsingFormValue<TU6>(string key)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6>(Definition, new FluentActionUsingFormValueDefinition
            {
                Type = typeof(TU6),
                Key = key
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6> UsingFormValue<TU6>(string key, TU6 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6>(Definition, new FluentActionUsingFormValueDefinition
            {
                Type = typeof(TU6),
                Key = key,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6> UsingModelBinder<TU6>(Type modelBinderType)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6>(Definition, new FluentActionUsingModelBinderDefinition
            {
                Type = typeof(TU6),
                ModelBinderType = modelBinderType
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6> UsingModelBinder<TU6>(Type modelBinderType, TU6 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6>(Definition, new FluentActionUsingModelBinderDefinition
            {
                Type = typeof(TU6),
                ModelBinderType = modelBinderType,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, HttpContext> UsingHttpContext()
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, HttpContext>(Definition, new FluentActionUsingHttpContextDefinition
            {
                Type = typeof(HttpContext)
            });
        }

        public FluentActionWithResult<TR2> To<TR2>(Func<TU1, TU2, TU3, TU4, TU5, TR2> handlerFuncAsync)
        {
            return new FluentActionWithResult<TR2>(Definition, handlerFuncAsync);
        }
    }

    public class FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6> : FluentActionBase
    {
        public FluentActionWithResultAndUsing(FluentActionDefinition fluentActionDefinition, FluentActionUsingDefinition usingDefinition) : base(fluentActionDefinition)
        {
            Definition.CurrentHandler.Usings.Add(usingDefinition);
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7> Using<TU7>(FluentActionUsingDefinition usingDefinition)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7>(Definition, usingDefinition);
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TR> UsingResultFromHandler()
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TR>(Definition, new FluentActionUsingResultFromHandlerDefinition
            {
                Type = typeof(TR)
            });
        }

        public FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7> UsingService<TU7>()
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7>(Definition, new FluentActionUsingServiceDefinition
            {
                Type = typeof(TU7)
            });
        }

        public FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7> UsingService<TU7>(TU7 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7>(Definition, new FluentActionUsingServiceDefinition
            {
                Type = typeof(TU7),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7> UsingRouteParameter<TU7>(string name)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7>(Definition, new FluentActionUsingRouteParameterDefinition
            {
                Type = typeof(TU7),
                Name = name
            });
        }

        public FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7> UsingRouteParameter<TU7>(string name, TU7 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7>(Definition, new FluentActionUsingRouteParameterDefinition
            {
                Type = typeof(TU7),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7> UsingQueryStringParameter<TU7>(string name)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7>(Definition, new FluentActionUsingQueryStringParameterDefinition
            {
                Type = typeof(TU7),
                Name = name
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7> UsingQueryStringParameter<TU7>(string name, TU7 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7>(Definition, new FluentActionUsingQueryStringParameterDefinition
            {
                Type = typeof(TU7),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7> UsingHeader<TU7>(string name)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7>(Definition, new FluentActionUsingHeaderParameterDefinition
            {
                Type = typeof(TU7),
                Name = name
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7> UsingHeader<TU7>(string name, TU7 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7>(Definition, new FluentActionUsingHeaderParameterDefinition
            {
                Type = typeof(TU7),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7> UsingBody<TU7>()
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7>(Definition, new FluentActionUsingBodyDefinition
            {
                Type = typeof(TU7)
            });
        }

        public FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7> UsingBody<TU7>(TU7 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7>(Definition, new FluentActionUsingBodyDefinition
            {
                Type = typeof(TU7),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7> UsingForm<TU7>()
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7>(Definition, new FluentActionUsingFormDefinition
            {
                Type = typeof(TU7)
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7> UsingForm<TU7>(TU7 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7>(Definition, new FluentActionUsingFormDefinition
            {
                Type = typeof(TU7),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7> UsingFormValue<TU7>(string key)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7>(Definition, new FluentActionUsingFormValueDefinition
            {
                Type = typeof(TU7),
                Key = key
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7> UsingFormValue<TU7>(string key, TU7 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7>(Definition, new FluentActionUsingFormValueDefinition
            {
                Type = typeof(TU7),
                Key = key,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7> UsingModelBinder<TU7>(Type modelBinderType)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7>(Definition, new FluentActionUsingModelBinderDefinition
            {
                Type = typeof(TU7),
                ModelBinderType = modelBinderType
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7> UsingModelBinder<TU7>(Type modelBinderType, TU7 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7>(Definition, new FluentActionUsingModelBinderDefinition
            {
                Type = typeof(TU7),
                ModelBinderType = modelBinderType,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, HttpContext> UsingHttpContext()
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, HttpContext>(Definition, new FluentActionUsingHttpContextDefinition
            {
                Type = typeof(HttpContext)
            });
        }

        public FluentActionWithResult<TR2> To<TR2>(Func<TU1, TU2, TU3, TU4, TU5, TU6, TR2> handlerFuncAsync)
        {
            return new FluentActionWithResult<TR2>(Definition, handlerFuncAsync);
        }
    }

    public class FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7> : FluentActionBase
    {
        public FluentActionWithResultAndUsing(FluentActionDefinition fluentActionDefinition, FluentActionUsingDefinition usingDefinition) : base(fluentActionDefinition)
        {
            Definition.CurrentHandler.Usings.Add(usingDefinition);
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> Using<TU8>(FluentActionUsingDefinition usingDefinition)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(Definition, usingDefinition);
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TR> UsingResultFromHandler()
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TR>(Definition, new FluentActionUsingResultFromHandlerDefinition
            {
                Type = typeof(TR)
            });
        }

        public FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> UsingService<TU8>()
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(Definition, new FluentActionUsingServiceDefinition
            {
                Type = typeof(TU8)
            });
        }

        public FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> UsingService<TU8>(TU8 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(Definition, new FluentActionUsingServiceDefinition
            {
                Type = typeof(TU8),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> UsingRouteParameter<TU8>(string name)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(Definition, new FluentActionUsingRouteParameterDefinition
            {
                Type = typeof(TU8),
                Name = name
            });
        }

        public FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> UsingRouteParameter<TU8>(string name, TU8 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(Definition, new FluentActionUsingRouteParameterDefinition
            {
                Type = typeof(TU8),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> UsingQueryStringParameter<TU8>(string name)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(Definition, new FluentActionUsingQueryStringParameterDefinition
            {
                Type = typeof(TU8),
                Name = name
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> UsingQueryStringParameter<TU8>(string name, TU8 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(Definition, new FluentActionUsingQueryStringParameterDefinition
            {
                Type = typeof(TU8),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> UsingHeader<TU8>(string name)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(Definition, new FluentActionUsingHeaderParameterDefinition
            {
                Type = typeof(TU8),
                Name = name
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> UsingHeader<TU8>(string name, TU8 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(Definition, new FluentActionUsingHeaderParameterDefinition
            {
                Type = typeof(TU8),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> UsingBody<TU8>()
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(Definition, new FluentActionUsingBodyDefinition
            {
                Type = typeof(TU8)
            });
        }

        public FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> UsingBody<TU8>(TU8 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(Definition, new FluentActionUsingBodyDefinition
            {
                Type = typeof(TU8),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> UsingForm<TU8>()
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(Definition, new FluentActionUsingFormDefinition
            {
                Type = typeof(TU8)
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> UsingForm<TU8>(TU8 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(Definition, new FluentActionUsingFormDefinition
            {
                Type = typeof(TU8),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> UsingFormValue<TU8>(string key)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(Definition, new FluentActionUsingFormValueDefinition
            {
                Type = typeof(TU8),
                Key = key
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> UsingFormValue<TU8>(string key, TU8 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(Definition, new FluentActionUsingFormValueDefinition
            {
                Type = typeof(TU8),
                Key = key,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> UsingModelBinder<TU8>(Type modelBinderType)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(Definition, new FluentActionUsingModelBinderDefinition
            {
                Type = typeof(TU8),
                ModelBinderType = modelBinderType
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> UsingModelBinder<TU8>(Type modelBinderType, TU8 defaultValue)
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(Definition, new FluentActionUsingModelBinderDefinition
            {
                Type = typeof(TU8),
                ModelBinderType = modelBinderType,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, HttpContext> UsingHttpContext()
        {
            return new FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, HttpContext>(Definition, new FluentActionUsingHttpContextDefinition
            {
                Type = typeof(HttpContext)
            });
        }

        public FluentActionWithResult<TR2> To<TR2>(Func<TU1, TU2, TU3, TU4, TU5, TU6, TU7, TR2> handlerFuncAsync)
        {
            return new FluentActionWithResult<TR2>(Definition, handlerFuncAsync);
        }
    }

    public class FluentActionWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> : FluentAction
    {
        public FluentActionWithResultAndUsing(FluentActionDefinition fluentActionDefinition, FluentActionUsingDefinition usingDefinition) : base(fluentActionDefinition)
        {
            Definition.CurrentHandler.Usings.Add(usingDefinition);
        }

        public FluentActionWithResult<TR2> To<TR2>(Func<TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8, TR2> handlerFuncAsync)
        {
            return new FluentActionWithResult<TR2>(Definition, handlerFuncAsync);
        }
    }
}
