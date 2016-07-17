using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

// ReSharper disable InconsistentNaming

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentEndpoints
{
    public class WebContext
    {

    }

    public class EndpointCollection : IEnumerable<Endpoint>
    {
        public List<Endpoint> Endpoints { get; internal set; }

        public EndpointCollection()
        {
            Endpoints = new List<Endpoint>();
        }

        public Endpoint Add(HttpMethod httpMethod, string url, string description = null)
        {
            var endpoint = new Endpoint(httpMethod, url, description);
            Endpoints.Add(endpoint);
            return endpoint;
        }

        public Endpoint Add(string url, HttpMethod httpMethod, string description = null)
        {
            var endpoint = new Endpoint(httpMethod, url, description);
            Endpoints.Add(endpoint);
            return endpoint;
        }

        public void Add(Endpoint endpoint)
        {
            Endpoints.Add(endpoint);
        }

        public void Add(IEnumerable<Endpoint> endpoints)
        {
            Endpoints.AddRange(endpoints);
        }

        public IEnumerator<Endpoint> GetEnumerator()
        {
            return Endpoints.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class EndpointHandlerUsingDefinition
    {
        public Type Type { get; set; }
    }

    public class EndpointHandlerServiceDefinition : EndpointHandlerUsingDefinition
    {
    }

    public class EndpointHandlerRouteParameterDefinition : EndpointHandlerUsingDefinition
    {
        public string Name { get; set; }
    }

    public class EndpointHandlerBodyParameterDefinition : EndpointHandlerUsingDefinition
    {
        public string Name { get; set; }
    }

    public class EndpointHandlerDefinition
    {
        public IList<EndpointHandlerUsingDefinition> Usings { get; set; }

        public Type ReturnType { get; set; }

        public LambdaExpression Func { get; set; }

        public EndpointHandlerDefinition()
        {
            Usings = new List<EndpointHandlerUsingDefinition>();
        }
    }

    public class EndpointDefinition
    {
        public readonly string Url;

        public readonly HttpMethod HttpMethod;

        public readonly string Description;

        public IList<EndpointHandlerDefinition> Handlers { get; set; }

        internal EndpointHandlerDefinition CurrentOrNewHandler
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
            return new EndpointWithUsing<TU1>(EndpointDefinition, new EndpointHandlerServiceDefinition
            {
                Type = typeof(TU1)
            });
        }

        public virtual EndpointWithUsing<TU1> UsingParameter<TU1>(string name = null)
        {
            return new EndpointWithUsing<TU1>(EndpointDefinition, new EndpointHandlerRouteParameterDefinition
            {
                Type = typeof(TU1),
                Name = name
            });
        }
    }

    public class EndpointWithUsing<TU1> : EndpointBase
    {
        public EndpointWithUsing(EndpointDefinition endpointDefinition, EndpointHandlerServiceDefinition serviceDefinition) : base(endpointDefinition)
        {
            EndpointDefinition.CurrentOrNewHandler.Usings.Add(serviceDefinition);
        }

        public EndpointWithUsing(EndpointDefinition endpointDefinition, EndpointHandlerRouteParameterDefinition parameterDefinition) : base(endpointDefinition)
        {
            EndpointDefinition.CurrentOrNewHandler.Usings.Add(parameterDefinition);
        }

        public EndpointWithUsing<TU1, TU2> UsingUrlParameter<TU2>(string name)
        {
            return new EndpointWithUsing<TU1, TU2>(EndpointDefinition, new EndpointHandlerRouteParameterDefinition
            {
                Type = typeof(TU2),
                Name = name
            });
        }

        public EndpointWithUsing<TU1, TU2> UsingModelFromBody<TU2>()
        {
            return new EndpointWithUsing<TU1, TU2>(EndpointDefinition, new EndpointHandlerBodyParameterDefinition
            {
                Type = typeof(TU2)
            });
        }

        public EndpointWithUsing<TU1, TU2> UsingService<TU2>()
        {
            return new EndpointWithUsing<TU1, TU2>(EndpointDefinition, new EndpointHandlerServiceDefinition
            {
                Type = typeof(TU2)
            });
        }

        public EndpointWithResult<TU1, TR> HandledBy<TR>(Expression<Func<TU1, TR>> handlerFuncAsync)
        {
            return new EndpointWithResult<TU1, TR>(EndpointDefinition, handlerFuncAsync);
        }
    }

    public class EndpointWithUsing<TU1, TU2> : EndpointBase
    {
        public EndpointWithUsing(EndpointDefinition endpointDefinition, EndpointHandlerUsingDefinition usingDefinition) : base(endpointDefinition)
        {
            EndpointDefinition.CurrentOrNewHandler.Usings.Add(usingDefinition);
        }

        public EndpointWithUsing<TU1, TU2, TU3> UsingModelFromBody<TU3>(string name = null)
        {
            return new EndpointWithUsing<TU1, TU2, TU3>(EndpointDefinition, new EndpointHandlerRouteParameterDefinition
            {
                Type = typeof(TU3),
                Name = name
            });
        }

        public EndpointWithResult<TU1, TU2, TR> HandledBy<TR>(Expression<Func<TU1, TU2, TR>> handlerFuncAsync)
        {
            return new EndpointWithResult<TU1, TU2, TR>(EndpointDefinition, handlerFuncAsync);
        }
    }

    public class EndpointWithUsing<TU1, TU2, TU3> : Endpoint
    {
        public EndpointWithUsing(EndpointDefinition endpointDefinition, EndpointHandlerServiceDefinition serviceDefinition) : base(endpointDefinition)
        {
            EndpointDefinition.CurrentOrNewHandler.Usings.Add(serviceDefinition);
        }

        public EndpointWithUsing(EndpointDefinition endpointDefinition, EndpointHandlerRouteParameterDefinition parameterDefinition) : base(endpointDefinition)
        {
            EndpointDefinition.CurrentOrNewHandler.Usings.Add(parameterDefinition);
        }

        public EndpointWithResult<TU1, TU2, TU3, TR> HandledBy<TR>(Expression<Func<TU1, TU2, TU3, TR>> handlerFuncAsync)
        {
            return new EndpointWithResult<TU1, TU2, TU3, TR>(EndpointDefinition, handlerFuncAsync);
        }
    }

    public class EndpointWithResult<TR> : Endpoint
    {
        public EndpointWithResult(EndpointDefinition endpointDefinition, Expression<Func<TR>> handlerFunc) : base(endpointDefinition)
        {
            EndpointDefinition.CurrentOrNewHandler.Func = handlerFunc;
            EndpointDefinition.CurrentOrNewHandler.ReturnType = typeof(TR);
        }
    }

    public class EndpointWithResult<TU1, TR> : Endpoint
    {
        public EndpointWithResult(EndpointDefinition endpointDefinition, Expression<Func<TU1, TR>> handlerFunc) : base(endpointDefinition)
        {
            EndpointDefinition.CurrentOrNewHandler.Func = handlerFunc;
            EndpointDefinition.CurrentOrNewHandler.ReturnType = typeof(TR);
        }
    }

    public class EndpointWithResult<TU1, TU2, TR> : Endpoint
    {
        public EndpointWithResult(EndpointDefinition endpointDefinition, Expression<Func<TU1, TU2, TR>> handlerFunc) : base(endpointDefinition)
        {
            EndpointDefinition.CurrentOrNewHandler.Func = handlerFunc;
            EndpointDefinition.CurrentOrNewHandler.ReturnType = typeof(TR);
        }
    }

    public class EndpointWithResult<TU1, TU2, TU3, TR> : Endpoint
    {
        public EndpointWithResult(EndpointDefinition endpointDefinition, Expression<Func<TU1, TU2, TU3, TR>> handlerFunc) : base(endpointDefinition)
        {
            EndpointDefinition.CurrentOrNewHandler.Func = handlerFunc;
            EndpointDefinition.CurrentOrNewHandler.ReturnType = typeof(TR);
        }
    }
}
