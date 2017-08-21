// Licensed under the MIT License. See LICENSE file in the root of the solution for license information.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentActions
{
    public enum FluentActionHandlerType
    {
        Unknown,
        Func,
        Action,
        View,
        PartialView,
        ViewComponent,
        Controller
    }

    public class FluentActionHandlerDefinition
    {
        public FluentActionHandlerType Type { get; set; }

        public IList<FluentActionUsingDefinition> Usings { get; set; }

        public Type ReturnType { get; set; }

        public bool Async { get; set; }

        public Delegate Delegate { get; set; }

        // Path to view or name of view component
        public string ViewTarget { get; set; }
        public Type ViewComponentType { get; set; }

        public LambdaExpression Expression { get; set; }

        public FluentActionHandlerDefinition()
        {
            Type = FluentActionHandlerType.Unknown;
            Usings = new List<FluentActionUsingDefinition>();
        }
    }

    public class FluentActionDefinition
    {
        public readonly string RouteTemplate;

        public readonly HttpMethod HttpMethod;

        public readonly string Id;

        public string Title { get; internal set; }

        public string Description { get; internal set; }

        public string GroupName { get; internal set; }

        public Type ParentType { get; internal set; }

        [Obsolete("This property will be removed in next major version. Please use CustomAttributes property instead.")]
        public bool ValidateAntiForgeryToken =>
            CustomAttributes.Any(attr => attr.Type == typeof(ValidateAntiForgeryTokenAttribute));

        public IList<FluentActionHandlerDefinition> Handlers { get; internal set; }
        
        internal FluentActionHandlerDefinition HandlerDraft { get; set; }

        internal FluentActionHandlerDefinition ExistingOrNewHandlerDraft 
        {
            get
            {
                if (HandlerDraft == null)
                {
                    HandlerDraft = new FluentActionHandlerDefinition();
                }

                return HandlerDraft;
            }
        }

        public IList<FluentActionCustomAttribute> CustomAttributes { get; internal set; }

        public Type ReturnType => Handlers?.LastOrDefault()?.ReturnType;

        public bool IsMapRoute => Handlers.Count == 1 && Handlers.First().Type == FluentActionHandlerType.Controller;

        public bool IsAsync => Handlers.Any(handler => handler.Async);

        public FluentActionDefinition(string routeTemplate, HttpMethod httpMethod, string id = null)
        {
            RouteTemplate = routeTemplate;
            HttpMethod = httpMethod;
            Id = id;

            Handlers = new List<FluentActionHandlerDefinition>();
            CustomAttributes = new List<FluentActionCustomAttribute>();
        }

        public override string ToString()
        {
            return $"[{HttpMethod}]/{RouteTemplate ?? "?"}";
        }

        public void CommitHandlerDraft()
        {
            if (HandlerDraft == null)
            {
                // Users should not be able to get this
                throw new Exception("Tried to add an empty fluent action handler (no draft exists).");
            }

            Handlers.Add(HandlerDraft);

            HandlerDraft = null;
        }
    }

    public class FluentActionBase
    {
        public readonly FluentActionDefinition Definition;

        public string RouteTemplate => Definition.RouteTemplate;

        public HttpMethod HttpMethod => Definition.HttpMethod;

        public string Id => Definition.Id;

        public FluentActionBase(HttpMethod httpMethod, string routeTemplate, string id = null)
        {
            Definition = new FluentActionDefinition(routeTemplate, httpMethod, id);
        }

        public FluentActionBase(string routeTemplate, HttpMethod httpMethod, string id = null)
            : this(httpMethod, routeTemplate, id) { }

        public FluentActionBase(FluentActionDefinition actionDefinition)
        {
            Definition = actionDefinition;
        }

        public override string ToString()
        {
            return Definition.ToString();
        }
    }
}
