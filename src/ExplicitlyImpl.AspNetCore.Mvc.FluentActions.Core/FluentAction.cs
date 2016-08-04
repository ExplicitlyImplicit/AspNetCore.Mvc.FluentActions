using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable InconsistentNaming

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentActions
{
    public class FluentActionHandlerDefinition
    {
        public IList<EndpointUsingDefinition> Usings { get; set; }

        public Type ReturnType { get; set; }

        public Delegate Delegate { get; set; }

        public FluentActionHandlerDefinition()
        {
            Usings = new List<EndpointUsingDefinition>();
        }
    }

    public class FluentActionDefinition
    {
        public readonly string Url;

        public readonly HttpMethod HttpMethod;

        public readonly string Title;

        public IList<FluentActionHandlerDefinition> Handlers { get; set; }

        public string PathToView { get; set; }

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

        public virtual EndpointWithUsing<TU1> Using<TU1>(EndpointUsingDefinition usingDefinition)
        {
            return new EndpointWithUsing<TU1>(Definition, usingDefinition);
        }

        public virtual EndpointWithUsing<TU1> UsingService<TU1>()
        {
            return new EndpointWithUsing<TU1>(Definition, new EndpointUsingServiceDefinition
            {
                Type = typeof(TU1)
            });
        }

        public virtual EndpointWithUsing<TU1> UsingRouteParameter<TU1>(string name)
        {
            return new EndpointWithUsing<TU1>(Definition, new EndpointUsingRouteParameterDefinition
            {
                Type = typeof(TU1),
                Name = name
            });
        }

        public virtual EndpointWithUsing<TU1> UsingQueryStringParameter<TU1>(string name)
        {
            return new EndpointWithUsing<TU1>(Definition, new EndpointUsingQueryStringParameterDefinition
            {
                Type = typeof(TU1),
                Name = name
            });
        }

        public virtual EndpointWithUsing<TU1> UsingHeader<TU1>(string name)
        {
            return new EndpointWithUsing<TU1>(Definition, new EndpointUsingHeaderParameterDefinition
            {
                Type = typeof(TU1),
                Name = name
            });
        }

        public virtual EndpointWithUsing<TU1> UsingBody<TU1>()
        {
            return new EndpointWithUsing<TU1>(Definition, new EndpointUsingBodyDefinition
            {
                Type = typeof(TU1)
            });
        }

        public virtual EndpointWithUsing<TU1> UsingForm<TU1>()
        {
            return new EndpointWithUsing<TU1>(Definition, new EndpointUsingFormDefinition
            {
                Type = typeof(TU1)
            });
        }

        public virtual EndpointWithUsing<TU1> UsingFormValue<TU1>(string key)
        {
            return new EndpointWithUsing<TU1>(Definition, new EndpointUsingFormValueDefinition
            {
                Type = typeof(TU1),
                Key = key
            });
        }

        public virtual EndpointWithUsing<TU1> UsingModelBinder<TU1>(Type modelBinderType)
        {
            return new EndpointWithUsing<TU1>(Definition, new EndpointUsingModelBinderDefinition
            {
                Type = typeof(TU1),
                ModelBinderType = modelBinderType
            });
        }

        public virtual EndpointWithUsing<HttpContext> UsingHttpContext()
        {
            return new EndpointWithUsing<HttpContext>(Definition, new EndpointUsingHttpContextDefinition
            {
                Type = typeof(HttpContext)
            });
        }

        public EndpointWithResult<TR> HandledBy<TR>(Func<TR> handlerFunc)
        {
            return new EndpointWithResult<TR>(Definition, handlerFunc);
        }
    }
}
