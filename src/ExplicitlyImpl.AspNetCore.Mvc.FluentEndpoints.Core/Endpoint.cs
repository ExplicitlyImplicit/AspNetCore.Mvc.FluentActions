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

    public class EndpointWithUsing<TU1> : EndpointBase
    {
        public EndpointWithUsing(EndpointDefinition endpointDefinition, EndpointUsingDefinition usingDefinition) : base(endpointDefinition)
        {
            EndpointDefinition.CurrentHandler.Usings.Add(usingDefinition);
        }

        public EndpointWithUsing<TU1, TU2> UsingService<TU2>()
        {
            return new EndpointWithUsing<TU1, TU2>(EndpointDefinition, new EndpointUsingServiceDefinition
            {
                Type = typeof(TU2)
            });
        }

        public EndpointWithUsing<TU1, TU2> UsingUrlParameter<TU2>(string name)
        {
            return new EndpointWithUsing<TU1, TU2>(EndpointDefinition, new EndpointUsingUrlParameterDefinition
            {
                Type = typeof(TU2),
                Name = name
            });
        }

        public EndpointWithUsing<TU1, TU2> UsingModelFromBody<TU2>()
        {
            return new EndpointWithUsing<TU1, TU2>(EndpointDefinition, new EndpointUsingModelFromBodyDefinition
            {
                Type = typeof(TU2)
            });
        }

        public EndpointWithResult<TR> HandledBy<TR>(Expression<Func<TU1, TR>> handlerFuncAsync)
        {
            return new EndpointWithResult<TR>(EndpointDefinition, handlerFuncAsync);
        }
    }

    public class EndpointWithUsing<TU1, TU2> : EndpointBase
    {
        public EndpointWithUsing(EndpointDefinition endpointDefinition, EndpointUsingDefinition usingDefinition) : base(endpointDefinition)
        {
            EndpointDefinition.CurrentHandler.Usings.Add(usingDefinition);
        }

        public EndpointWithUsing<TU1, TU2, TU3> UsingService<TU3>()
        {
            return new EndpointWithUsing<TU1, TU2, TU3>(EndpointDefinition, new EndpointUsingServiceDefinition
            {
                Type = typeof(TU3)
            });
        }

        public EndpointWithUsing<TU1, TU2, TU3> UsingUrlParameter<TU3>(string name)
        {
            return new EndpointWithUsing<TU1, TU2, TU3>(EndpointDefinition, new EndpointUsingUrlParameterDefinition
            {
                Type = typeof(TU3),
                Name = name
            });
        }

        public EndpointWithUsing<TU1, TU2, TU3> UsingModelFromBody<TU3>()
        {
            return new EndpointWithUsing<TU1, TU2, TU3>(EndpointDefinition, new EndpointUsingModelFromBodyDefinition
            {
                Type = typeof(TU3)
            });
        }

        public EndpointWithResult<TR> HandledBy<TR>(Expression<Func<TU1, TU2, TR>> handlerFuncAsync)
        {
            return new EndpointWithResult<TR>(EndpointDefinition, handlerFuncAsync);
        }
    }

    public class EndpointWithUsing<TU1, TU2, TU3> : EndpointBase
    {
        public EndpointWithUsing(EndpointDefinition endpointDefinition, EndpointUsingDefinition usingDefinition) : base(endpointDefinition)
        {
            EndpointDefinition.CurrentHandler.Usings.Add(usingDefinition);
        }

        public EndpointWithUsing<TU1, TU2, TU3, TU4> UsingService<TU4>()
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4>(EndpointDefinition, new EndpointUsingServiceDefinition
            {
                Type = typeof(TU4)
            });
        }

        public EndpointWithUsing<TU1, TU2, TU3, TU4> UsingUrlParameter<TU4>(string name)
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4>(EndpointDefinition, new EndpointUsingUrlParameterDefinition
            {
                Type = typeof(TU4),
                Name = name
            });
        }

        public EndpointWithUsing<TU1, TU2, TU3, TU4> UsingModelFromBody<TU4>()
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4>(EndpointDefinition, new EndpointUsingModelFromBodyDefinition
            {
                Type = typeof(TU4)
            });
        }

        public EndpointWithResult<TR> HandledBy<TR>(Expression<Func<TU1, TU2, TU3, TR>> handlerFuncAsync)
        {
            return new EndpointWithResult<TR>(EndpointDefinition, handlerFuncAsync);
        }
    }


    public class EndpointWithUsing<TU1, TU2, TU3, TU4> : EndpointBase
    {
        public EndpointWithUsing(EndpointDefinition endpointDefinition, EndpointUsingDefinition usingDefinition) : base(endpointDefinition)
        {
            EndpointDefinition.CurrentHandler.Usings.Add(usingDefinition);
        }

        public EndpointWithUsing<TU1, TU2, TU3, TU4, TU5> UsingService<TU5>()
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4, TU5>(EndpointDefinition, new EndpointUsingServiceDefinition
            {
                Type = typeof(TU5)
            });
        }

        public EndpointWithUsing<TU1, TU2, TU3, TU4, TU5> UsingUrlParameter<TU5>(string name)
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4, TU5>(EndpointDefinition, new EndpointUsingUrlParameterDefinition
            {
                Type = typeof(TU5),
                Name = name
            });
        }

        public EndpointWithUsing<TU1, TU2, TU3, TU4, TU5> UsingModelFromBody<TU5>()
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4, TU5>(EndpointDefinition, new EndpointUsingModelFromBodyDefinition
            {
                Type = typeof(TU5)
            });
        }

        public EndpointWithResult<TR> HandledBy<TR>(Expression<Func<TU1, TU2, TU3, TU4, TR>> handlerFuncAsync)
        {
            return new EndpointWithResult<TR>(EndpointDefinition, handlerFuncAsync);
        }
    }

    public class EndpointWithUsing<TU1, TU2, TU3, TU4, TU5> : EndpointBase
    {
        public EndpointWithUsing(EndpointDefinition endpointDefinition, EndpointUsingDefinition usingDefinition) : base(endpointDefinition)
        {
            EndpointDefinition.CurrentHandler.Usings.Add(usingDefinition);
        }

        public EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6> UsingService<TU6>()
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6>(EndpointDefinition, new EndpointUsingServiceDefinition
            {
                Type = typeof(TU6)
            });
        }

        public EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6> UsingUrlParameter<TU6>(string name)
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6>(EndpointDefinition, new EndpointUsingUrlParameterDefinition
            {
                Type = typeof(TU6),
                Name = name
            });
        }

        public EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6> UsingModelFromBody<TU6>()
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6>(EndpointDefinition, new EndpointUsingModelFromBodyDefinition
            {
                Type = typeof(TU6)
            });
        }

        public EndpointWithResult<TR> HandledBy<TR>(Expression<Func<TU1, TU2, TU3, TU4, TU5, TR>> handlerFuncAsync)
        {
            return new EndpointWithResult<TR>(EndpointDefinition, handlerFuncAsync);
        }
    }

    public class EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6> : EndpointBase
    {
        public EndpointWithUsing(EndpointDefinition endpointDefinition, EndpointUsingDefinition usingDefinition) : base(endpointDefinition)
        {
            EndpointDefinition.CurrentHandler.Usings.Add(usingDefinition);
        }

        public EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7> UsingService<TU7>()
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7>(EndpointDefinition, new EndpointUsingServiceDefinition
            {
                Type = typeof(TU7)
            });
        }

        public EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7> UsingUrlParameter<TU7>(string name)
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7>(EndpointDefinition, new EndpointUsingUrlParameterDefinition
            {
                Type = typeof(TU7),
                Name = name
            });
        }

        public EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7> UsingModelFromBody<TU7>()
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7>(EndpointDefinition, new EndpointUsingModelFromBodyDefinition
            {
                Type = typeof(TU7)
            });
        }

        public EndpointWithResult<TR> HandledBy<TR>(Expression<Func<TU1, TU2, TU3, TU4, TU5, TU6, TR>> handlerFuncAsync)
        {
            return new EndpointWithResult<TR>(EndpointDefinition, handlerFuncAsync);
        }
    }

    public class EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7> : EndpointBase
    {
        public EndpointWithUsing(EndpointDefinition endpointDefinition, EndpointUsingDefinition usingDefinition) : base(endpointDefinition)
        {
            EndpointDefinition.CurrentHandler.Usings.Add(usingDefinition);
        }

        public EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> UsingService<TU8>()
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(EndpointDefinition, new EndpointUsingServiceDefinition
            {
                Type = typeof(TU8)
            });
        }

        public EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> UsingUrlParameter<TU8>(string name)
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(EndpointDefinition, new EndpointUsingUrlParameterDefinition
            {
                Type = typeof(TU8),
                Name = name
            });
        }

        public EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> UsingModelFromBody<TU8>()
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(EndpointDefinition, new EndpointUsingModelFromBodyDefinition
            {
                Type = typeof(TU8)
            });
        }

        public EndpointWithResult<TR> HandledBy<TR>(Expression<Func<TU1, TU2, TU3, TU4, TU5, TU6, TU7, TR>> handlerFuncAsync)
        {
            return new EndpointWithResult<TR>(EndpointDefinition, handlerFuncAsync);
        }
    }

    public class EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> : Endpoint
    {
        public EndpointWithUsing(EndpointDefinition endpointDefinition, EndpointUsingDefinition usingDefinition) : base(endpointDefinition)
        {
            EndpointDefinition.CurrentHandler.Usings.Add(usingDefinition);
        }

        public EndpointWithResult<TR> HandledBy<TR>(Expression<Func<TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8, TR>> handlerFuncAsync)
        {
            return new EndpointWithResult<TR>(EndpointDefinition, handlerFuncAsync);
        }
    }

    public class EndpointWithResult<TR> : Endpoint
    {
        public EndpointWithResult(EndpointDefinition endpointDefinition, LambdaExpression handlerFunc) : base(endpointDefinition)
        {
            EndpointDefinition.CurrentHandler.Func = handlerFunc;
            EndpointDefinition.CurrentHandler.ReturnType = typeof(TR);
        }
    }
}
