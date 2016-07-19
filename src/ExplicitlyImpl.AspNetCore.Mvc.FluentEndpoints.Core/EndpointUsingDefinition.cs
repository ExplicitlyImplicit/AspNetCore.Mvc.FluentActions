﻿using System;
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

    public class EndpointUsingRouteParameterDefinition : EndpointUsingDefinition
    {
        public string Name { get; set; }
    }

    public class EndpointUsingQueryStringParameterDefinition : EndpointUsingDefinition
    {
        public string Name { get; set; }
    }

    public class EndpointUsingHeaderParameterDefinition : EndpointUsingDefinition
    {
        public string Name { get; set; }
    }

    public class EndpointUsingBodyDefinition : EndpointUsingDefinition
    {
    }

    public class EndpointUsingFormDefinition : EndpointUsingDefinition
    {
    }

    public class EndpointUsingFormValueDefinition : EndpointUsingDefinition
    {
        public string Key { get; set; }
    }
}