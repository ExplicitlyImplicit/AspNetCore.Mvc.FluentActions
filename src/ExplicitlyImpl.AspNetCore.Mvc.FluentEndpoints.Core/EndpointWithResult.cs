using System.Linq.Expressions;

// ReSharper disable InconsistentNaming

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentEndpoints
{
    public class EndpointWithResult<TR> : Endpoint
    {
        public EndpointWithResult(EndpointDefinition endpointDefinition, LambdaExpression handlerFunc) : base(endpointDefinition)
        {
            EndpointDefinition.CurrentHandler.Func = handlerFunc;
            EndpointDefinition.CurrentHandler.ReturnType = typeof(TR);
        }
    }
}
