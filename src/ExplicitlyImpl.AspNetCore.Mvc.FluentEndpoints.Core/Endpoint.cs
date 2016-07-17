using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

// ReSharper disable InconsistentNaming

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentEndpoints
{
    public class EndpointUsingDefinition
    {
        public Type Type { get; set; }
    }

    public class EndpointUsingServiceDefinition : EndpointUsingDefinition
    {
    }

    public class EndpointUsingUrlParameterDefinition : EndpointUsingDefinition
    {
        public string Name { get; set; }
    }

    public class EndpointUsingModelFromBodyDefinition : EndpointUsingDefinition
    {
    }

    public class EndpointHandlerDefinition
    {
        public IList<EndpointUsingDefinition> Usings { get; set; }

        public Type ReturnType { get; set; }

        public LambdaExpression Func { get; set; }

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

        public virtual EndpointWithUsing<TU1> UsingUrlParameter<TU1>(string name)
        {
            return new EndpointWithUsing<TU1>(EndpointDefinition, new EndpointUsingUrlParameterDefinition
            {
                Type = typeof(TU1),
                Name = name
            });
        }

        public virtual EndpointWithUsing<TU1> UsingModelFromBody<TU1>()
        {
            return new EndpointWithUsing<TU1>(EndpointDefinition, new EndpointUsingModelFromBodyDefinition
            {
                Type = typeof(TU1)
            });
        }

        public EndpointWithResult<TR> HandledBy<TR>(Expression<Func<TR>> handlerFuncAsync)
        {
            return new EndpointWithResult<TR>(EndpointDefinition, handlerFuncAsync);
        }
    }
}
