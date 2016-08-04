using System;

// ReSharper disable InconsistentNaming

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentActions
{
    public abstract class EndpointUsingDefinition
    {
        public Type Type { get; set; }

        public abstract bool IsMethodParameter { get; }

        public override bool Equals(object other)
        {
            return other is EndpointUsingDefinition && other.GetHashCode() == GetHashCode();
        }

        public override int GetHashCode()
        {
            return Tuple.Create(GetType(), Type).GetHashCode();
        }
    }

    public class EndpointUsingServiceDefinition : EndpointUsingDefinition
    {
        public override bool IsMethodParameter => true;
    }

    public class EndpointUsingRouteParameterDefinition : EndpointUsingDefinition
    {
        public string Name { get; set; }

        public override bool IsMethodParameter => true;

        public override int GetHashCode()
        {
            return Tuple.Create(GetType(), Type, Name.ToLowerInvariant()).GetHashCode();
        }
    }

    public class EndpointUsingQueryStringParameterDefinition : EndpointUsingDefinition
    {
        public string Name { get; set; }

        public override bool IsMethodParameter => true;

        public override int GetHashCode()
        {
            return Tuple.Create(GetType(), Type, Name.ToLowerInvariant()).GetHashCode();
        }
    }

    public class EndpointUsingHeaderParameterDefinition : EndpointUsingDefinition
    {
        public string Name { get; set; }

        public override bool IsMethodParameter => true;

        public override int GetHashCode()
        {
            return Tuple.Create(GetType(), Type, Name.ToLowerInvariant()).GetHashCode();
        }
    }

    public class EndpointUsingBodyDefinition : EndpointUsingDefinition
    {
        public override bool IsMethodParameter => true;
    }

    public class EndpointUsingFormDefinition : EndpointUsingDefinition
    {
        public override bool IsMethodParameter => true;
    }

    public class EndpointUsingFormValueDefinition : EndpointUsingDefinition
    {
        public string Key { get; set; }

        public override bool IsMethodParameter => true;

        public override int GetHashCode()
        {
            return Tuple.Create(GetType(), Type, Key.ToLowerInvariant()).GetHashCode();
        }
    }

    public class EndpointUsingModelBinderDefinition : EndpointUsingDefinition
    {
        public Type ModelBinderType { get; set; }

        public override bool IsMethodParameter => true;

        public override int GetHashCode()
        {
            return Tuple.Create(GetType(), Type, ModelBinderType).GetHashCode();
        }
    }

    public class EndpointUsingResultFromHandlerDefinition : EndpointUsingDefinition
    {
        public override bool IsMethodParameter => false;
    }

    public class EndpointUsingHttpContextDefinition : EndpointUsingDefinition
    {
        public override bool IsMethodParameter => false;
    }
}
