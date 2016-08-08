using System;

// ReSharper disable InconsistentNaming

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentActions
{
    public abstract class FluentActionUsingDefinition
    {
        public Type Type { get; set; }

        public bool HasDefaultValue { get; set; }

        public object DefaultValue { get; set; }

        public abstract bool IsMethodParameter { get; }

        public override bool Equals(object other)
        {
            return other is FluentActionUsingDefinition && other.GetHashCode() == GetHashCode();
        }

        public override int GetHashCode()
        {
            return Tuple.Create(GetType(), Type).GetHashCode();
        }
    }

    public class FluentActionUsingServiceDefinition : FluentActionUsingDefinition
    {
        public override bool IsMethodParameter => true;
    }

    public class FluentActionUsingRouteParameterDefinition : FluentActionUsingDefinition
    {
        public string Name { get; set; }

        public override bool IsMethodParameter => true;

        public override int GetHashCode()
        {
            return Tuple.Create(GetType(), Type, Name.ToLowerInvariant()).GetHashCode();
        }
    }

    public class FluentActionUsingQueryStringParameterDefinition : FluentActionUsingDefinition
    {
        public string Name { get; set; }

        public override bool IsMethodParameter => true;

        public override int GetHashCode()
        {
            return Tuple.Create(GetType(), Type, Name.ToLowerInvariant()).GetHashCode();
        }
    }

    public class FluentActionUsingHeaderParameterDefinition : FluentActionUsingDefinition
    {
        public string Name { get; set; }

        public override bool IsMethodParameter => true;

        public override int GetHashCode()
        {
            return Tuple.Create(GetType(), Type, Name.ToLowerInvariant()).GetHashCode();
        }
    }

    public class FluentActionUsingBodyDefinition : FluentActionUsingDefinition
    {
        public override bool IsMethodParameter => true;
    }

    public class FluentActionUsingFormDefinition : FluentActionUsingDefinition
    {
        public override bool IsMethodParameter => true;
    }

    public class FluentActionUsingFormValueDefinition : FluentActionUsingDefinition
    {
        public string Key { get; set; }

        public override bool IsMethodParameter => true;

        public override int GetHashCode()
        {
            return Tuple.Create(GetType(), Type, Key.ToLowerInvariant()).GetHashCode();
        }
    }

    public class FluentActionUsingModelBinderDefinition : FluentActionUsingDefinition
    {
        public Type ModelBinderType { get; set; }

        public override bool IsMethodParameter => true;

        public override int GetHashCode()
        {
            return Tuple.Create(GetType(), Type, ModelBinderType).GetHashCode();
        }
    }

    public class FluentActionUsingResultFromHandlerDefinition : FluentActionUsingDefinition
    {
        public override bool IsMethodParameter => false;
    }

    public class FluentActionUsingHttpContextDefinition : FluentActionUsingDefinition
    {
        public override bool IsMethodParameter => false;
    }
}
