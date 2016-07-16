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

    public class EndpointHandlerParameterDefinition : EndpointHandlerUsingDefinition
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

    //public class EndpointUsing
    //{
    //    public Type Type { get; set; }
    //}

    //public enum EndpointDefinitionParameterKind
    //{
    //    Url,
    //    Body
    //}

    //public class EndpointDefinitionParameter
    //{
    //    public EndpointDefinitionParameterKind Kind { get; set; }

    //    public Type Type { get; set; }

    //    public string Name { get; set; }

    //    public string Description { get; set; }

    //    public EndpointDefinitionParameter(EndpointDefinitionParameterKind kind, Type type, string name = null, string description = null)
    //    {
    //        Kind = kind;
    //        Type = type;
    //        Name = name;
    //        Description = description;
    //    }
    //}

    //public class UsingEntity
    //{

    //}

    //public class UsingEntity<T> : UsingEntity
    //{

    //}

    //public class ServiceInjection<T> : UsingEntity<T>
    //{

    //}

    //public class ModelBinder<T> : UsingEntity<T>
    //{
    //}

    //public static class Inject
    //{
    //    public static ServiceInjection<T> Service<T>()
    //    {
    //        return new ServiceInjection<T>();
    //    }
    //}

    //public static class Bind
    //{
    //    public static ModelBinder<T> UrlParameter<T>(string name = null)
    //    {
    //        return new ModelBinder<T>();
    //    }

    //    public static ModelBinder<T> QueryStringParameter<T>(string name)
    //    {
    //        return new ModelBinder<T>();
    //    }

    //    public static ModelBinder<T> BodyParameter<T>(string name = null)
    //    {
    //        return new ModelBinder<T>();
    //    }
    //}


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
            return new EndpointWithUsing<TU1>(EndpointDefinition, new EndpointHandlerParameterDefinition
            {
                Type = typeof(TU1),
                Name = name
            });
        }

        //public EndpointWithUsing<TU1> Using<TU1>(UsingEntity<TU1> service1)
        //{
        //    return new EndpointWithUsing<TU1>(EndpointDefinition);
        //}

        //public EndpointWithUsing<TU1, TU2> Using<TU1, TU2>(UsingEntity<TU1> service1, UsingEntity<TU2> service2)
        //{
        //    return new EndpointWithUsing<TU1, TU2>(EndpointDefinition);
        //}

        //public EndpointWithUsing<TU1, TU2, TU3> Using<TU1, TU2, TU3>(UsingEntity<TU1> service1, UsingEntity<TU2> service2, UsingEntity<TU3> service3)
        //{
        //    return new EndpointWithUsing<TU1, TU2, TU3>(EndpointDefinition);
        //}
    }

    public class EndpointWithUsing<TU1> : EndpointBase
    {
        public EndpointWithUsing(EndpointDefinition endpointDefinition, EndpointHandlerServiceDefinition serviceDefinition) : base(endpointDefinition)
        {
            EndpointDefinition.CurrentOrNewHandler.Usings.Add(serviceDefinition);
        }

        public EndpointWithUsing(EndpointDefinition endpointDefinition, EndpointHandlerParameterDefinition parameterDefinition) : base(endpointDefinition)
        {
            EndpointDefinition.CurrentOrNewHandler.Usings.Add(parameterDefinition);
        }

        public EndpointWithUsing<TU1, TU2> UsingRouteParameter<TU2>(string name = null)
        {
            return new EndpointWithUsing<TU1, TU2>(EndpointDefinition, new EndpointHandlerParameterDefinition
            {
                Type = typeof(TU2),
                Name = name
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
        public EndpointWithUsing(EndpointDefinition endpointDefinition, EndpointHandlerServiceDefinition serviceDefinition) : base(endpointDefinition)
        {
            EndpointDefinition.CurrentOrNewHandler.Usings.Add(serviceDefinition);
        }

        public EndpointWithUsing(EndpointDefinition endpointDefinition, EndpointHandlerParameterDefinition parameterDefinition) : base(endpointDefinition)
        {
            EndpointDefinition.CurrentOrNewHandler.Usings.Add(parameterDefinition);
        }

        public EndpointWithUsing<TU1, TU2, TU3> UsingBodyParameter<TU3>(string name = null)
        {
            return new EndpointWithUsing<TU1, TU2, TU3>(EndpointDefinition, new EndpointHandlerParameterDefinition
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

        public EndpointWithUsing(EndpointDefinition endpointDefinition, EndpointHandlerParameterDefinition parameterDefinition) : base(endpointDefinition)
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

    //public class Endpoint_2P_U<TU, TP1, TP2>
    //{
    //    public readonly EndpointDefinition EndpointDefinition;

    //    public Endpoint_2P_U(EndpointDefinition endpointDefinition)
    //    {
    //        EndpointDefinition = endpointDefinition;
    //        EndpointDefinition.HandlerType = typeof(TU);
    //    }

    //    public Endpoint_2P_U_H_A<TU, TP1, TP2, THr> HandledByAsync<THr>(Func<WebHandlerContext, TU, TP1, TP2, Task<THr>> handlerFuncAsync)
    //    {
    //        return new Endpoint_2P_U_H_A<TU, TP1, TP2, THr>(EndpointDefinition, handlerFuncAsync);
    //    }
    //}

    //public class Endpoint_U_H<TU, THr>
    //{
    //    public readonly EndpointDefinition EndpointDefinition;

    //    public Endpoint_U_H(EndpointDefinition endpointDefinition, Func<WebHandlerContext, TU, THr> handlerFunc)
    //    {
    //        EndpointDefinition = endpointDefinition;
    //        EndpointDefinition.HandlerFunc = handlerFunc;
    //        EndpointDefinition.HandlerFuncReturnType = typeof(THr);
    //    }

    //    public EndpointDefinition Returns<TR2>(Func<WebHandlerContext, THr, TR2> returnFunc) where TR2 : IActionResult
    //    {
    //        EndpointDefinition.ReturnType = typeof(TR2);
    //        EndpointDefinition.ReturnFunc = returnFunc;

    //        return EndpointDefinition;
    //    }
    //}

    //public class Endpoint_U_H_A<TU, THr>
    //{
    //    public readonly EndpointDefinition EndpointDefinition;

    //    public Endpoint_U_H_A(EndpointDefinition endpointDefinition, Func<WebHandlerContext, TU, Task<THr>> handlerFunc)
    //    {
    //        EndpointDefinition = endpointDefinition;
    //        EndpointDefinition.HandlerFunc = handlerFunc;
    //        EndpointDefinition.HandlerFuncReturnType = typeof(THr);
    //    }

    //    public Endpoint Returns<TR>(Func<WebHandlerContext, THr, TR> returnFunc) where TR : IActionResult
    //    {
    //        EndpointDefinition.ReturnType = typeof(TR);
    //        EndpointDefinition.ReturnFunc = returnFunc;

    //        return new Endpoint(EndpointDefinition);
    //    }
    //}

    //public class Endpoint_U_H_1P_A<TU, TP1, THr>
    //{
    //    public readonly EndpointDefinition EndpointDefinition;

    //    public Endpoint_U_H_1P_A(EndpointDefinition endpointDefinition, Func<WebHandlerContext, TU, TP1, Task<THr>> handlerFunc)
    //    {
    //        EndpointDefinition = endpointDefinition;
    //        EndpointDefinition.HandlerFunc = handlerFunc;
    //        EndpointDefinition.HandlerFuncReturnType = typeof(THr);
    //    }

    //    public Endpoint Returns<TR>(Func<WebHandlerContext, TP1, THr, TR> returnFunc) where TR : IActionResult
    //    {
    //        EndpointDefinition.ReturnType = typeof(TR);
    //        EndpointDefinition.ReturnFunc = returnFunc;

    //        return new Endpoint(EndpointDefinition);
    //    }
    //}

    //public class Endpoint_2P_U_H_A<TU, TP1, TP2, THr>
    //{
    //    public readonly EndpointDefinition EndpointDefinition;

    //    public Endpoint_2P_U_H_A(EndpointDefinition endpointDefinition, Func<WebHandlerContext, TU, TP1, TP2, Task<THr>> handlerFunc)
    //    {
    //        EndpointDefinition = endpointDefinition;
    //        EndpointDefinition.HandlerFunc = handlerFunc;
    //        EndpointDefinition.HandlerFuncReturnType = typeof(THr);
    //    }

    //    public Endpoint Returns<TR>(Func<WebHandlerContext, TP1, TP2, THr, TR> returnFunc) where TR : IActionResult
    //    {
    //        EndpointDefinition.ReturnType = typeof(TR);
    //        EndpointDefinition.ReturnFunc = returnFunc;

    //        return new Endpoint(EndpointDefinition);
    //    }
    //}

    //public class Endpoint_1P_U_H<TU, THr, TP1>
    //{
    //    public readonly EndpointDefinition EndpointDefinition;

    //    public Endpoint_1P_U_H(EndpointDefinition endpointDefinition, Func<WebHandlerContext, TU, TP1, THr> handlerFunc)
    //    {
    //        EndpointDefinition = endpointDefinition;
    //        EndpointDefinition.HandlerFunc = handlerFunc;
    //        EndpointDefinition.HandlerFuncReturnType = typeof(THr);
    //    }

    //    public Endpoint Returns<TR2>(Func<WebHandlerContext, TP1, THr, TR2> returnFunc) where TR2 : IActionResult
    //    {
    //        EndpointDefinition.ReturnType = typeof(TR2);
    //        EndpointDefinition.ReturnFunc = returnFunc;

    //        return new Endpoint(EndpointDefinition);
    //    }
    //}

    //public class WebHandlerContext
    //{
    //    public IUserService Service<T>()
    //    {
    //        return new UserService();
    //    }
    //}

    //public static class EndpointExtensions
    //{
    //    public static Endpoint ReturnsJson<TU, THr>(this Endpoint_U_H_A<TU, THr> endpoint, Func<WebHandlerContext, THr, object> transformBeforeEncoding = null)
    //    {
    //        return endpoint.Returns((result) => new JsonResult(transformBeforeEncoding != null ? transformBeforeEncoding.Invoke(result) : result));
    //    }

    //    public static Endpoint ReturnsJson<TU, TP1, THr>(this Endpoint_U_H_1P_A<TU, TP1, THr> endpoint, Func<WebHandlerContext, TP1, THr, object> transformBeforeEncoding = null)
    //    {
    //        return endpoint.Returns((p1, result) => new JsonResult(transformBeforeEncoding != null ? transformBeforeEncoding.Invoke(p1, result) : result));
    //    }

    //    public static Endpoint ReturnsJson<TU, TP1, TP2, THr>(this Endpoint_2P_U_H_A<TU, TP1, TP2, THr> endpoint, Func<WebHandlerContext, TP1, TP2, THr, object> transformBeforeEncoding = null)
    //    {
    //        return endpoint.Returns((p1, p2, result) => new JsonResult(transformBeforeEncoding != null ? transformBeforeEncoding.Invoke(p1, p2, result) : result));
    //    }
    //}
}
