using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable InconsistentNaming

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentEndpoints
{
    public class EndpointHandlerDefinition
    {
        public IList<EndpointUsingDefinition> Usings { get; set; }

        public Type ReturnType { get; set; }

        public Delegate Delegate { get; set; }

        public EndpointHandlerDefinition()
        {
            Usings = new List<EndpointUsingDefinition>();
        }
    }

    public class EndpointDefinition
    {
        public readonly string Url;

        public readonly HttpMethod HttpMethod;

        public readonly string Description;

        public IList<EndpointHandlerDefinition> Handlers { get; set; }

        internal EndpointHandlerDefinition CurrentHandler
        {
            get
            {
                if (!Handlers.Any())
                {
                    Handlers.Add(new EndpointHandlerDefinition());
                }

                return Handlers.Last();
            }
        }

        public EndpointDefinition(string url, HttpMethod httpMethod, string description = null)
        {
            Url = url.TrimStart('/');
            HttpMethod = httpMethod;
            Description = description;

            Handlers = new List<EndpointHandlerDefinition>();
        }

        public override string ToString()
        {
            return $"[{HttpMethod}]{Url}";
        }
    }

    public class EndpointBase
    {
        public readonly EndpointDefinition EndpointDefinition;

        public string Url => EndpointDefinition.Url;

        public HttpMethod HttpMethod => EndpointDefinition.HttpMethod;

        public string Description => EndpointDefinition.Description;

        public EndpointBase(HttpMethod httpMethod, string url, string description = null)
        {
            EndpointDefinition = new EndpointDefinition(url, httpMethod, description);
        }

        public EndpointBase(string url, HttpMethod httpMethod, string description = null)
            : this(httpMethod, url, description) { }

        public EndpointBase(EndpointDefinition endpointDefinition)
        {
            EndpointDefinition = endpointDefinition;
        }

        public override string ToString()
        {
            return EndpointDefinition.ToString();
        }
    }

    public class Endpoint : EndpointBase
    {
        public Endpoint(HttpMethod httpMethod, string url, string description = null)
            : base(httpMethod, url, description) { }

        public Endpoint(string url, HttpMethod httpMethod, string description = null)
            : base(httpMethod, url, description) { }

        public Endpoint(EndpointDefinition endpointDefinition)
            : base(endpointDefinition) { }

        public virtual EndpointWithUsing<TU1> UsingService<TU1>()
        {
            return new EndpointWithUsing<TU1>(EndpointDefinition, new EndpointUsingServiceDefinition
            {
                Type = typeof(TU1)
            });
        }

        public virtual EndpointWithUsing<TU1> UsingRouteParameter<TU1>(string name)
        {
            return new EndpointWithUsing<TU1>(EndpointDefinition, new EndpointUsingRouteParameterDefinition
            {
                Type = typeof(TU1),
                Name = name
            });
        }

        public virtual EndpointWithUsing<TU1> UsingQueryStringParameter<TU1>(string name)
        {
            return new EndpointWithUsing<TU1>(EndpointDefinition, new EndpointUsingQueryStringParameterDefinition
            {
                Type = typeof(TU1),
                Name = name
            });
        }

        public virtual EndpointWithUsing<TU1> UsingHeader<TU1>(string name)
        {
            return new EndpointWithUsing<TU1>(EndpointDefinition, new EndpointUsingHeaderParameterDefinition
            {
                Type = typeof(TU1),
                Name = name
            });
        }

        public virtual EndpointWithUsing<TU1> UsingBody<TU1>()
        {
            return new EndpointWithUsing<TU1>(EndpointDefinition, new EndpointUsingBodyDefinition
            {
                Type = typeof(TU1)
            });
        }

        public virtual EndpointWithUsing<TU1> UsingForm<TU1>()
        {
            return new EndpointWithUsing<TU1>(EndpointDefinition, new EndpointUsingFormDefinition
            {
                Type = typeof(TU1)
            });
        }

        public virtual EndpointWithUsing<TU1> UsingFormValue<TU1>(string key)
        {
            return new EndpointWithUsing<TU1>(EndpointDefinition, new EndpointUsingFormValueDefinition
            {
                Type = typeof(TU1),
                Key = key
            });
        }

        public virtual EndpointWithUsing<TU1> UsingModelBinder<TU1>(Type modelBinderType)
        {
            return new EndpointWithUsing<TU1>(EndpointDefinition, new EndpointUsingModelBinderDefinition
            {
                Type = typeof(TU1),
                ModelBinderType = modelBinderType
            });
        }

        public EndpointWithResult<TR> HandledBy<TR>(Func<TR> handlerFuncAsync)
        {
            return new EndpointWithResult<TR>(EndpointDefinition, handlerFuncAsync);
        }
    }
}
