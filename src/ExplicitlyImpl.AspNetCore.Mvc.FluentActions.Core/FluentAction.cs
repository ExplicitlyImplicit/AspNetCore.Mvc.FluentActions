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

    public class FluentAction : FluentActionBase
    {
        public FluentAction(HttpMethod httpMethod, string routeTemplate, string id = null)
            : base(httpMethod, routeTemplate, id) { }

        public FluentAction(string routeTemplate, HttpMethod httpMethod, string id = null)
            : base(httpMethod, routeTemplate, id) { }

        public FluentAction(FluentActionDefinition fluentActionDefinition)
            : base(fluentActionDefinition) { }


        public virtual FluentAction InheritingFrom(Type parentType)
        {
            if (parentType != typeof(Controller) && !parentType.GetTypeInfo().IsSubclassOf(typeof(Controller)))
            {
                throw new Exception($"Cannot make fluent action controller inherit from a class that is not derived from the Controller class (${parentType.FullName}).");
            }

            Definition.ParentType = parentType;
            return this;
        }

        public virtual FluentAction InheritingFrom<T>() where T : Controller
        {
            Definition.ParentType = typeof(T);
            return this;
        }

        public virtual FluentAction GroupBy(string groupName)
        {
            Definition.GroupName = groupName;
            return this;
        }

        public virtual FluentAction WithTitle(string title)
        {
            Definition.Title = title;
            return this;
        }

        public virtual FluentAction WithDescription(string description)
        {
            Definition.Description = description;
            return this;
        }

        public virtual FluentAction WithCustomAttribute<T>()
        {
            return WithCustomAttribute<T>(new Type[0], new object[0]);
        }

        public virtual FluentAction WithCustomAttribute<T>(Type[] constructorArgTypes, object[] constructorArgs)
        {
            var attributeConstructorInfo = typeof(T).GetConstructor(constructorArgTypes);
            return WithCustomAttribute<T>(attributeConstructorInfo, constructorArgs);
        }

        public virtual FluentAction WithCustomAttribute<T>(Type[] constructorArgTypes, object[] constructorArgs, string[] namedProperties, object[] propertyValues)
        {
            var attributeConstructorInfo = typeof(T).GetConstructor(constructorArgTypes);

            return WithCustomAttribute<T>(
                attributeConstructorInfo, 
                constructorArgs, 
                namedProperties.Select(propertyName => typeof(T).GetProperty(propertyName)).ToArray(), 
                propertyValues);
        }

        public virtual FluentAction WithCustomAttribute<T>(ConstructorInfo constructor, object[] constructorArgs)
        {
            return WithCustomAttribute<T>(
                constructor,
                constructorArgs,
                new PropertyInfo[0],
                new object[0],
                new FieldInfo[0],
                new object[0]);
        }

        public virtual FluentAction WithCustomAttribute<T>(ConstructorInfo constructor, object[] constructorArgs, FieldInfo[] namedFields, object[] fieldValues)
        {
            return WithCustomAttribute<T>(
                constructor,
                constructorArgs,
                new PropertyInfo[0],
                new object[0],
                namedFields,
                fieldValues);
        }

        public virtual FluentAction WithCustomAttribute<T>(ConstructorInfo constructor, object[] constructorArgs, PropertyInfo[] namedProperties, object[] propertyValues)
        {
            return WithCustomAttribute<T>(
                constructor,
                constructorArgs,
                namedProperties,
                propertyValues,
                new FieldInfo[0],
                new object[0]);
        }

        public virtual FluentAction WithCustomAttribute<T>(ConstructorInfo constructor, object[] constructorArgs, PropertyInfo[] namedProperties, object[] propertyValues, FieldInfo[] namedFields, object[] fieldValues)
        {
            Definition.CustomAttributes.Add(new FluentActionCustomAttribute()
            {
                Type = typeof(T),
                Constructor = constructor,
                ConstructorArgs = constructorArgs,
                NamedProperties = namedProperties,
                PropertyValues = propertyValues,
                NamedFields = namedFields,
                FieldValues = fieldValues,
            });
            return this;
        }

        public virtual FluentAction ValidateAntiForgeryToken()
        {
            return WithCustomAttribute<ValidateAntiForgeryTokenAttribute>();
        }

        public virtual FluentAction Authorize(string policy = null, string roles = null, string activeAuthenticationSchemes = null)
        {
            return WithCustomAttribute<AuthorizeAttribute>(
                new Type[] { typeof(string) },
                new object[] { policy },
                new string[] { "Roles", "ActiveAuthenticationSchemes" },
                new object[] { roles, activeAuthenticationSchemes });
        }

        public virtual FluentAction AllowAnonymous()
        {
            return WithCustomAttribute<AllowAnonymousAttribute>();
        }

        public virtual FluentActionWithUsing<TU1> Using<TU1>(FluentActionUsingDefinition usingDefinition)
        {
            return new FluentActionWithUsing<TU1>(Definition, usingDefinition);
        }

        public virtual FluentActionWithUsing<TU1> UsingService<TU1>()
        {
            return new FluentActionWithUsing<TU1>(Definition, new FluentActionUsingServiceDefinition
            {
                Type = typeof(TU1)
            });
        }

        public virtual FluentActionWithUsing<TU1> UsingService<TU1>(TU1 defaultValue)
        {
            return new FluentActionWithUsing<TU1>(Definition, new FluentActionUsingServiceDefinition
            {
                Type = typeof(TU1),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithUsing<TU1> UsingRouteParameter<TU1>(string name)
        {
            return new FluentActionWithUsing<TU1>(Definition, new FluentActionUsingRouteParameterDefinition
            {
                Type = typeof(TU1),
                Name = name
            });
        }

        public virtual FluentActionWithUsing<TU1> UsingRouteParameter<TU1>(string name, TU1 defaultValue)
        {
            return new FluentActionWithUsing<TU1>(Definition, new FluentActionUsingRouteParameterDefinition
            {
                Type = typeof(TU1),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithUsing<TU1> UsingQueryStringParameter<TU1>(string name)
        {
            return new FluentActionWithUsing<TU1>(Definition, new FluentActionUsingQueryStringParameterDefinition
            {
                Type = typeof(TU1),
                Name = name
            });
        }

        public virtual FluentActionWithUsing<TU1> UsingQueryStringParameter<TU1>(string name, TU1 defaultValue)
        {
            return new FluentActionWithUsing<TU1>(Definition, new FluentActionUsingQueryStringParameterDefinition
            {
                Type = typeof(TU1),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithUsing<TU1> UsingHeader<TU1>(string name)
        {
            return new FluentActionWithUsing<TU1>(Definition, new FluentActionUsingHeaderParameterDefinition
            {
                Type = typeof(TU1),
                Name = name
            });
        }

        public virtual FluentActionWithUsing<TU1> UsingHeader<TU1>(string name, TU1 defaultValue)
        {
            return new FluentActionWithUsing<TU1>(Definition, new FluentActionUsingHeaderParameterDefinition
            {
                Type = typeof(TU1),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithUsing<TU1> UsingBody<TU1>()
        {
            return new FluentActionWithUsing<TU1>(Definition, new FluentActionUsingBodyDefinition
            {
                Type = typeof(TU1)
            });
        }

        public virtual FluentActionWithUsing<TU1> UsingBody<TU1>(TU1 defaultValue)
        {
            return new FluentActionWithUsing<TU1>(Definition, new FluentActionUsingBodyDefinition
            {
                Type = typeof(TU1),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithUsing<TU1> UsingForm<TU1>()
        {
            return new FluentActionWithUsing<TU1>(Definition, new FluentActionUsingFormDefinition
            {
                Type = typeof(TU1)
            });
        }

        public virtual FluentActionWithUsing<TU1> UsingForm<TU1>(TU1 defaultValue)
        {
            return new FluentActionWithUsing<TU1>(Definition, new FluentActionUsingFormDefinition
            {
                Type = typeof(TU1),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithUsing<IFormFile> UsingFormFile(string name)
        {
            return new FluentActionWithUsing<IFormFile>(Definition, new FluentActionUsingFormFileDefinition
            {
                Type = typeof(IFormFile),
                Name = name
            });
        }

        public virtual FluentActionWithUsing<IEnumerable<IFormFile>> UsingFormFiles(string name)
        {
            return new FluentActionWithUsing<IEnumerable<IFormFile>>(Definition, new FluentActionUsingFormFilesDefinition
            {
                Type = typeof(IEnumerable<IFormFile>),
                Name = name
            });
        }

        public virtual FluentActionWithUsing<TU1> UsingFormValue<TU1>(string key)
        {
            return new FluentActionWithUsing<TU1>(Definition, new FluentActionUsingFormValueDefinition
            {
                Type = typeof(TU1),
                Key = key
            });
        }

        public virtual FluentActionWithUsing<TU1> UsingFormValue<TU1>(string key, TU1 defaultValue)
        {
            return new FluentActionWithUsing<TU1>(Definition, new FluentActionUsingFormValueDefinition
            {
                Type = typeof(TU1),
                Key = key,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentActionWithUsing<TU1> UsingModelBinder<TU1>(Type modelBinderType, string parameterName = null)
        {
            return new FluentActionWithUsing<TU1>(Definition, new FluentActionUsingModelBinderDefinition
            {
                Type = typeof(TU1),
                ModelBinderType = modelBinderType,
                ParameterName = parameterName
            });
        }

        public virtual FluentActionWithUsing<TU1> UsingModelBinder<TU1>(Type modelBinderType, string parameterName, TU1 defaultValue)
        {
            return new FluentActionWithUsing<TU1>(Definition, new FluentActionUsingModelBinderDefinition
            {
                Type = typeof(TU1),
                ModelBinderType = modelBinderType,
                HasDefaultValue = true,
                DefaultValue = defaultValue,
                ParameterName = parameterName
            });
        }

        public virtual FluentActionWithUsing<ModelStateDictionary> UsingModelState()
        {
            return new FluentActionWithUsing<ModelStateDictionary>(Definition, new FluentActionUsingModelStateDefinition
            {
                Type = typeof(ModelStateDictionary)
            });
        }

        public virtual FluentActionWithUsing<HttpContext> UsingHttpContext()
        {
            return new FluentActionWithUsing<HttpContext>(Definition, new FluentActionUsingHttpContextDefinition
            {
                Type = typeof(HttpContext)
            });
        }

        public virtual FluentActionWithUsing<dynamic> UsingViewBag()
        {
            return new FluentActionWithUsing<dynamic>(Definition, new FluentActionUsingViewBagDefinition
            {
                Type = typeof(object)
            });
        }

        public virtual FluentActionWithUsing<ViewDataDictionary> UsingViewData()
        {
            return new FluentActionWithUsing<ViewDataDictionary>(Definition, new FluentActionUsingViewDataDefinition
            {
                Type = typeof(ViewDataDictionary)
            });
        }

        public virtual FluentActionWithUsing<ITempDataDictionary> UsingTempData()
        {
            return new FluentActionWithUsing<ITempDataDictionary>(Definition, new FluentActionUsingTempDataDefinition
            {
                Type = typeof(ITempDataDictionary)
            });
        }

        public FluentAction Do(Action handlerAction)
        {
            Definition.ExistingOrNewHandlerDraft.Type = FluentActionHandlerType.Action;
            Definition.ExistingOrNewHandlerDraft.Delegate = handlerAction;
            Definition.CommitHandlerDraft();
            return new FluentAction(Definition);
        }

        public FluentAction DoAsync(Func<Task> asyncHandlerAction)
        {
            Definition.ExistingOrNewHandlerDraft.Type = FluentActionHandlerType.Action;
            Definition.ExistingOrNewHandlerDraft.Delegate = asyncHandlerAction;
            Definition.ExistingOrNewHandlerDraft.Async = true;
            Definition.CommitHandlerDraft();
            return new FluentAction(Definition);
        }

        public virtual FluentActionWithMvcController<TC> ToMvcController<TC>() where TC : Controller
        {
            return new FluentActionWithMvcController<TC>(Definition, new FluentActionUsingMvcControllerDefinition
            {
                Type = typeof(TC)
            });
        }

        public FluentActionWithResult<TR> To<TR>(Func<TR> handlerFunc)
        {
            return new FluentActionWithResult<TR>(Definition, handlerFunc);
        }

        public FluentActionWithResult<TR> To<TR>(Func<Task<TR>> asyncHandlerFunc)
        {
            return new FluentActionWithResult<TR>(Definition, asyncHandlerFunc, async: true);
        }

        public FluentActionWithView ToView(string pathToView)
        {
            return new FluentActionWithView(Definition, pathToView);
        }

        public FluentActionWithPartialView ToPartialView(string pathToView)
        {
            return new FluentActionWithPartialView(Definition, pathToView);
        }

        public FluentActionWithViewComponent ToViewComponent(Type viewComponentType)
        {
            return new FluentActionWithViewComponent(Definition, viewComponentType);
        }

        public FluentActionWithViewComponent ToViewComponent(string viewComponentName)
        {
            return new FluentActionWithViewComponent(Definition, viewComponentName);
        }

        public static FluentAction Route(string routeTemplate, HttpMethod httpMethod, string id = null)
        {
            return new FluentAction(httpMethod, routeTemplate, id);
        }

        public static FluentAction RouteDelete(string routeTemplate, string id = null)
        {
            return Route(routeTemplate, HttpMethod.Delete, id);
        }

        public static FluentAction RouteGet(string routeTemplate, string id = null)
        {
            return Route(routeTemplate, HttpMethod.Get, id);
        }

        public static FluentAction RouteHead(string routeTemplate, string id = null)
        {
            return Route(routeTemplate, HttpMethod.Head, id);
        }

        public static FluentAction RouteOptions(string routeTemplate, string id = null)
        {
            return Route(routeTemplate, HttpMethod.Options, id);
        }

        public static FluentAction RoutePatch(string routeTemplate, string id = null)
        {
            return Route(routeTemplate, HttpMethod.Patch, id);
        }

        public static FluentAction RoutePost(string routeTemplate, string id = null)
        {
            return Route(routeTemplate, HttpMethod.Post, id);
        }

        public static FluentAction RoutePut(string routeTemplate, string id = null)
        {
            return Route(routeTemplate, HttpMethod.Put, id);
        }
    }
}
