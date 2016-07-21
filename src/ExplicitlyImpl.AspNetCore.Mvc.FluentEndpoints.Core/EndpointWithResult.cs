﻿using System;

// ReSharper disable InconsistentNaming

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentEndpoints
{
    public class EndpointWithResult<TR> : EndpointBase
    {
        public EndpointWithResult(EndpointDefinition endpointDefinition, Delegate handlerFunc) : base(endpointDefinition)
        {
            var returnType = typeof(TR);

            EndpointDefinition.CurrentHandler.Delegate = handlerFunc;
            EndpointDefinition.CurrentHandler.ReturnType = returnType.IsAnonymous() ? typeof(object) : returnType;
        }

        public virtual EndpointWithResultAndUsing<TR, TU1> Using<TU1>(EndpointUsingDefinition usingDefinition)
        {
            return new EndpointWithResultAndUsing<TR, TU1>(EndpointDefinition, usingDefinition);
        }

        public virtual EndpointWithResultAndUsing<TR, TR> UsingResultFromHandler()
        {
            return new EndpointWithResultAndUsing<TR, TR>(EndpointDefinition, new EndpointUsingResultFromHandlerDefinition
            {
                Type = typeof(TR)
            });
        }

        public virtual EndpointWithResultAndUsing<TR, TU1> UsingService<TU1>()
        {
            return new EndpointWithResultAndUsing<TR, TU1>(EndpointDefinition, new EndpointUsingServiceDefinition
            {
                Type = typeof(TU1)
            });
        }

        public virtual EndpointWithResultAndUsing<TR, TU1> UsingRouteParameter<TU1>(string name)
        {
            return new EndpointWithResultAndUsing<TR, TU1>(EndpointDefinition, new EndpointUsingRouteParameterDefinition
            {
                Type = typeof(TU1),
                Name = name
            });
        }

        public virtual EndpointWithResultAndUsing<TR, TU1> UsingQueryStringParameter<TU1>(string name)
        {
            return new EndpointWithResultAndUsing<TR, TU1>(EndpointDefinition, new EndpointUsingQueryStringParameterDefinition
            {
                Type = typeof(TU1),
                Name = name
            });
        }

        public virtual EndpointWithResultAndUsing<TR, TU1> UsingHeader<TU1>(string name)
        {
            return new EndpointWithResultAndUsing<TR, TU1>(EndpointDefinition, new EndpointUsingHeaderParameterDefinition
            {
                Type = typeof(TU1),
                Name = name
            });
        }

        public virtual EndpointWithResultAndUsing<TR, TU1> UsingBody<TU1>()
        {
            return new EndpointWithResultAndUsing<TR, TU1>(EndpointDefinition, new EndpointUsingBodyDefinition
            {
                Type = typeof(TU1)
            });
        }

        public virtual EndpointWithResultAndUsing<TR, TU1> UsingForm<TU1>()
        {
            return new EndpointWithResultAndUsing<TR, TU1>(EndpointDefinition, new EndpointUsingFormDefinition
            {
                Type = typeof(TU1)
            });
        }

        public virtual EndpointWithResultAndUsing<TR, TU1> UsingFormValue<TU1>(string key)
        {
            return new EndpointWithResultAndUsing<TR, TU1>(EndpointDefinition, new EndpointUsingFormValueDefinition
            {
                Type = typeof(TU1),
                Key = key
            });
        }

        public virtual EndpointWithResultAndUsing<TR, TU1> UsingModelBinder<TU1>(Type modelBinderType)
        {
            return new EndpointWithResultAndUsing<TR, TU1>(EndpointDefinition, new EndpointUsingModelBinderDefinition
            {
                Type = typeof(TU1),
                ModelBinderType = modelBinderType
            });
        }

        public EndpointWithResult<TR2> HandledBy<TR2>(Func<TR2> handlerFuncAsync)
        {
            return new EndpointWithResult<TR2>(EndpointDefinition, handlerFuncAsync);
        }
    }

    public class EndpointWithResultAndUsing<TR, TU1> : EndpointBase
    {
        public EndpointWithResultAndUsing(EndpointDefinition endpointDefinition, EndpointUsingDefinition usingDefinition) : base(endpointDefinition)
        {
            EndpointDefinition.Handlers.Add(new EndpointHandlerDefinition());
            EndpointDefinition.CurrentHandler.Usings.Add(usingDefinition);
        }

        public virtual EndpointWithResultAndUsing<TR, TU1, TU2> Using<TU2>(EndpointUsingDefinition usingDefinition)
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2>(EndpointDefinition, usingDefinition);
        }

        public virtual EndpointWithResultAndUsing<TR, TU1, TR> UsingResultFromHandler()
        {
            return new EndpointWithResultAndUsing<TR, TU1, TR>(EndpointDefinition, new EndpointUsingResultFromHandlerDefinition
            {
                Type = typeof(TR)
            });
        }

        public EndpointWithResultAndUsing<TR, TU1, TU2> UsingService<TU2>()
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2>(EndpointDefinition, new EndpointUsingServiceDefinition
            {
                Type = typeof(TU2)
            });
        }

        public EndpointWithResultAndUsing<TR, TU1, TU2> UsingRouteParameter<TU2>(string name)
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2>(EndpointDefinition, new EndpointUsingRouteParameterDefinition
            {
                Type = typeof(TU2),
                Name = name
            });
        }

        public virtual EndpointWithResultAndUsing<TR, TU1, TU2> UsingQueryStringParameter<TU2>(string name)
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2>(EndpointDefinition, new EndpointUsingQueryStringParameterDefinition
            {
                Type = typeof(TU2),
                Name = name
            });
        }

        public virtual EndpointWithResultAndUsing<TR, TU1, TU2> UsingHeader<TU2>(string name)
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2>(EndpointDefinition, new EndpointUsingHeaderParameterDefinition
            {
                Type = typeof(TU2),
                Name = name
            });
        }

        public EndpointWithResultAndUsing<TR, TU1, TU2> UsingBody<TU2>()
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2>(EndpointDefinition, new EndpointUsingBodyDefinition
            {
                Type = typeof(TU2)
            });
        }

        public virtual EndpointWithResultAndUsing<TR, TU1, TU2> UsingForm<TU2>()
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2>(EndpointDefinition, new EndpointUsingFormDefinition
            {
                Type = typeof(TU2)
            });
        }

        public virtual EndpointWithResultAndUsing<TR, TU1, TU2> UsingFormValue<TU2>(string key)
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2>(EndpointDefinition, new EndpointUsingFormValueDefinition
            {
                Type = typeof(TU2),
                Key = key
            });
        }

        public virtual EndpointWithResultAndUsing<TR, TU1, TU2> UsingModelBinder<TU2>(Type modelBinderType)
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2>(EndpointDefinition, new EndpointUsingModelBinderDefinition
            {
                Type = typeof(TU2),
                ModelBinderType = modelBinderType
            });
        }

        public EndpointWithResult<TR2> HandledBy<TR2>(Func<TU1, TR2> handlerFuncAsync)
        {
            return new EndpointWithResult<TR2>(EndpointDefinition, handlerFuncAsync);
        }
    }

    public class EndpointWithResultAndUsing<TR, TU1, TU2> : EndpointBase
    {
        public EndpointWithResultAndUsing(EndpointDefinition endpointDefinition, EndpointUsingDefinition usingDefinition) : base(endpointDefinition)
        {
            EndpointDefinition.CurrentHandler.Usings.Add(usingDefinition);
        }

        public virtual EndpointWithResultAndUsing<TR, TU1, TU2, TU3> Using<TU3>(EndpointUsingDefinition usingDefinition)
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3>(EndpointDefinition, usingDefinition);
        }

        public virtual EndpointWithResultAndUsing<TR, TU1, TU2, TR> UsingResultFromHandler()
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TR>(EndpointDefinition, new EndpointUsingResultFromHandlerDefinition
            {
                Type = typeof(TR)
            });
        }

        public EndpointWithResultAndUsing<TR, TU1, TU2, TU3> UsingService<TU3>()
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3>(EndpointDefinition, new EndpointUsingServiceDefinition
            {
                Type = typeof(TU3)
            });
        }

        public EndpointWithResultAndUsing<TR, TU1, TU2, TU3> UsingRouteParameter<TU3>(string name)
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3>(EndpointDefinition, new EndpointUsingRouteParameterDefinition
            {
                Type = typeof(TU3),
                Name = name
            });
        }

        public virtual EndpointWithResultAndUsing<TR, TU1, TU2, TU3> UsingQueryStringParameter<TU3>(string name)
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3>(EndpointDefinition, new EndpointUsingQueryStringParameterDefinition
            {
                Type = typeof(TU3),
                Name = name
            });
        }

        public virtual EndpointWithResultAndUsing<TR, TU1, TU2, TU3> UsingHeader<TU3>(string name)
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3>(EndpointDefinition, new EndpointUsingHeaderParameterDefinition
            {
                Type = typeof(TU3),
                Name = name
            });
        }

        public EndpointWithResultAndUsing<TR, TU1, TU2, TU3> UsingBody<TU3>()
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3>(EndpointDefinition, new EndpointUsingBodyDefinition
            {
                Type = typeof(TU3)
            });
        }

        public virtual EndpointWithResultAndUsing<TR, TU1, TU2, TU3> UsingForm<TU3>()
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3>(EndpointDefinition, new EndpointUsingFormDefinition
            {
                Type = typeof(TU3)
            });
        }

        public virtual EndpointWithResultAndUsing<TR, TU1, TU2, TU3> UsingFormValue<TU3>(string key)
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3>(EndpointDefinition, new EndpointUsingFormValueDefinition
            {
                Type = typeof(TU3),
                Key = key
            });
        }

        public virtual EndpointWithResultAndUsing<TR, TU1, TU2, TU3> UsingModelBinder<TU3>(Type modelBinderType)
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3>(EndpointDefinition, new EndpointUsingModelBinderDefinition
            {
                Type = typeof(TU3),
                ModelBinderType = modelBinderType
            });
        }

        public EndpointWithResult<TR2> HandledBy<TR2>(Func<TU1, TU2, TR2> handlerFuncAsync)
        {
            return new EndpointWithResult<TR2>(EndpointDefinition, handlerFuncAsync);
        }
    }

    public class EndpointWithResultAndUsing<TR, TU1, TU2, TU3> : EndpointBase
    {
        public EndpointWithResultAndUsing(EndpointDefinition endpointDefinition, EndpointUsingDefinition usingDefinition) : base(endpointDefinition)
        {
            EndpointDefinition.CurrentHandler.Usings.Add(usingDefinition);
        }

        public virtual EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4> Using<TU4>(EndpointUsingDefinition usingDefinition)
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4>(EndpointDefinition, usingDefinition);
        }

        public virtual EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TR> UsingResultFromHandler()
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TR>(EndpointDefinition, new EndpointUsingResultFromHandlerDefinition
            {
                Type = typeof(TR)
            });
        }

        public EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4> UsingService<TU4>()
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4>(EndpointDefinition, new EndpointUsingServiceDefinition
            {
                Type = typeof(TU4)
            });
        }

        public EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4> UsingRouteParameter<TU4>(string name)
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4>(EndpointDefinition, new EndpointUsingRouteParameterDefinition
            {
                Type = typeof(TU4),
                Name = name
            });
        }

        public virtual EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4> UsingQueryStringParameter<TU4>(string name)
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4>(EndpointDefinition, new EndpointUsingQueryStringParameterDefinition
            {
                Type = typeof(TU4),
                Name = name
            });
        }

        public virtual EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4> UsingHeader<TU4>(string name)
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4>(EndpointDefinition, new EndpointUsingHeaderParameterDefinition
            {
                Type = typeof(TU4),
                Name = name
            });
        }

        public EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4> UsingBody<TU4>()
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4>(EndpointDefinition, new EndpointUsingBodyDefinition
            {
                Type = typeof(TU4)
            });
        }

        public virtual EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4> UsingForm<TU4>()
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4>(EndpointDefinition, new EndpointUsingFormDefinition
            {
                Type = typeof(TU4)
            });
        }

        public virtual EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4> UsingFormValue<TU4>(string key)
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4>(EndpointDefinition, new EndpointUsingFormValueDefinition
            {
                Type = typeof(TU4),
                Key = key
            });
        }

        public virtual EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4> UsingModelBinder<TU4>(Type modelBinderType)
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4>(EndpointDefinition, new EndpointUsingModelBinderDefinition
            {
                Type = typeof(TU4),
                ModelBinderType = modelBinderType
            });
        }

        public EndpointWithResult<TR2> HandledBy<TR2>(Func<TU1, TU2, TU3, TR2> handlerFuncAsync)
        {
            return new EndpointWithResult<TR2>(EndpointDefinition, handlerFuncAsync);
        }
    }

    public class EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4> : EndpointBase
    {
        public EndpointWithResultAndUsing(EndpointDefinition endpointDefinition, EndpointUsingDefinition usingDefinition) : base(endpointDefinition)
        {
            EndpointDefinition.CurrentHandler.Usings.Add(usingDefinition);
        }

        public virtual EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5> Using<TU5>(EndpointUsingDefinition usingDefinition)
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5>(EndpointDefinition, usingDefinition);
        }

        public virtual EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TR> UsingResultFromHandler()
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TR>(EndpointDefinition, new EndpointUsingResultFromHandlerDefinition
            {
                Type = typeof(TR)
            });
        }

        public EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5> UsingService<TU5>()
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5>(EndpointDefinition, new EndpointUsingServiceDefinition
            {
                Type = typeof(TU5)
            });
        }

        public EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5> UsingRouteParameter<TU5>(string name)
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5>(EndpointDefinition, new EndpointUsingRouteParameterDefinition
            {
                Type = typeof(TU5),
                Name = name
            });
        }

        public virtual EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5> UsingQueryStringParameter<TU5>(string name)
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5>(EndpointDefinition, new EndpointUsingQueryStringParameterDefinition
            {
                Type = typeof(TU5),
                Name = name
            });
        }

        public virtual EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5> UsingHeader<TU5>(string name)
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5>(EndpointDefinition, new EndpointUsingHeaderParameterDefinition
            {
                Type = typeof(TU5),
                Name = name
            });
        }

        public EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5> UsingBody<TU5>()
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5>(EndpointDefinition, new EndpointUsingBodyDefinition
            {
                Type = typeof(TU5)
            });
        }

        public virtual EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5> UsingForm<TU5>()
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5>(EndpointDefinition, new EndpointUsingFormDefinition
            {
                Type = typeof(TU5)
            });
        }

        public virtual EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5> UsingFormValue<TU5>(string key)
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5>(EndpointDefinition, new EndpointUsingFormValueDefinition
            {
                Type = typeof(TU5),
                Key = key
            });
        }

        public virtual EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5> UsingModelBinder<TU5>(Type modelBinderType)
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5>(EndpointDefinition, new EndpointUsingModelBinderDefinition
            {
                Type = typeof(TU5),
                ModelBinderType = modelBinderType
            });
        }

        public EndpointWithResult<TR2> HandledBy<TR2>(Func<TU1, TU2, TU3, TU4, TR2> handlerFuncAsync)
        {
            return new EndpointWithResult<TR2>(EndpointDefinition, handlerFuncAsync);
        }
    }

    public class EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5> : EndpointBase
    {
        public EndpointWithResultAndUsing(EndpointDefinition endpointDefinition, EndpointUsingDefinition usingDefinition) : base(endpointDefinition)
        {
            EndpointDefinition.CurrentHandler.Usings.Add(usingDefinition);
        }

        public virtual EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6> Using<TU6>(EndpointUsingDefinition usingDefinition)
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6>(EndpointDefinition, usingDefinition);
        }

        public virtual EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TR> UsingResultFromHandler()
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TR>(EndpointDefinition, new EndpointUsingResultFromHandlerDefinition
            {
                Type = typeof(TR)
            });
        }

        public EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6> UsingService<TU6>()
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6>(EndpointDefinition, new EndpointUsingServiceDefinition
            {
                Type = typeof(TU6)
            });
        }

        public EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6> UsingRouteParameter<TU6>(string name)
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6>(EndpointDefinition, new EndpointUsingRouteParameterDefinition
            {
                Type = typeof(TU6),
                Name = name
            });
        }

        public virtual EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6> UsingQueryStringParameter<TU6>(string name)
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6>(EndpointDefinition, new EndpointUsingQueryStringParameterDefinition
            {
                Type = typeof(TU6),
                Name = name
            });
        }

        public virtual EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6> UsingHeader<TU6>(string name)
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6>(EndpointDefinition, new EndpointUsingHeaderParameterDefinition
            {
                Type = typeof(TU6),
                Name = name
            });
        }

        public EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6> UsingBody<TU6>()
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6>(EndpointDefinition, new EndpointUsingBodyDefinition
            {
                Type = typeof(TU6)
            });
        }

        public virtual EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6> UsingForm<TU6>()
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6>(EndpointDefinition, new EndpointUsingFormDefinition
            {
                Type = typeof(TU6)
            });
        }

        public virtual EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6> UsingFormValue<TU6>(string key)
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6>(EndpointDefinition, new EndpointUsingFormValueDefinition
            {
                Type = typeof(TU6),
                Key = key
            });
        }

        public virtual EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6> UsingModelBinder<TU6>(Type modelBinderType)
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6>(EndpointDefinition, new EndpointUsingModelBinderDefinition
            {
                Type = typeof(TU6),
                ModelBinderType = modelBinderType
            });
        }

        public EndpointWithResult<TR2> HandledBy<TR2>(Func<TU1, TU2, TU3, TU4, TU5, TR2> handlerFuncAsync)
        {
            return new EndpointWithResult<TR2>(EndpointDefinition, handlerFuncAsync);
        }
    }

    public class EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6> : EndpointBase
    {
        public EndpointWithResultAndUsing(EndpointDefinition endpointDefinition, EndpointUsingDefinition usingDefinition) : base(endpointDefinition)
        {
            EndpointDefinition.CurrentHandler.Usings.Add(usingDefinition);
        }

        public virtual EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7> Using<TU7>(EndpointUsingDefinition usingDefinition)
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7>(EndpointDefinition, usingDefinition);
        }

        public virtual EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TR> UsingResultFromHandler()
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TR>(EndpointDefinition, new EndpointUsingResultFromHandlerDefinition
            {
                Type = typeof(TR)
            });
        }

        public EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7> UsingService<TU7>()
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7>(EndpointDefinition, new EndpointUsingServiceDefinition
            {
                Type = typeof(TU7)
            });
        }

        public EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7> UsingRouteParameter<TU7>(string name)
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7>(EndpointDefinition, new EndpointUsingRouteParameterDefinition
            {
                Type = typeof(TU7),
                Name = name
            });
        }

        public virtual EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7> UsingQueryStringParameter<TU7>(string name)
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7>(EndpointDefinition, new EndpointUsingQueryStringParameterDefinition
            {
                Type = typeof(TU7),
                Name = name
            });
        }

        public virtual EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7> UsingHeader<TU7>(string name)
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7>(EndpointDefinition, new EndpointUsingHeaderParameterDefinition
            {
                Type = typeof(TU7),
                Name = name
            });
        }

        public EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7> UsingBody<TU7>()
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7>(EndpointDefinition, new EndpointUsingBodyDefinition
            {
                Type = typeof(TU7)
            });
        }

        public virtual EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7> UsingForm<TU7>()
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7>(EndpointDefinition, new EndpointUsingFormDefinition
            {
                Type = typeof(TU7)
            });
        }

        public virtual EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7> UsingFormValue<TU7>(string key)
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7>(EndpointDefinition, new EndpointUsingFormValueDefinition
            {
                Type = typeof(TU7),
                Key = key
            });
        }

        public virtual EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7> UsingModelBinder<TU7>(Type modelBinderType)
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7>(EndpointDefinition, new EndpointUsingModelBinderDefinition
            {
                Type = typeof(TU7),
                ModelBinderType = modelBinderType
            });
        }

        public EndpointWithResult<TR2> HandledBy<TR2>(Func<TU1, TU2, TU3, TU4, TU5, TU6, TR2> handlerFuncAsync)
        {
            return new EndpointWithResult<TR2>(EndpointDefinition, handlerFuncAsync);
        }
    }

    public class EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7> : EndpointBase
    {
        public EndpointWithResultAndUsing(EndpointDefinition endpointDefinition, EndpointUsingDefinition usingDefinition) : base(endpointDefinition)
        {
            EndpointDefinition.CurrentHandler.Usings.Add(usingDefinition);
        }

        public virtual EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> Using<TU8>(EndpointUsingDefinition usingDefinition)
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(EndpointDefinition, usingDefinition);
        }

        public virtual EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TR> UsingResultFromHandler()
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TR>(EndpointDefinition, new EndpointUsingResultFromHandlerDefinition
            {
                Type = typeof(TR)
            });
        }

        public EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> UsingService<TU8>()
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(EndpointDefinition, new EndpointUsingServiceDefinition
            {
                Type = typeof(TU8)
            });
        }

        public EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> UsingRouteParameter<TU8>(string name)
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(EndpointDefinition, new EndpointUsingRouteParameterDefinition
            {
                Type = typeof(TU8),
                Name = name
            });
        }

        public virtual EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> UsingQueryStringParameter<TU8>(string name)
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(EndpointDefinition, new EndpointUsingQueryStringParameterDefinition
            {
                Type = typeof(TU8),
                Name = name
            });
        }

        public virtual EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> UsingHeader<TU8>(string name)
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(EndpointDefinition, new EndpointUsingHeaderParameterDefinition
            {
                Type = typeof(TU8),
                Name = name
            });
        }

        public EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> UsingBody<TU8>()
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(EndpointDefinition, new EndpointUsingBodyDefinition
            {
                Type = typeof(TU8)
            });
        }

        public virtual EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> UsingForm<TU8>()
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(EndpointDefinition, new EndpointUsingFormDefinition
            {
                Type = typeof(TU8)
            });
        }

        public virtual EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> UsingFormValue<TU8>(string key)
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(EndpointDefinition, new EndpointUsingFormValueDefinition
            {
                Type = typeof(TU8),
                Key = key
            });
        }

        public virtual EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> UsingModelBinder<TU8>(Type modelBinderType)
        {
            return new EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(EndpointDefinition, new EndpointUsingModelBinderDefinition
            {
                Type = typeof(TU8),
                ModelBinderType = modelBinderType
            });
        }

        public EndpointWithResult<TR2> HandledBy<TR2>(Func<TU1, TU2, TU3, TU4, TU5, TU6, TU7, TR2> handlerFuncAsync)
        {
            return new EndpointWithResult<TR2>(EndpointDefinition, handlerFuncAsync);
        }
    }

    public class EndpointWithResultAndUsing<TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> : Endpoint
    {
        public EndpointWithResultAndUsing(EndpointDefinition endpointDefinition, EndpointUsingDefinition usingDefinition) : base(endpointDefinition)
        {
            EndpointDefinition.CurrentHandler.Usings.Add(usingDefinition);
        }

        public EndpointWithResult<TR2> HandledBy<TR2>(Func<TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8, TR2> handlerFuncAsync)
        {
            return new EndpointWithResult<TR2>(EndpointDefinition, handlerFuncAsync);
        }
    }
}
