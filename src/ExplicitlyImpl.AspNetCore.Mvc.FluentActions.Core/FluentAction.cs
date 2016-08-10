using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable InconsistentNaming

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentActions
{
    public enum FluentActionHandlerType
    {
        Unknown,
        Func,
        View,
        PartialView,
        ViewComponent
    }

    public class FluentActionHandlerDefinition
    {
        public FluentActionHandlerType Type { get; set; }

        public IList<FluentActionUsingDefinition> Usings { get; set; }

        public Type ReturnType { get; set; }

        public Delegate Delegate { get; set; }

        public string PathToView { get; set; }

        public FluentActionHandlerDefinition()
        {
            Type = FluentActionHandlerType.Unknown;
            Usings = new List<FluentActionUsingDefinition>();
        }
    }

    public class FluentActionDefinition
    {
        public readonly string Url;

        public readonly HttpMethod HttpMethod;

        public readonly string Title;

        public IList<FluentActionHandlerDefinition> Handlers { get; set; }

        internal FluentActionHandlerDefinition CurrentHandler
        {
            get
            {
                if (!Handlers.Any())
                {
                    Handlers.Add(new FluentActionHandlerDefinition());
                }

                return Handlers.Last();
            }
        }

        public FluentActionDefinition(string url, HttpMethod httpMethod, string title = null)
        {
            Url = url.TrimStart('/');
            HttpMethod = httpMethod;
            Title = title;

            Handlers = new List<FluentActionHandlerDefinition>();
        }

        public override string ToString()
        {
            return $"[{HttpMethod}]{Url}";
        }
    }

    public class FluentActionBase
    {
        public readonly FluentActionDefinition Definition;

        public string Url => Definition.Url;

        public HttpMethod HttpMethod => Definition.HttpMethod;

        public string Title => Definition.Title;

        public FluentActionBase(HttpMethod httpMethod, string url, string title = null)
        {
            Definition = new FluentActionDefinition(url, httpMethod, title);
        }

        public FluentActionBase(string url, HttpMethod httpMethod, string title = null)
            : this(httpMethod, url, title) { }

        public FluentActionBase(FluentActionDefinition actionDefinition)
        {
            Definition = actionDefinition;
        }

        public override string ToString()
        {
            return Definition.ToString();
        }
    }

    public class FluentAction : FluentActionBase
    {
        public FluentAction(HttpMethod httpMethod, string url, string title = null)
            : base(httpMethod, url, title) { }

        public FluentAction(string url, HttpMethod httpMethod, string title = null)
            : base(httpMethod, url, title) { }

        public FluentAction(FluentActionDefinition fluentActionDefinition)
            : base(fluentActionDefinition) { }

        public virtual FluentActionWithUsing<TU1> Using<TU1>(FluentActionUsingDefinition usingDefinition)
        {
            return new FluentActionWithUsing<TU1>(Definition, usingDefinition);
        }

        public virtual FluentActionWithUsing<TU1> UsingService<TU1>()
        {
            return new FluentActionWithUsing<TU1>(Definition, new FluentActionUsingServiceDefinition
            {
                Type = typeof(TU1)
            });
        }

        public virtual FluentActionWithUsing<TU1> UsingService<TU1>(TU1 defaultValue)
        {
            return new FluentActionWithUsing<TU1>(Definition, new FluentActionUsingServiceDefinition
            {
                Type = typeof(TU1),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithUsing<TU1> UsingRouteParameter<TU1>(string name)
        {
            return new FluentActionWithUsing<TU1>(Definition, new FluentActionUsingRouteParameterDefinition
            {
                Type = typeof(TU1),
                Name = name
            });
        }

        public virtual FluentActionWithUsing<TU1> UsingRouteParameter<TU1>(string name, TU1 defaultValue)
        {
            return new FluentActionWithUsing<TU1>(Definition, new FluentActionUsingRouteParameterDefinition
            {
                Type = typeof(TU1),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithUsing<TU1> UsingQueryStringParameter<TU1>(string name)
        {
            return new FluentActionWithUsing<TU1>(Definition, new FluentActionUsingQueryStringParameterDefinition
            {
                Type = typeof(TU1),
                Name = name
            });
        }

        public virtual FluentActionWithUsing<TU1> UsingQueryStringParameter<TU1>(string name, TU1 defaultValue)
        {
            return new FluentActionWithUsing<TU1>(Definition, new FluentActionUsingQueryStringParameterDefinition
            {
                Type = typeof(TU1),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithUsing<TU1> UsingHeader<TU1>(string name)
        {
            return new FluentActionWithUsing<TU1>(Definition, new FluentActionUsingHeaderParameterDefinition
            {
                Type = typeof(TU1),
                Name = name
            });
        }

        public virtual FluentActionWithUsing<TU1> UsingHeader<TU1>(string name, TU1 defaultValue)
        {
            return new FluentActionWithUsing<TU1>(Definition, new FluentActionUsingHeaderParameterDefinition
            {
                Type = typeof(TU1),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithUsing<TU1> UsingBody<TU1>()
        {
            return new FluentActionWithUsing<TU1>(Definition, new FluentActionUsingBodyDefinition
            {
                Type = typeof(TU1)
            });
        }

        public virtual FluentActionWithUsing<TU1> UsingBody<TU1>(TU1 defaultValue)
        {
            return new FluentActionWithUsing<TU1>(Definition, new FluentActionUsingBodyDefinition
            {
                Type = typeof(TU1),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithUsing<TU1> UsingForm<TU1>()
        {
            return new FluentActionWithUsing<TU1>(Definition, new FluentActionUsingFormDefinition
            {
                Type = typeof(TU1)
            });
        }

        public virtual FluentActionWithUsing<TU1> UsingForm<TU1>(TU1 defaultValue)
        {
            return new FluentActionWithUsing<TU1>(Definition, new FluentActionUsingFormDefinition
            {
                Type = typeof(TU1),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithUsing<TU1> UsingFormValue<TU1>(string key)
        {
            return new FluentActionWithUsing<TU1>(Definition, new FluentActionUsingFormValueDefinition
            {
                Type = typeof(TU1),
                Key = key
            });
        }

        public virtual FluentActionWithUsing<TU1> UsingFormValue<TU1>(string key, TU1 defaultValue)
        {
            return new FluentActionWithUsing<TU1>(Definition, new FluentActionUsingFormValueDefinition
            {
                Type = typeof(TU1),
                Key = key,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithUsing<TU1> UsingModelBinder<TU1>(Type modelBinderType)
        {
            return new FluentActionWithUsing<TU1>(Definition, new FluentActionUsingModelBinderDefinition
            {
                Type = typeof(TU1),
                ModelBinderType = modelBinderType
            });
        }

        public virtual FluentActionWithUsing<TU1> UsingModelBinder<TU1>(Type modelBinderType, TU1 defaultValue)
        {
            return new FluentActionWithUsing<TU1>(Definition, new FluentActionUsingModelBinderDefinition
            {
                Type = typeof(TU1),
                ModelBinderType = modelBinderType,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithUsing<HttpContext> UsingHttpContext()
        {
            return new FluentActionWithUsing<HttpContext>(Definition, new FluentActionUsingHttpContextDefinition
            {
                Type = typeof(HttpContext)
            });
        }

        public FluentActionWithResult<TR> To<TR>(Func<TR> handlerFunc)
        {
            return new FluentActionWithResult<TR>(Definition, handlerFunc);
        }
    }
}
