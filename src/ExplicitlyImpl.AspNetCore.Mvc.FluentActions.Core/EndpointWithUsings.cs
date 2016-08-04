using Microsoft.AspNetCore.Http;
using System;
using System.Linq.Expressions;

// ReSharper disable InconsistentNaming

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentActions
{
    public class EndpointWithUsing<TU1> : FluentActionBase
    {
        public EndpointWithUsing(FluentActionDefinition endpointDefinition, EndpointUsingDefinition usingDefinition) : base(endpointDefinition)
        {
            Definition.CurrentHandler.Usings.Add(usingDefinition);
        }

        public virtual EndpointWithUsing<TU1, TU2> Using<TU2>(EndpointUsingDefinition usingDefinition)
        {
            return new EndpointWithUsing<TU1, TU2>(Definition, usingDefinition);
        }

        public EndpointWithUsing<TU1, TU2> UsingService<TU2>()
        {
            return new EndpointWithUsing<TU1, TU2>(Definition, new EndpointUsingServiceDefinition
            {
                Type = typeof(TU2)
            });
        }

        public EndpointWithUsing<TU1, TU2> UsingRouteParameter<TU2>(string name)
        {
            return new EndpointWithUsing<TU1, TU2>(Definition, new EndpointUsingRouteParameterDefinition
            {
                Type = typeof(TU2),
                Name = name
            });
        }

        public virtual EndpointWithUsing<TU1, TU2> UsingQueryStringParameter<TU2>(string name)
        {
            return new EndpointWithUsing<TU1, TU2>(Definition, new EndpointUsingQueryStringParameterDefinition
            {
                Type = typeof(TU2),
                Name = name
            });
        }

        public virtual EndpointWithUsing<TU1, TU2> UsingHeader<TU2>(string name)
        {
            return new EndpointWithUsing<TU1, TU2>(Definition, new EndpointUsingHeaderParameterDefinition
            {
                Type = typeof(TU2),
                Name = name
            });
        }

        public EndpointWithUsing<TU1, TU2> UsingBody<TU2>()
        {
            return new EndpointWithUsing<TU1, TU2>(Definition, new EndpointUsingBodyDefinition
            {
                Type = typeof(TU2)
            });
        }

        public virtual EndpointWithUsing<TU1, TU2> UsingForm<TU2>()
        {
            return new EndpointWithUsing<TU1, TU2>(Definition, new EndpointUsingFormDefinition
            {
                Type = typeof(TU2)
            });
        }

        public virtual EndpointWithUsing<TU1, TU2> UsingFormValue<TU2>(string key)
        {
            return new EndpointWithUsing<TU1, TU2>(Definition, new EndpointUsingFormValueDefinition
            {
                Type = typeof(TU2),
                Key = key
            });
        }

        public virtual EndpointWithUsing<TU1, TU2> UsingModelBinder<TU2>(Type modelBinderType)
        {
            return new EndpointWithUsing<TU1, TU2>(Definition, new EndpointUsingModelBinderDefinition
            {
                Type = typeof(TU2),
                ModelBinderType = modelBinderType
            });
        }

        public virtual EndpointWithUsing<TU1, HttpContext> UsingHttpContext()
        {
            return new EndpointWithUsing<TU1, HttpContext>(Definition, new EndpointUsingHttpContextDefinition
            {
                Type = typeof(HttpContext)
            });
        }

        public EndpointWithResult<TR> HandledBy<TR>(Func<TU1, TR> handlerFuncAsync)
        {
            return new EndpointWithResult<TR>(Definition, handlerFuncAsync);
        }
    }

    public class EndpointWithUsing<TU1, TU2> : FluentActionBase
    {
        public EndpointWithUsing(FluentActionDefinition endpointDefinition, EndpointUsingDefinition usingDefinition) : base(endpointDefinition)
        {
            Definition.CurrentHandler.Usings.Add(usingDefinition);
        }

        public virtual EndpointWithUsing<TU1, TU2, TU3> Using<TU3>(EndpointUsingDefinition usingDefinition)
        {
            return new EndpointWithUsing<TU1, TU2, TU3>(Definition, usingDefinition);
        }

        public EndpointWithUsing<TU1, TU2, TU3> UsingService<TU3>()
        {
            return new EndpointWithUsing<TU1, TU2, TU3>(Definition, new EndpointUsingServiceDefinition
            {
                Type = typeof(TU3)
            });
        }

        public EndpointWithUsing<TU1, TU2, TU3> UsingRouteParameter<TU3>(string name)
        {
            return new EndpointWithUsing<TU1, TU2, TU3>(Definition, new EndpointUsingRouteParameterDefinition
            {
                Type = typeof(TU3),
                Name = name
            });
        }

        public virtual EndpointWithUsing<TU1, TU2, TU3> UsingQueryStringParameter<TU3>(string name)
        {
            return new EndpointWithUsing<TU1, TU2, TU3>(Definition, new EndpointUsingQueryStringParameterDefinition
            {
                Type = typeof(TU3),
                Name = name
            });
        }

        public virtual EndpointWithUsing<TU1, TU2, TU3> UsingHeader<TU3>(string name)
        {
            return new EndpointWithUsing<TU1, TU2, TU3>(Definition, new EndpointUsingHeaderParameterDefinition
            {
                Type = typeof(TU3),
                Name = name
            });
        }

        public EndpointWithUsing<TU1, TU2, TU3> UsingBody<TU3>()
        {
            return new EndpointWithUsing<TU1, TU2, TU3>(Definition, new EndpointUsingBodyDefinition
            {
                Type = typeof(TU3)
            });
        }

        public virtual EndpointWithUsing<TU1, TU2, TU3> UsingForm<TU3>()
        {
            return new EndpointWithUsing<TU1, TU2, TU3>(Definition, new EndpointUsingFormDefinition
            {
                Type = typeof(TU3)
            });
        }

        public virtual EndpointWithUsing<TU1, TU2, TU3> UsingFormValue<TU3>(string key)
        {
            return new EndpointWithUsing<TU1, TU2, TU3>(Definition, new EndpointUsingFormValueDefinition
            {
                Type = typeof(TU3),
                Key = key
            });
        }

        public virtual EndpointWithUsing<TU1, TU2, TU3> UsingModelBinder<TU3>(Type modelBinderType)
        {
            return new EndpointWithUsing<TU1, TU2, TU3>(Definition, new EndpointUsingModelBinderDefinition
            {
                Type = typeof(TU3),
                ModelBinderType = modelBinderType
            });
        }

        public virtual EndpointWithUsing<TU1, TU2, HttpContext> UsingHttpContext()
        {
            return new EndpointWithUsing<TU1, TU2, HttpContext>(Definition, new EndpointUsingHttpContextDefinition
            {
                Type = typeof(HttpContext)
            });
        }

        public EndpointWithResult<TR> HandledBy<TR>(Func<TU1, TU2, TR> handlerFuncAsync)
        {
            return new EndpointWithResult<TR>(Definition, handlerFuncAsync);
        }
    }

    public class EndpointWithUsing<TU1, TU2, TU3> : FluentActionBase
    {
        public EndpointWithUsing(FluentActionDefinition endpointDefinition, EndpointUsingDefinition usingDefinition) : base(endpointDefinition)
        {
            Definition.CurrentHandler.Usings.Add(usingDefinition);
        }

        public virtual EndpointWithUsing<TU1, TU2, TU3, TU4> Using<TU4>(EndpointUsingDefinition usingDefinition)
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4>(Definition, usingDefinition);
        }

        public EndpointWithUsing<TU1, TU2, TU3, TU4> UsingService<TU4>()
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4>(Definition, new EndpointUsingServiceDefinition
            {
                Type = typeof(TU4)
            });
        }

        public EndpointWithUsing<TU1, TU2, TU3, TU4> UsingRouteParameter<TU4>(string name)
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4>(Definition, new EndpointUsingRouteParameterDefinition
            {
                Type = typeof(TU4),
                Name = name
            });
        }

        public virtual EndpointWithUsing<TU1, TU2, TU3, TU4> UsingQueryStringParameter<TU4>(string name)
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4>(Definition, new EndpointUsingQueryStringParameterDefinition
            {
                Type = typeof(TU4),
                Name = name
            });
        }

        public virtual EndpointWithUsing<TU1, TU2, TU3, TU4> UsingHeader<TU4>(string name)
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4>(Definition, new EndpointUsingHeaderParameterDefinition
            {
                Type = typeof(TU4),
                Name = name
            });
        }

        public EndpointWithUsing<TU1, TU2, TU3, TU4> UsingBody<TU4>()
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4>(Definition, new EndpointUsingBodyDefinition
            {
                Type = typeof(TU4)
            });
        }

        public virtual EndpointWithUsing<TU1, TU2, TU3, TU4> UsingForm<TU4>()
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4>(Definition, new EndpointUsingFormDefinition
            {
                Type = typeof(TU4)
            });
        }

        public virtual EndpointWithUsing<TU1, TU2, TU3, TU4> UsingFormValue<TU4>(string key)
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4>(Definition, new EndpointUsingFormValueDefinition
            {
                Type = typeof(TU4),
                Key = key
            });
        }

        public virtual EndpointWithUsing<TU1, TU2, TU3, TU4> UsingModelBinder<TU4>(Type modelBinderType)
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4>(Definition, new EndpointUsingModelBinderDefinition
            {
                Type = typeof(TU4),
                ModelBinderType = modelBinderType
            });
        }

        public virtual EndpointWithUsing<TU1, TU2, TU3, HttpContext> UsingHttpContext()
        {
            return new EndpointWithUsing<TU1, TU2, TU3, HttpContext>(Definition, new EndpointUsingHttpContextDefinition
            {
                Type = typeof(HttpContext)
            });
        }

        public EndpointWithResult<TR> HandledBy<TR>(Func<TU1, TU2, TU3, TR> handlerFuncAsync)
        {
            return new EndpointWithResult<TR>(Definition, handlerFuncAsync);
        }
    }

    public class EndpointWithUsing<TU1, TU2, TU3, TU4> : FluentActionBase
    {
        public EndpointWithUsing(FluentActionDefinition endpointDefinition, EndpointUsingDefinition usingDefinition) : base(endpointDefinition)
        {
            Definition.CurrentHandler.Usings.Add(usingDefinition);
        }

        public virtual EndpointWithUsing<TU1, TU2, TU3, TU4, TU5> Using<TU5>(EndpointUsingDefinition usingDefinition)
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4, TU5>(Definition, usingDefinition);
        }

        public EndpointWithUsing<TU1, TU2, TU3, TU4, TU5> UsingService<TU5>()
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4, TU5>(Definition, new EndpointUsingServiceDefinition
            {
                Type = typeof(TU5)
            });
        }

        public EndpointWithUsing<TU1, TU2, TU3, TU4, TU5> UsingRouteParameter<TU5>(string name)
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4, TU5>(Definition, new EndpointUsingRouteParameterDefinition
            {
                Type = typeof(TU5),
                Name = name
            });
        }

        public virtual EndpointWithUsing<TU1, TU2, TU3, TU4, TU5> UsingQueryStringParameter<TU5>(string name)
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4, TU5>(Definition, new EndpointUsingQueryStringParameterDefinition
            {
                Type = typeof(TU5),
                Name = name
            });
        }

        public virtual EndpointWithUsing<TU1, TU2, TU3, TU4, TU5> UsingHeader<TU5>(string name)
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4, TU5>(Definition, new EndpointUsingHeaderParameterDefinition
            {
                Type = typeof(TU5),
                Name = name
            });
        }

        public EndpointWithUsing<TU1, TU2, TU3, TU4, TU5> UsingBody<TU5>()
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4, TU5>(Definition, new EndpointUsingBodyDefinition
            {
                Type = typeof(TU5)
            });
        }

        public virtual EndpointWithUsing<TU1, TU2, TU3, TU4, TU5> UsingForm<TU5>()
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4, TU5>(Definition, new EndpointUsingFormDefinition
            {
                Type = typeof(TU5)
            });
        }

        public virtual EndpointWithUsing<TU1, TU2, TU3, TU4, TU5> UsingFormValue<TU5>(string key)
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4, TU5>(Definition, new EndpointUsingFormValueDefinition
            {
                Type = typeof(TU5),
                Key = key
            });
        }

        public virtual EndpointWithUsing<TU1, TU2, TU3, TU4, TU5> UsingModelBinder<TU5>(Type modelBinderType)
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4, TU5>(Definition, new EndpointUsingModelBinderDefinition
            {
                Type = typeof(TU5),
                ModelBinderType = modelBinderType
            });
        }

        public virtual EndpointWithUsing<TU1, TU2, TU3, TU4, HttpContext> UsingHttpContext()
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4, HttpContext>(Definition, new EndpointUsingHttpContextDefinition
            {
                Type = typeof(HttpContext)
            });
        }

        public EndpointWithResult<TR> HandledBy<TR>(Func<TU1, TU2, TU3, TU4, TR> handlerFuncAsync)
        {
            return new EndpointWithResult<TR>(Definition, handlerFuncAsync);
        }
    }

    public class EndpointWithUsing<TU1, TU2, TU3, TU4, TU5> : FluentActionBase
    {
        public EndpointWithUsing(FluentActionDefinition endpointDefinition, EndpointUsingDefinition usingDefinition) : base(endpointDefinition)
        {
            Definition.CurrentHandler.Usings.Add(usingDefinition);
        }

        public virtual EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6> Using<TU6>(EndpointUsingDefinition usingDefinition)
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6>(Definition, usingDefinition);
        }

        public EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6> UsingService<TU6>()
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6>(Definition, new EndpointUsingServiceDefinition
            {
                Type = typeof(TU6)
            });
        }

        public EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6> UsingRouteParameter<TU6>(string name)
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6>(Definition, new EndpointUsingRouteParameterDefinition
            {
                Type = typeof(TU6),
                Name = name
            });
        }

        public virtual EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6> UsingQueryStringParameter<TU6>(string name)
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6>(Definition, new EndpointUsingQueryStringParameterDefinition
            {
                Type = typeof(TU6),
                Name = name
            });
        }

        public virtual EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6> UsingHeader<TU6>(string name)
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6>(Definition, new EndpointUsingHeaderParameterDefinition
            {
                Type = typeof(TU6),
                Name = name
            });
        }

        public EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6> UsingBody<TU6>()
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6>(Definition, new EndpointUsingBodyDefinition
            {
                Type = typeof(TU6)
            });
        }

        public virtual EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6> UsingForm<TU6>()
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6>(Definition, new EndpointUsingFormDefinition
            {
                Type = typeof(TU6)
            });
        }

        public virtual EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6> UsingFormValue<TU6>(string key)
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6>(Definition, new EndpointUsingFormValueDefinition
            {
                Type = typeof(TU6),
                Key = key
            });
        }

        public virtual EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6> UsingModelBinder<TU6>(Type modelBinderType)
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6>(Definition, new EndpointUsingModelBinderDefinition
            {
                Type = typeof(TU6),
                ModelBinderType = modelBinderType
            });
        }

        public virtual EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, HttpContext> UsingHttpContext()
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, HttpContext>(Definition, new EndpointUsingHttpContextDefinition
            {
                Type = typeof(HttpContext)
            });
        }

        public EndpointWithResult<TR> HandledBy<TR>(Func<TU1, TU2, TU3, TU4, TU5, TR> handlerFuncAsync)
        {
            return new EndpointWithResult<TR>(Definition, handlerFuncAsync);
        }
    }

    public class EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6> : FluentActionBase
    {
        public EndpointWithUsing(FluentActionDefinition endpointDefinition, EndpointUsingDefinition usingDefinition) : base(endpointDefinition)
        {
            Definition.CurrentHandler.Usings.Add(usingDefinition);
        }

        public virtual EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7> Using<TU7>(EndpointUsingDefinition usingDefinition)
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7>(Definition, usingDefinition);
        }

        public EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7> UsingService<TU7>()
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7>(Definition, new EndpointUsingServiceDefinition
            {
                Type = typeof(TU7)
            });
        }

        public EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7> UsingRouteParameter<TU7>(string name)
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7>(Definition, new EndpointUsingRouteParameterDefinition
            {
                Type = typeof(TU7),
                Name = name
            });
        }

        public virtual EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7> UsingQueryStringParameter<TU7>(string name)
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7>(Definition, new EndpointUsingQueryStringParameterDefinition
            {
                Type = typeof(TU7),
                Name = name
            });
        }

        public virtual EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7> UsingHeader<TU7>(string name)
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7>(Definition, new EndpointUsingHeaderParameterDefinition
            {
                Type = typeof(TU7),
                Name = name
            });
        }

        public EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7> UsingBody<TU7>()
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7>(Definition, new EndpointUsingBodyDefinition
            {
                Type = typeof(TU7)
            });
        }

        public virtual EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7> UsingForm<TU7>()
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7>(Definition, new EndpointUsingFormDefinition
            {
                Type = typeof(TU7)
            });
        }

        public virtual EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7> UsingFormValue<TU7>(string key)
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7>(Definition, new EndpointUsingFormValueDefinition
            {
                Type = typeof(TU7),
                Key = key
            });
        }

        public virtual EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7> UsingModelBinder<TU7>(Type modelBinderType)
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7>(Definition, new EndpointUsingModelBinderDefinition
            {
                Type = typeof(TU7),
                ModelBinderType = modelBinderType
            });
        }

        public virtual EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, HttpContext> UsingHttpContext()
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, HttpContext>(Definition, new EndpointUsingHttpContextDefinition
            {
                Type = typeof(HttpContext)
            });
        }

        public EndpointWithResult<TR> HandledBy<TR>(Func<TU1, TU2, TU3, TU4, TU5, TU6, TR> handlerFuncAsync)
        {
            return new EndpointWithResult<TR>(Definition, handlerFuncAsync);
        }
    }

    public class EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7> : FluentActionBase
    {
        public EndpointWithUsing(FluentActionDefinition endpointDefinition, EndpointUsingDefinition usingDefinition) : base(endpointDefinition)
        {
            Definition.CurrentHandler.Usings.Add(usingDefinition);
        }

        public virtual EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> Using<TU8>(EndpointUsingDefinition usingDefinition)
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(Definition, usingDefinition);
        }

        public EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> UsingService<TU8>()
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(Definition, new EndpointUsingServiceDefinition
            {
                Type = typeof(TU8)
            });
        }

        public EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> UsingRouteParameter<TU8>(string name)
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(Definition, new EndpointUsingRouteParameterDefinition
            {
                Type = typeof(TU8),
                Name = name
            });
        }

        public virtual EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> UsingQueryStringParameter<TU8>(string name)
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(Definition, new EndpointUsingQueryStringParameterDefinition
            {
                Type = typeof(TU8),
                Name = name
            });
        }

        public virtual EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> UsingHeader<TU8>(string name)
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(Definition, new EndpointUsingHeaderParameterDefinition
            {
                Type = typeof(TU8),
                Name = name
            });
        }

        public EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> UsingBody<TU8>()
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(Definition, new EndpointUsingBodyDefinition
            {
                Type = typeof(TU8)
            });
        }

        public virtual EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> UsingForm<TU8>()
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(Definition, new EndpointUsingFormDefinition
            {
                Type = typeof(TU8)
            });
        }

        public virtual EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> UsingFormValue<TU8>(string key)
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(Definition, new EndpointUsingFormValueDefinition
            {
                Type = typeof(TU8),
                Key = key
            });
        }

        public virtual EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> UsingModelBinder<TU8>(Type modelBinderType)
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(Definition, new EndpointUsingModelBinderDefinition
            {
                Type = typeof(TU8),
                ModelBinderType = modelBinderType
            });
        }

        public virtual EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7, HttpContext> UsingHttpContext()
        {
            return new EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7, HttpContext>(Definition, new EndpointUsingHttpContextDefinition
            {
                Type = typeof(HttpContext)
            });
        }

        public EndpointWithResult<TR> HandledBy<TR>(Func<TU1, TU2, TU3, TU4, TU5, TU6, TU7, TR> handlerFuncAsync)
        {
            return new EndpointWithResult<TR>(Definition, handlerFuncAsync);
        }
    }

    public class EndpointWithUsing<TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> : FluentAction
    {
        public EndpointWithUsing(FluentActionDefinition endpointDefinition, EndpointUsingDefinition usingDefinition) : base(endpointDefinition)
        {
            Definition.CurrentHandler.Usings.Add(usingDefinition);
        }

        public EndpointWithResult<TR> HandledBy<TR>(Func<TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8, TR> handlerFuncAsync)
        {
            return new EndpointWithResult<TR>(Definition, handlerFuncAsync);
        }
    }
}
