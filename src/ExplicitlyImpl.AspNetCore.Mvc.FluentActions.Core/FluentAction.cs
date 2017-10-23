// Licensed under the MIT License. See LICENSE file in the root of the solution for license information.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentActions
{
    public class FluentAction : FluentAction<object, object>
    {
        public FluentAction(FluentActionDefinition fluentActionDefinition)
            : base(fluentActionDefinition) { }

        public FluentAction(string routeTemplate, HttpMethod httpMethod, string id = null)
            : base(routeTemplate, httpMethod, id) { }
    }

    public class FluentAction<TP, TR> : FluentActionBase
    {
        public FluentAction(FluentActionDefinition fluentActionDefinition)
            : base(fluentActionDefinition) { }

        public FluentAction(string routeTemplate, HttpMethod httpMethod, string id = null)
            : base(httpMethod, routeTemplate, id) { }

        public FluentAction(FluentActionDefinition fluentActionDefinition, Delegate handlerFunc, bool async = false) 
            : base(fluentActionDefinition)
        {
            var returnType = typeof(TR);

            Definition.ExistingOrNewHandlerDraft.Type = FluentActionHandlerType.Func;
            Definition.ExistingOrNewHandlerDraft.Delegate = handlerFunc;
            Definition.ExistingOrNewHandlerDraft.ReturnType = returnType.IsAnonymous() ? typeof(object) : returnType;
            Definition.ExistingOrNewHandlerDraft.Async = async;
            Definition.CommitHandlerDraft();
        }

        public virtual FluentAction<TP2, TR> InheritingFrom<TP2>() where TP2 : Controller
        {
            Definition.ParentType = typeof(TP2);
            return new FluentAction<TP2, TR>(Definition);
        }

        public virtual FluentAction<TP, TR> GroupBy(string groupName)
        {
            Definition.GroupName = groupName;
            return this;
        }

        public virtual FluentAction<TP, TR> WithTitle(string title)
        {
            Definition.Title = title;
            return this;
        }

        public virtual FluentAction<TP, TR> WithDescription(string description)
        {
            Definition.Description = description;
            return this;
        }

        public virtual FluentAction<TP, TR> WithCustomAttribute<T>()
        {
            return WithCustomAttribute<T>(new Type[0], new object[0]);
        }

        public virtual FluentAction<TP, TR> WithCustomAttribute<T>(Type[] constructorArgTypes, object[] constructorArgs)
        {
            var attributeConstructorInfo = typeof(T).GetConstructor(constructorArgTypes);
            return WithCustomAttribute<T>(attributeConstructorInfo, constructorArgs);
        }

        public virtual FluentAction<TP, TR> WithCustomAttribute<T>(Type[] constructorArgTypes, object[] constructorArgs, string[] namedProperties, object[] propertyValues)
        {
            var attributeConstructorInfo = typeof(T).GetConstructor(constructorArgTypes);

            return WithCustomAttribute<T>(
                attributeConstructorInfo,
                constructorArgs,
                namedProperties.Select(propertyName => typeof(T).GetProperty(propertyName)).ToArray(),
                propertyValues);
        }

        public virtual FluentAction<TP, TR> WithCustomAttribute<T>(ConstructorInfo constructor, object[] constructorArgs)
        {
            return WithCustomAttribute<T>(
                constructor,
                constructorArgs,
                new PropertyInfo[0],
                new object[0],
                new FieldInfo[0],
                new object[0]);
        }

        public virtual FluentAction<TP, TR> WithCustomAttribute<T>(ConstructorInfo constructor, object[] constructorArgs, FieldInfo[] namedFields, object[] fieldValues)
        {
            return WithCustomAttribute<T>(
                constructor,
                constructorArgs,
                new PropertyInfo[0],
                new object[0],
                namedFields,
                fieldValues);
        }

        public virtual FluentAction<TP, TR> WithCustomAttribute<T>(ConstructorInfo constructor, object[] constructorArgs, PropertyInfo[] namedProperties, object[] propertyValues)
        {
            return WithCustomAttribute<T>(
                constructor,
                constructorArgs,
                namedProperties,
                propertyValues,
                new FieldInfo[0],
                new object[0]);
        }

        public virtual FluentAction<TP, TR> WithCustomAttribute<T>(ConstructorInfo constructor, object[] constructorArgs, PropertyInfo[] namedProperties, object[] propertyValues, FieldInfo[] namedFields, object[] fieldValues)
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

        public virtual FluentAction<TP, TR> WithCustomAttributeOnClass<T>()
        {
            return WithCustomAttributeOnClass<T>(new Type[0], new object[0]);
        }

        public virtual FluentAction<TP, TR> WithCustomAttributeOnClass<T>(Type[] constructorArgTypes, object[] constructorArgs)
        {
            var attributeConstructorInfo = typeof(T).GetConstructor(constructorArgTypes);
            return WithCustomAttributeOnClass<T>(attributeConstructorInfo, constructorArgs);
        }

        public virtual FluentAction<TP, TR> WithCustomAttributeOnClass<T>(Type[] constructorArgTypes, object[] constructorArgs, string[] namedProperties, object[] propertyValues)
        {
            var attributeConstructorInfo = typeof(T).GetConstructor(constructorArgTypes);

            return WithCustomAttributeOnClass<T>(
                attributeConstructorInfo,
                constructorArgs,
                namedProperties.Select(propertyName => typeof(T).GetProperty(propertyName)).ToArray(),
                propertyValues);
        }

        public virtual FluentAction<TP, TR> WithCustomAttributeOnClass<T>(ConstructorInfo constructor, object[] constructorArgs)
        {
            return WithCustomAttributeOnClass<T>(
                constructor,
                constructorArgs,
                new PropertyInfo[0],
                new object[0],
                new FieldInfo[0],
                new object[0]);
        }

        public virtual FluentAction<TP, TR> WithCustomAttributeOnClass<T>(ConstructorInfo constructor, object[] constructorArgs, FieldInfo[] namedFields, object[] fieldValues)
        {
            return WithCustomAttributeOnClass<T>(
                constructor,
                constructorArgs,
                new PropertyInfo[0],
                new object[0],
                namedFields,
                fieldValues);
        }

        public virtual FluentAction<TP, TR> WithCustomAttributeOnClass<T>(ConstructorInfo constructor, object[] constructorArgs, PropertyInfo[] namedProperties, object[] propertyValues)
        {
            return WithCustomAttributeOnClass<T>(
                constructor,
                constructorArgs,
                namedProperties,
                propertyValues,
                new FieldInfo[0],
                new object[0]);
        }

        public virtual FluentAction<TP, TR> WithCustomAttributeOnClass<T>(ConstructorInfo constructor, object[] constructorArgs, PropertyInfo[] namedProperties, object[] propertyValues, FieldInfo[] namedFields, object[] fieldValues)
        {
            Definition.CustomAttributesOnClass.Add(new FluentActionCustomAttribute()
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

        public virtual FluentAction<TP, TR> ValidateAntiForgeryToken()
        {
            return WithCustomAttribute<ValidateAntiForgeryTokenAttribute>();
        }

        public virtual FluentAction<TP, TR> Authorize(string policy = null, string roles = null, string activeAuthenticationSchemes = null)
        {
            return WithCustomAttribute<AuthorizeAttribute>(
                new Type[] { typeof(string) },
                new object[] { policy },
                new string[] { "Roles", "ActiveAuthenticationSchemes" },
                new object[] { roles, activeAuthenticationSchemes });
        }

        public virtual FluentAction<TP, TR> AuthorizeClass(string policy = null, string roles = null, string activeAuthenticationSchemes = null)
        {
            return WithCustomAttributeOnClass<AuthorizeAttribute>(
                new Type[] { typeof(string) },
                new object[] { policy },
                new string[] { "Roles", "ActiveAuthenticationSchemes" },
                new object[] { roles, activeAuthenticationSchemes });
        }

        public virtual FluentAction<TP, TR> AllowAnonymous()
        {
            return WithCustomAttribute<AllowAnonymousAttribute>();
        }

        public virtual FluentAction<TP, TR, TU1> Using<TU1>(FluentActionUsingDefinition usingDefinition)
        {
            return new FluentAction<TP, TR, TU1>(Definition, usingDefinition);
        }

        public virtual FluentAction<TP, TR, TU1> UsingBody<TU1>()
        {
            return new FluentAction<TP, TR, TU1>(Definition, new FluentActionUsingBodyDefinition
            {
                Type = typeof(TU1)
            });
        }

        public virtual FluentAction<TP, TR, TU1> UsingBody<TU1>(TU1 defaultValue)
        {
            return new FluentAction<TP, TR, TU1>(Definition, new FluentActionUsingBodyDefinition
            {
                Type = typeof(TU1),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentAction<TP, TR, TU1> UsingForm<TU1>()
        {
            return new FluentAction<TP, TR, TU1>(Definition, new FluentActionUsingFormDefinition
            {
                Type = typeof(TU1)
            });
        }

        public virtual FluentAction<TP, TR, TU1> UsingForm<TU1>(TU1 defaultValue)
        {
            return new FluentAction<TP, TR, TU1>(Definition, new FluentActionUsingFormDefinition
            {
                Type = typeof(TU1),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentAction<TP, TR, IFormFile> UsingFormFile(string name)
        {
            return new FluentAction<TP, TR, IFormFile>(Definition, new FluentActionUsingFormFileDefinition
            {
                Type = typeof(IFormFile),
                Name = name
            });
        }

        public virtual FluentAction<TP, TR, IEnumerable<IFormFile>> UsingFormFiles(string name)
        {
            return new FluentAction<TP, TR, IEnumerable<IFormFile>>(Definition, new FluentActionUsingFormFilesDefinition
            {
                Type = typeof(IEnumerable<IFormFile>),
                Name = name
            });
        }

        public virtual FluentAction<TP, TR, TU1> UsingFormValue<TU1>(string key)
        {
            return new FluentAction<TP, TR, TU1>(Definition, new FluentActionUsingFormValueDefinition
            {
                Type = typeof(TU1),
                Key = key
            });
        }

        public virtual FluentAction<TP, TR, TU1> UsingFormValue<TU1>(string key, TU1 defaultValue)
        {
            return new FluentAction<TP, TR, TU1>(Definition, new FluentActionUsingFormValueDefinition
            {
                Type = typeof(TU1),
                Key = key,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentAction<TP, TR, TU1> UsingHeader<TU1>(string name)
        {
            return new FluentAction<TP, TR, TU1>(Definition, new FluentActionUsingHeaderParameterDefinition
            {
                Type = typeof(TU1),
                Name = name
            });
        }

        public virtual FluentAction<TP, TR, TU1> UsingHeader<TU1>(string name, TU1 defaultValue)
        {
            return new FluentAction<TP, TR, TU1>(Definition, new FluentActionUsingHeaderParameterDefinition
            {
                Type = typeof(TU1),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentAction<TP, TR, HttpContext> UsingHttpContext()
        {
            return new FluentAction<TP, TR, HttpContext>(Definition, new FluentActionUsingHttpContextDefinition
            {
                Type = typeof(HttpContext)
            });
        }

        public virtual FluentAction<TP, TR, TU1> UsingModelBinder<TU1>(Type modelBinderType, string parameterName = null)
        {
            return new FluentAction<TP, TR, TU1>(Definition, new FluentActionUsingModelBinderDefinition
            {
                Type = typeof(TU1),
                ModelBinderType = modelBinderType,
                ParameterName = parameterName
            });
        }

        public virtual FluentAction<TP, TR, TU1> UsingModelBinder<TU1>(Type modelBinderType, string parameterName, TU1 defaultValue)
        {
            return new FluentAction<TP, TR, TU1>(Definition, new FluentActionUsingModelBinderDefinition
            {
                Type = typeof(TU1),
                ModelBinderType = modelBinderType,
                HasDefaultValue = true,
                DefaultValue = defaultValue,
                ParameterName = parameterName
            });
        }

        public virtual FluentAction<TP, TR, ModelStateDictionary> UsingModelState()
        {
            return new FluentAction<TP, TR, ModelStateDictionary>(Definition, new FluentActionUsingModelStateDefinition
            {
                Type = typeof(ModelStateDictionary)
            });
        }

        public virtual FluentAction<TP, TR, TP> UsingParent()
        {
            return new FluentAction<TP, TR, TP>(Definition, new FluentActionUsingParentDefinition
            {
                Type = typeof(TR)
            });
        }

        public virtual FluentAction<TP, TR, TP2> UsingParent<TP2>()
        {
            return new FluentAction<TP, TR, TP2>(Definition, new FluentActionUsingParentDefinition
            {
                Type = typeof(TP2)
            });
        }

        public virtual FluentAction<TP, TR, TU1> UsingQueryStringParameter<TU1>(string name)
        {
            return new FluentAction<TP, TR, TU1>(Definition, new FluentActionUsingQueryStringParameterDefinition
            {
                Type = typeof(TU1),
                Name = name
            });
        }

        public virtual FluentAction<TP, TR, TU1> UsingQueryStringParameter<TU1>(string name, TU1 defaultValue)
        {
            return new FluentAction<TP, TR, TU1>(Definition, new FluentActionUsingQueryStringParameterDefinition
            {
                Type = typeof(TU1),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentAction<TP, TR, HttpResponse> UsingResponse()
        {
            return new FluentAction<TP, TR, HttpResponse>(Definition, new FluentActionUsingResponseDefinition
            {
                Type = typeof(HttpResponse)
            });
        }

        public virtual FluentAction<TP, TR, TR> UsingResult()
        {
            return new FluentAction<TP, TR, TR>(Definition, new FluentActionUsingResultDefinition
            {
                Type = typeof(TR)
            });
        }

        public virtual FluentAction<TP, TR, TU1> UsingRouteParameter<TU1>(string name)
        {
            return new FluentAction<TP, TR, TU1>(Definition, new FluentActionUsingRouteParameterDefinition
            {
                Type = typeof(TU1),
                Name = name
            });
        }

        public virtual FluentAction<TP, TR, TU1> UsingRouteParameter<TU1>(string name, TU1 defaultValue)
        {
            return new FluentAction<TP, TR, TU1>(Definition, new FluentActionUsingRouteParameterDefinition
            {
                Type = typeof(TU1),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentAction<TP, TR, TU1> UsingService<TU1>()
        {
            return new FluentAction<TP, TR, TU1>(Definition, new FluentActionUsingServiceDefinition
            {
                Type = typeof(TU1)
            });
        }

        public virtual FluentAction<TP, TR, TU1> UsingService<TU1>(TU1 defaultValue)
        {
            return new FluentAction<TP, TR, TU1>(Definition, new FluentActionUsingServiceDefinition
            {
                Type = typeof(TU1),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentAction<TP, TR, ITempDataDictionary> UsingTempData()
        {
            return new FluentAction<TP, TR, ITempDataDictionary>(Definition, new FluentActionUsingTempDataDefinition
            {
                Type = typeof(ITempDataDictionary)
            });
        }

        public virtual FluentAction<TP, TR, dynamic> UsingViewBag()
        {
            return new FluentAction<TP, TR, dynamic>(Definition, new FluentActionUsingViewBagDefinition
            {
                Type = typeof(object)
            });
        }

        public virtual FluentAction<TP, TR, ViewDataDictionary> UsingViewData()
        {
            return new FluentAction<TP, TR, ViewDataDictionary>(Definition, new FluentActionUsingViewDataDefinition
            {
                Type = typeof(ViewDataDictionary)
            });
        }

        public FluentAction<TP, object> Do(Action handlerAction)
        {
            Definition.ExistingOrNewHandlerDraft.Type = FluentActionHandlerType.Action;
            Definition.ExistingOrNewHandlerDraft.Delegate = handlerAction;
            Definition.CommitHandlerDraft();
            return new FluentAction<TP, object>(Definition);
        }

        public FluentAction<TP, object> DoAsync(Func<Task> asyncHandlerAction)
        {
            Definition.ExistingOrNewHandlerDraft.Type = FluentActionHandlerType.Action;
            Definition.ExistingOrNewHandlerDraft.Delegate = asyncHandlerAction;
            Definition.ExistingOrNewHandlerDraft.Async = true;
            Definition.CommitHandlerDraft();
            return new FluentAction<TP, object>(Definition);
        }

        public virtual FluentActionWithMvcController<TC> ToMvcController<TC>() where TC : Controller
        {
            return new FluentActionWithMvcController<TC>(Definition, new FluentActionUsingMvcControllerDefinition
            {
                Type = typeof(TC)
            });
        }

        public FluentAction<TP, TR2> To<TR2>(Func<TR2> handlerFunc)
        {
            return new FluentAction<TP, TR2>(Definition, handlerFunc);
        }

        public FluentAction<TP, TR2> To<TR2>(Func<Task<TR2>> asyncHandlerFunc)
        {
            return new FluentAction<TP, TR2>(Definition, asyncHandlerFunc, async: true);
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
    }

    public class FluentAction<TP, TR, TU1> : FluentActionBase
    {
        public FluentAction(FluentActionDefinition fluentActionDefinition, FluentActionUsingDefinition usingDefinition) 
            : base(fluentActionDefinition)
        {
            Definition.ExistingOrNewHandlerDraft.Usings.Add(usingDefinition);
        }

        public virtual FluentAction<TP, TR, TU1, TU2> Using<TU2>(FluentActionUsingDefinition usingDefinition)
        {
            return new FluentAction<TP, TR, TU1, TU2>(Definition, usingDefinition);
        }

        public virtual FluentAction<TP, TR, TU1, TP> UsingParent()
        {
            return new FluentAction<TP, TR, TU1, TP>(Definition, new FluentActionUsingParentDefinition
            {
                Type = typeof(TR)
            });
        }

        public virtual FluentAction<TP, TR, TU1, TP2> UsingParent<TP2>()
        {
            return new FluentAction<TP, TR, TU1, TP2>(Definition, new FluentActionUsingParentDefinition
            {
                Type = typeof(TP2)
            });
        }

        public virtual FluentAction<TP, TR, TU1, TR> UsingResult()
        {
            return new FluentAction<TP, TR, TU1, TR>(Definition, new FluentActionUsingResultDefinition
            {
                Type = typeof(TR)
            });
        }

        public FluentAction<TP, TR, TU1, TU2> UsingService<TU2>()
        {
            return new FluentAction<TP, TR, TU1, TU2>(Definition, new FluentActionUsingServiceDefinition
            {
                Type = typeof(TU2)
            });
        }

        public FluentAction<TP, TR, TU1, TU2> UsingService<TU2>(TU2 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2>(Definition, new FluentActionUsingServiceDefinition
            {
                Type = typeof(TU2),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public FluentAction<TP, TR, TU1, TU2> UsingRouteParameter<TU2>(string name)
        {
            return new FluentAction<TP, TR, TU1, TU2>(Definition, new FluentActionUsingRouteParameterDefinition
            {
                Type = typeof(TU2),
                Name = name
            });
        }

        public FluentAction<TP, TR, TU1, TU2> UsingRouteParameter<TU2>(string name, TU2 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2>(Definition, new FluentActionUsingRouteParameterDefinition
            {
                Type = typeof(TU2),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2> UsingQueryStringParameter<TU2>(string name)
        {
            return new FluentAction<TP, TR, TU1, TU2>(Definition, new FluentActionUsingQueryStringParameterDefinition
            {
                Type = typeof(TU2),
                Name = name
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2> UsingQueryStringParameter<TU2>(string name, TU2 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2>(Definition, new FluentActionUsingQueryStringParameterDefinition
            {
                Type = typeof(TU2),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2> UsingHeader<TU2>(string name)
        {
            return new FluentAction<TP, TR, TU1, TU2>(Definition, new FluentActionUsingHeaderParameterDefinition
            {
                Type = typeof(TU2),
                Name = name
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2> UsingHeader<TU2>(string name, TU2 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2>(Definition, new FluentActionUsingHeaderParameterDefinition
            {
                Type = typeof(TU2),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public FluentAction<TP, TR, TU1, TU2> UsingBody<TU2>()
        {
            return new FluentAction<TP, TR, TU1, TU2>(Definition, new FluentActionUsingBodyDefinition
            {
                Type = typeof(TU2)
            });
        }

        public FluentAction<TP, TR, TU1, TU2> UsingBody<TU2>(TU2 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2>(Definition, new FluentActionUsingBodyDefinition
            {
                Type = typeof(TU2),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2> UsingForm<TU2>()
        {
            return new FluentAction<TP, TR, TU1, TU2>(Definition, new FluentActionUsingFormDefinition
            {
                Type = typeof(TU2)
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2> UsingForm<TU2>(TU2 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2>(Definition, new FluentActionUsingFormDefinition
            {
                Type = typeof(TU2),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentAction<TP, TR, TU1, IFormFile> UsingFormFile(string name)
        {
            return new FluentAction<TP, TR, TU1, IFormFile>(Definition, new FluentActionUsingFormFileDefinition
            {
                Type = typeof(IFormFile),
                Name = name
            });
        }

        public virtual FluentAction<TP, TR, TU1, IEnumerable<IFormFile>> UsingFormFiles(string name)
        {
            return new FluentAction<TP, TR, TU1, IEnumerable<IFormFile>>(Definition, new FluentActionUsingFormFilesDefinition
            {
                Type = typeof(IEnumerable<IFormFile>),
                Name = name
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2> UsingFormValue<TU2>(string key)
        {
            return new FluentAction<TP, TR, TU1, TU2>(Definition, new FluentActionUsingFormValueDefinition
            {
                Type = typeof(TU2),
                Key = key
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2> UsingFormValue<TU2>(string key, TU2 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2>(Definition, new FluentActionUsingFormValueDefinition
            {
                Type = typeof(TU2),
                Key = key,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2> UsingModelBinder<TU2>(Type modelBinderType, string parameterName = null)
        {
            return new FluentAction<TP, TR, TU1, TU2>(Definition, new FluentActionUsingModelBinderDefinition
            {
                Type = typeof(TU2),
                ModelBinderType = modelBinderType,
                ParameterName = parameterName
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2> UsingModelBinder<TU2>(Type modelBinderType, string parameterName, TU2 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2>(Definition, new FluentActionUsingModelBinderDefinition
            {
                Type = typeof(TU2),
                ModelBinderType = modelBinderType,
                HasDefaultValue = true,
                DefaultValue = defaultValue,
                ParameterName = parameterName
            });
        }

        public virtual FluentAction<TP, TR, TU1, ModelStateDictionary> UsingModelState()
        {
            return new FluentAction<TP, TR, TU1, ModelStateDictionary>(Definition, new FluentActionUsingModelStateDefinition
            {
                Type = typeof(ModelStateDictionary)
            });
        }

        public virtual FluentAction<TP, TR, TU1, HttpContext> UsingHttpContext()
        {
            return new FluentAction<TP, TR, TU1, HttpContext>(Definition, new FluentActionUsingHttpContextDefinition
            {
                Type = typeof(HttpContext)
            });
        }

        public virtual FluentAction<TP, TR, TU1, dynamic> UsingViewBag()
        {
            return new FluentAction<TP, TR, TU1, dynamic>(Definition, new FluentActionUsingViewBagDefinition
            {
                Type = typeof(object)
            });
        }

        public virtual FluentAction<TP, TR, TU1, ViewDataDictionary> UsingViewData()
        {
            return new FluentAction<TP, TR, TU1, ViewDataDictionary>(Definition, new FluentActionUsingViewDataDefinition
            {
                Type = typeof(ViewDataDictionary)
            });
        }

        public virtual FluentAction<TP, TR, TU1, ITempDataDictionary> UsingTempData()
        {
            return new FluentAction<TP, TR, TU1, ITempDataDictionary>(Definition, new FluentActionUsingTempDataDefinition
            {
                Type = typeof(ITempDataDictionary)
            });
        }

        public FluentAction<TP, object> Do(Action<TU1> handlerAction)
        {
            Definition.ExistingOrNewHandlerDraft.Type = FluentActionHandlerType.Action;
            Definition.ExistingOrNewHandlerDraft.Delegate = handlerAction;
            Definition.CommitHandlerDraft();
            return new FluentAction<TP, object>(Definition);
        }

        public FluentAction<TP, object> DoAsync(Func<TU1, Task> asyncHandlerAction)
        {
            Definition.ExistingOrNewHandlerDraft.Type = FluentActionHandlerType.Action;
            Definition.ExistingOrNewHandlerDraft.Delegate = asyncHandlerAction;
            Definition.ExistingOrNewHandlerDraft.Async = true;
            Definition.CommitHandlerDraft();
            return new FluentAction<TP, object>(Definition);
        }

        public virtual FluentActionWithMvcController<TU1, TC> ToMvcController<TC>() where TC : Controller
        {
            return new FluentActionWithMvcController<TU1, TC>(Definition, new FluentActionUsingMvcControllerDefinition
            {
                Type = typeof(TC)
            });
        }

        public FluentAction<TP, TR2> To<TR2>(Func<TU1, TR2> handlerFunc)
        {
            return new FluentAction<TP, TR2>(Definition, handlerFunc);
        }

        public FluentAction<TP, TR2> To<TR2>(Func<TU1, Task<TR2>> asyncHandlerFunc)
        {
            return new FluentAction<TP, TR2>(Definition, asyncHandlerFunc, async: true);
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
    }

    public class FluentAction<TP, TR, TU1, TU2> : FluentActionBase
    {
        public FluentAction(FluentActionDefinition fluentActionDefinition, FluentActionUsingDefinition usingDefinition) : base(fluentActionDefinition)
        {
            Definition.ExistingOrNewHandlerDraft.Usings.Add(usingDefinition);
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3> Using<TU3>(FluentActionUsingDefinition usingDefinition)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3>(Definition, usingDefinition);
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TP> UsingParent()
        {
            return new FluentAction<TP, TR, TU1, TU2, TP>(Definition, new FluentActionUsingParentDefinition
            {
                Type = typeof(TR)
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TP2> UsingParent<TP2>()
        {
            return new FluentAction<TP, TR, TU1, TU2, TP2>(Definition, new FluentActionUsingParentDefinition
            {
                Type = typeof(TP2)
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TR> UsingResult()
        {
            return new FluentAction<TP, TR, TU1, TU2, TR>(Definition, new FluentActionUsingResultDefinition
            {
                Type = typeof(TR)
            });
        }

        public FluentAction<TP, TR, TU1, TU2, TU3> UsingService<TU3>()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3>(Definition, new FluentActionUsingServiceDefinition
            {
                Type = typeof(TU3)
            });
        }

        public FluentAction<TP, TR, TU1, TU2, TU3> UsingService<TU3>(TU3 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3>(Definition, new FluentActionUsingServiceDefinition
            {
                Type = typeof(TU3),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public FluentAction<TP, TR, TU1, TU2, TU3> UsingRouteParameter<TU3>(string name)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3>(Definition, new FluentActionUsingRouteParameterDefinition
            {
                Type = typeof(TU3),
                Name = name
            });
        }

        public FluentAction<TP, TR, TU1, TU2, TU3> UsingRouteParameter<TU3>(string name, TU3 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3>(Definition, new FluentActionUsingRouteParameterDefinition
            {
                Type = typeof(TU3),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3> UsingQueryStringParameter<TU3>(string name)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3>(Definition, new FluentActionUsingQueryStringParameterDefinition
            {
                Type = typeof(TU3),
                Name = name
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3> UsingQueryStringParameter<TU3>(string name, TU3 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3>(Definition, new FluentActionUsingQueryStringParameterDefinition
            {
                Type = typeof(TU3),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3> UsingHeader<TU3>(string name)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3>(Definition, new FluentActionUsingHeaderParameterDefinition
            {
                Type = typeof(TU3),
                Name = name
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3> UsingHeader<TU3>(string name, TU3 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3>(Definition, new FluentActionUsingHeaderParameterDefinition
            {
                Type = typeof(TU3),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public FluentAction<TP, TR, TU1, TU2, TU3> UsingBody<TU3>()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3>(Definition, new FluentActionUsingBodyDefinition
            {
                Type = typeof(TU3)
            });
        }

        public FluentAction<TP, TR, TU1, TU2, TU3> UsingBody<TU3>(TU3 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3>(Definition, new FluentActionUsingBodyDefinition
            {
                Type = typeof(TU3),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3> UsingForm<TU3>()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3>(Definition, new FluentActionUsingFormDefinition
            {
                Type = typeof(TU3)
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3> UsingForm<TU3>(TU3 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3>(Definition, new FluentActionUsingFormDefinition
            {
                Type = typeof(TU3),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, IFormFile> UsingFormFile(string name)
        {
            return new FluentAction<TP, TR, TU1, TU2, IFormFile>(Definition, new FluentActionUsingFormFileDefinition
            {
                Type = typeof(IFormFile),
                Name = name
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, IEnumerable<IFormFile>> UsingFormFiles(string name)
        {
            return new FluentAction<TP, TR, TU1, TU2, IEnumerable<IFormFile>>(Definition, new FluentActionUsingFormFilesDefinition
            {
                Type = typeof(IEnumerable<IFormFile>),
                Name = name
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3> UsingFormValue<TU3>(string key)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3>(Definition, new FluentActionUsingFormValueDefinition
            {
                Type = typeof(TU3),
                Key = key
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3> UsingFormValue<TU3>(string key, TU3 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3>(Definition, new FluentActionUsingFormValueDefinition
            {
                Type = typeof(TU3),
                Key = key,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3> UsingModelBinder<TU3>(Type modelBinderType, string parameterName = null)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3>(Definition, new FluentActionUsingModelBinderDefinition
            {
                Type = typeof(TU3),
                ModelBinderType = modelBinderType,
                ParameterName = parameterName
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3> UsingModelBinder<TU3>(Type modelBinderType, string parameterName, TU3 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3>(Definition, new FluentActionUsingModelBinderDefinition
            {
                Type = typeof(TU3),
                ModelBinderType = modelBinderType,
                HasDefaultValue = true,
                DefaultValue = defaultValue,
                ParameterName = parameterName
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, ModelStateDictionary> UsingModelState()
        {
            return new FluentAction<TP, TR, TU1, TU2, ModelStateDictionary>(Definition, new FluentActionUsingModelStateDefinition
            {
                Type = typeof(ModelStateDictionary)
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, HttpContext> UsingHttpContext()
        {
            return new FluentAction<TP, TR, TU1, TU2, HttpContext>(Definition, new FluentActionUsingHttpContextDefinition
            {
                Type = typeof(HttpContext)
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, dynamic> UsingViewBag()
        {
            return new FluentAction<TP, TR, TU1, TU2, dynamic>(Definition, new FluentActionUsingViewBagDefinition
            {
                Type = typeof(object)
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, ViewDataDictionary> UsingViewData()
        {
            return new FluentAction<TP, TR, TU1, TU2, ViewDataDictionary>(Definition, new FluentActionUsingViewDataDefinition
            {
                Type = typeof(ViewDataDictionary)
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, ITempDataDictionary> UsingTempData()
        {
            return new FluentAction<TP, TR, TU1, TU2, ITempDataDictionary>(Definition, new FluentActionUsingTempDataDefinition
            {
                Type = typeof(ITempDataDictionary)
            });
        }

        public FluentAction<TP, object> Do(Action<TU1, TU2> handlerAction)
        {
            Definition.ExistingOrNewHandlerDraft.Type = FluentActionHandlerType.Action;
            Definition.ExistingOrNewHandlerDraft.Delegate = handlerAction;
            Definition.CommitHandlerDraft();
            return new FluentAction<TP, object>(Definition);
        }

        public FluentAction<TP, object> DoAsync(Func<TU1, TU2, Task> asyncHandlerAction)
        {
            Definition.ExistingOrNewHandlerDraft.Type = FluentActionHandlerType.Action;
            Definition.ExistingOrNewHandlerDraft.Delegate = asyncHandlerAction;
            Definition.ExistingOrNewHandlerDraft.Async = true;
            Definition.CommitHandlerDraft();
            return new FluentAction<TP, object>(Definition);
        }

        public virtual FluentActionWithMvcController<TU1, TU2, TC> ToMvcController<TC>() where TC : Controller
        {
            return new FluentActionWithMvcController<TU1, TU2, TC>(Definition, new FluentActionUsingMvcControllerDefinition
            {
                Type = typeof(TC)
            });
        }

        public FluentAction<TP, TR2> To<TR2>(Func<TU1, TU2, TR2> handlerFunc)
        {
            return new FluentAction<TP, TR2>(Definition, handlerFunc);
        }

        public FluentAction<TP, TR2> To<TR2>(Func<TU1, TU2, Task<TR2>> asyncHandlerFunc)
        {
            return new FluentAction<TP, TR2>(Definition, asyncHandlerFunc, async: true);
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
    }

    public class FluentAction<TP, TR, TU1, TU2, TU3> : FluentActionBase
    {
        public FluentAction(FluentActionDefinition fluentActionDefinition, FluentActionUsingDefinition usingDefinition) 
            : base(fluentActionDefinition)
        {
            Definition.ExistingOrNewHandlerDraft.Usings.Add(usingDefinition);
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4> Using<TU4>(FluentActionUsingDefinition usingDefinition)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4>(Definition, usingDefinition);
        }

        public FluentAction<TP, TR, TU1, TU2, TU3, TU4> UsingService<TU4>()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4>(Definition, new FluentActionUsingServiceDefinition
            {
                Type = typeof(TU4)
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TP> UsingParent()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TP>(Definition, new FluentActionUsingParentDefinition
            {
                Type = typeof(TR)
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TP2> UsingParent<TP2>()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TP2>(Definition, new FluentActionUsingParentDefinition
            {
                Type = typeof(TP2)
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TR> UsingResult()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TR>(Definition, new FluentActionUsingResultDefinition
            {
                Type = typeof(TR)
            });
        }

        public FluentAction<TP, TR, TU1, TU2, TU3, TU4> UsingService<TU4>(TU4 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4>(Definition, new FluentActionUsingServiceDefinition
            {
                Type = typeof(TU4),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public FluentAction<TP, TR, TU1, TU2, TU3, TU4> UsingRouteParameter<TU4>(string name)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4>(Definition, new FluentActionUsingRouteParameterDefinition
            {
                Type = typeof(TU4),
                Name = name
            });
        }

        public FluentAction<TP, TR, TU1, TU2, TU3, TU4> UsingRouteParameter<TU4>(string name, TU4 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4>(Definition, new FluentActionUsingRouteParameterDefinition
            {
                Type = typeof(TU4),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4> UsingQueryStringParameter<TU4>(string name)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4>(Definition, new FluentActionUsingQueryStringParameterDefinition
            {
                Type = typeof(TU4),
                Name = name
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4> UsingQueryStringParameter<TU4>(string name, TU4 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4>(Definition, new FluentActionUsingQueryStringParameterDefinition
            {
                Type = typeof(TU4),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4> UsingHeader<TU4>(string name)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4>(Definition, new FluentActionUsingHeaderParameterDefinition
            {
                Type = typeof(TU4),
                Name = name
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4> UsingHeader<TU4>(string name, TU4 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4>(Definition, new FluentActionUsingHeaderParameterDefinition
            {
                Type = typeof(TU4),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public FluentAction<TP, TR, TU1, TU2, TU3, TU4> UsingBody<TU4>()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4>(Definition, new FluentActionUsingBodyDefinition
            {
                Type = typeof(TU4)
            });
        }

        public FluentAction<TP, TR, TU1, TU2, TU3, TU4> UsingBody<TU4>(TU4 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4>(Definition, new FluentActionUsingBodyDefinition
            {
                Type = typeof(TU4),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4> UsingForm<TU4>()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4>(Definition, new FluentActionUsingFormDefinition
            {
                Type = typeof(TU4)
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4> UsingForm<TU4>(TU4 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4>(Definition, new FluentActionUsingFormDefinition
            {
                Type = typeof(TU4),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, IFormFile> UsingFormFile(string name)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, IFormFile>(Definition, new FluentActionUsingFormFileDefinition
            {
                Type = typeof(IFormFile),
                Name = name
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, IEnumerable<IFormFile>> UsingFormFiles(string name)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, IEnumerable<IFormFile>>(Definition, new FluentActionUsingFormFilesDefinition
            {
                Type = typeof(IEnumerable<IFormFile>),
                Name = name
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4> UsingFormValue<TU4>(string key)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4>(Definition, new FluentActionUsingFormValueDefinition
            {
                Type = typeof(TU4),
                Key = key
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4> UsingFormValue<TU4>(string key, TU4 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4>(Definition, new FluentActionUsingFormValueDefinition
            {
                Type = typeof(TU4),
                Key = key,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4> UsingModelBinder<TU4>(Type modelBinderType, string parameterName = null)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4>(Definition, new FluentActionUsingModelBinderDefinition
            {
                Type = typeof(TU4),
                ModelBinderType = modelBinderType,
                ParameterName = parameterName
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4> UsingModelBinder<TU4>(Type modelBinderType, string parameterName, TU4 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4>(Definition, new FluentActionUsingModelBinderDefinition
            {
                Type = typeof(TU4),
                ModelBinderType = modelBinderType,
                HasDefaultValue = true,
                DefaultValue = defaultValue,
                ParameterName = parameterName
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, ModelStateDictionary> UsingModelState()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, ModelStateDictionary>(Definition, new FluentActionUsingModelStateDefinition
            {
                Type = typeof(ModelStateDictionary)
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, HttpContext> UsingHttpContext()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, HttpContext>(Definition, new FluentActionUsingHttpContextDefinition
            {
                Type = typeof(HttpContext)
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, dynamic> UsingViewBag()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, dynamic>(Definition, new FluentActionUsingViewBagDefinition
            {
                Type = typeof(object)
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, ViewDataDictionary> UsingViewData()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, ViewDataDictionary>(Definition, new FluentActionUsingViewDataDefinition
            {
                Type = typeof(ViewDataDictionary)
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, ITempDataDictionary> UsingTempData()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, ITempDataDictionary>(Definition, new FluentActionUsingTempDataDefinition
            {
                Type = typeof(ITempDataDictionary)
            });
        }

        public FluentAction<TP, object> Do(Action<TU1, TU2, TU3> handlerAction)
        {
            Definition.ExistingOrNewHandlerDraft.Type = FluentActionHandlerType.Action;
            Definition.ExistingOrNewHandlerDraft.Delegate = handlerAction;
            Definition.CommitHandlerDraft();
            return new FluentAction<TP, object>(Definition);
        }

        public FluentAction<TP, object> DoAsync(Func<TU1, TU2, TU3, Task> asyncHandlerAction)
        {
            Definition.ExistingOrNewHandlerDraft.Type = FluentActionHandlerType.Action;
            Definition.ExistingOrNewHandlerDraft.Delegate = asyncHandlerAction;
            Definition.ExistingOrNewHandlerDraft.Async = true;
            Definition.CommitHandlerDraft();
            return new FluentAction<TP, object>(Definition);
        }

        public virtual FluentActionWithMvcController<TU1, TU2, TU3, TC> ToMvcController<TC>() where TC : Controller
        {
            return new FluentActionWithMvcController<TU1, TU2, TU3, TC>(Definition, new FluentActionUsingMvcControllerDefinition
            {
                Type = typeof(TC)
            });
        }

        public FluentAction<TP, TR2> To<TR2>(Func<TU1, TU2, TU3, TR2> handlerFunc)
        {
            return new FluentAction<TP, TR2>(Definition, handlerFunc);
        }

        public FluentAction<TP, TR2> To<TR2>(Func<TU1, TU2, TU3, Task<TR2>> asyncHandlerFunc)
        {
            return new FluentAction<TP, TR2>(Definition, asyncHandlerFunc, async: true);
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
    }

    public class FluentAction<TP, TR, TU1, TU2, TU3, TU4> : FluentActionBase
    {
        public FluentAction(FluentActionDefinition fluentActionDefinition, FluentActionUsingDefinition usingDefinition) 
            : base(fluentActionDefinition)
        {
            Definition.ExistingOrNewHandlerDraft.Usings.Add(usingDefinition);
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5> Using<TU5>(FluentActionUsingDefinition usingDefinition)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5>(Definition, usingDefinition);
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TP> UsingParent()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TP>(Definition, new FluentActionUsingParentDefinition
            {
                Type = typeof(TR)
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TP2> UsingParent<TP2>()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TP2>(Definition, new FluentActionUsingParentDefinition
            {
                Type = typeof(TP2)
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TR> UsingResult()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TR>(Definition, new FluentActionUsingResultDefinition
            {
                Type = typeof(TR)
            });
        }

        public FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5> UsingService<TU5>()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5>(Definition, new FluentActionUsingServiceDefinition
            {
                Type = typeof(TU5)
            });
        }

        public FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5> UsingService<TU5>(TU5 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5>(Definition, new FluentActionUsingServiceDefinition
            {
                Type = typeof(TU5),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5> UsingRouteParameter<TU5>(string name)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5>(Definition, new FluentActionUsingRouteParameterDefinition
            {
                Type = typeof(TU5),
                Name = name
            });
        }

        public FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5> UsingRouteParameter<TU5>(string name, TU5 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5>(Definition, new FluentActionUsingRouteParameterDefinition
            {
                Type = typeof(TU5),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5> UsingQueryStringParameter<TU5>(string name)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5>(Definition, new FluentActionUsingQueryStringParameterDefinition
            {
                Type = typeof(TU5),
                Name = name
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5> UsingQueryStringParameter<TU5>(string name, TU5 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5>(Definition, new FluentActionUsingQueryStringParameterDefinition
            {
                Type = typeof(TU5),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5> UsingHeader<TU5>(string name)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5>(Definition, new FluentActionUsingHeaderParameterDefinition
            {
                Type = typeof(TU5),
                Name = name
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5> UsingHeader<TU5>(string name, TU5 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5>(Definition, new FluentActionUsingHeaderParameterDefinition
            {
                Type = typeof(TU5),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5> UsingBody<TU5>()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5>(Definition, new FluentActionUsingBodyDefinition
            {
                Type = typeof(TU5)
            });
        }

        public FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5> UsingBody<TU5>(TU5 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5>(Definition, new FluentActionUsingBodyDefinition
            {
                Type = typeof(TU5),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5> UsingForm<TU5>()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5>(Definition, new FluentActionUsingFormDefinition
            {
                Type = typeof(TU5)
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5> UsingForm<TU5>(TU5 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5>(Definition, new FluentActionUsingFormDefinition
            {
                Type = typeof(TU5),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, IFormFile> UsingFormFile(string name)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, IFormFile>(Definition, new FluentActionUsingFormFileDefinition
            {
                Type = typeof(IFormFile),
                Name = name
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, IEnumerable<IFormFile>> UsingFormFiles(string name)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, IEnumerable<IFormFile>>(Definition, new FluentActionUsingFormFilesDefinition
            {
                Type = typeof(IEnumerable<IFormFile>),
                Name = name
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5> UsingFormValue<TU5>(string key)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5>(Definition, new FluentActionUsingFormValueDefinition
            {
                Type = typeof(TU5),
                Key = key
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5> UsingFormValue<TU5>(string key, TU5 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5>(Definition, new FluentActionUsingFormValueDefinition
            {
                Type = typeof(TU5),
                Key = key,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5> UsingModelBinder<TU5>(Type modelBinderType, string parameterName = null)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5>(Definition, new FluentActionUsingModelBinderDefinition
            {
                Type = typeof(TU5),
                ModelBinderType = modelBinderType,
                ParameterName = parameterName
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5> UsingModelBinder<TU5>(Type modelBinderType, string parameterName, TU5 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5>(Definition, new FluentActionUsingModelBinderDefinition
            {
                Type = typeof(TU5),
                ModelBinderType = modelBinderType,
                HasDefaultValue = true,
                DefaultValue = defaultValue,
                ParameterName = parameterName
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, ModelStateDictionary> UsingModelState()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, ModelStateDictionary>(Definition, new FluentActionUsingModelStateDefinition
            {
                Type = typeof(ModelStateDictionary)
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, HttpContext> UsingHttpContext()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, HttpContext>(Definition, new FluentActionUsingHttpContextDefinition
            {
                Type = typeof(HttpContext)
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, dynamic> UsingViewBag()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, dynamic>(Definition, new FluentActionUsingViewBagDefinition
            {
                Type = typeof(object)
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, ViewDataDictionary> UsingViewData()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, ViewDataDictionary>(Definition, new FluentActionUsingViewDataDefinition
            {
                Type = typeof(ViewDataDictionary)
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, ITempDataDictionary> UsingTempData()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, ITempDataDictionary>(Definition, new FluentActionUsingTempDataDefinition
            {
                Type = typeof(ITempDataDictionary)
            });
        }

        public FluentAction<TP, object> Do(Action<TU1, TU2, TU3, TU4> handlerAction)
        {
            Definition.ExistingOrNewHandlerDraft.Type = FluentActionHandlerType.Action;
            Definition.ExistingOrNewHandlerDraft.Delegate = handlerAction;
            Definition.CommitHandlerDraft();
            return new FluentAction<TP, object>(Definition);
        }

        public FluentAction<TP, object> DoAsync(Func<TU1, TU2, TU3, TU4, Task> asyncHandlerAction)
        {
            Definition.ExistingOrNewHandlerDraft.Type = FluentActionHandlerType.Action;
            Definition.ExistingOrNewHandlerDraft.Delegate = asyncHandlerAction;
            Definition.ExistingOrNewHandlerDraft.Async = true;
            Definition.CommitHandlerDraft();
            return new FluentAction<TP, object>(Definition);
        }

        public virtual FluentActionWithMvcController<TU1, TU2, TU3, TU4, TC> ToMvcController<TC>() where TC : Controller
        {
            return new FluentActionWithMvcController<TU1, TU2, TU3, TU4, TC>(Definition, new FluentActionUsingMvcControllerDefinition
            {
                Type = typeof(TC)
            });
        }

        public FluentAction<TP, TR2> To<TR2>(Func<TU1, TU2, TU3, TU4, TR2> handlerFunc)
        {
            return new FluentAction<TP, TR2>(Definition, handlerFunc);
        }

        public FluentAction<TP, TR2> To<TR2>(Func<TU1, TU2, TU3, TU4, Task<TR2>> asyncHandlerFunc)
        {
            return new FluentAction<TP, TR2>(Definition, asyncHandlerFunc, async: true);
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
    }

    public class FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5> : FluentActionBase
    {
        public FluentAction(FluentActionDefinition fluentActionDefinition, FluentActionUsingDefinition usingDefinition) : base(fluentActionDefinition)
        {
            Definition.ExistingOrNewHandlerDraft.Usings.Add(usingDefinition);
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6> Using<TU6>(FluentActionUsingDefinition usingDefinition)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6>(Definition, usingDefinition);
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TP> UsingParent()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TP>(Definition, new FluentActionUsingParentDefinition
            {
                Type = typeof(TR)
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TP2> UsingParent<TP2>()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TP2>(Definition, new FluentActionUsingParentDefinition
            {
                Type = typeof(TP2)
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TR> UsingResult()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TR>(Definition, new FluentActionUsingResultDefinition
            {
                Type = typeof(TR)
            });
        }

        public FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6> UsingService<TU6>()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6>(Definition, new FluentActionUsingServiceDefinition
            {
                Type = typeof(TU6)
            });
        }

        public FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6> UsingService<TU6>(TU6 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6>(Definition, new FluentActionUsingServiceDefinition
            {
                Type = typeof(TU6),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6> UsingRouteParameter<TU6>(string name)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6>(Definition, new FluentActionUsingRouteParameterDefinition
            {
                Type = typeof(TU6),
                Name = name
            });
        }

        public FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6> UsingRouteParameter<TU6>(string name, TU6 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6>(Definition, new FluentActionUsingRouteParameterDefinition
            {
                Type = typeof(TU6),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6> UsingQueryStringParameter<TU6>(string name)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6>(Definition, new FluentActionUsingQueryStringParameterDefinition
            {
                Type = typeof(TU6),
                Name = name
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6> UsingQueryStringParameter<TU6>(string name, TU6 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6>(Definition, new FluentActionUsingQueryStringParameterDefinition
            {
                Type = typeof(TU6),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6> UsingHeader<TU6>(string name)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6>(Definition, new FluentActionUsingHeaderParameterDefinition
            {
                Type = typeof(TU6),
                Name = name
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6> UsingHeader<TU6>(string name, TU6 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6>(Definition, new FluentActionUsingHeaderParameterDefinition
            {
                Type = typeof(TU6),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6> UsingBody<TU6>()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6>(Definition, new FluentActionUsingBodyDefinition
            {
                Type = typeof(TU6)
            });
        }

        public FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6> UsingBody<TU6>(TU6 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6>(Definition, new FluentActionUsingBodyDefinition
            {
                Type = typeof(TU6),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6> UsingForm<TU6>()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6>(Definition, new FluentActionUsingFormDefinition
            {
                Type = typeof(TU6)
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6> UsingForm<TU6>(TU6 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6>(Definition, new FluentActionUsingFormDefinition
            {
                Type = typeof(TU6),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, IFormFile> UsingFormFile(string name)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, IFormFile>(Definition, new FluentActionUsingFormFileDefinition
            {
                Type = typeof(IFormFile),
                Name = name
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, IEnumerable<IFormFile>> UsingFormFiles(string name)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, IEnumerable<IFormFile>>(Definition, new FluentActionUsingFormFilesDefinition
            {
                Type = typeof(IEnumerable<IFormFile>),
                Name = name
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6> UsingFormValue<TU6>(string key)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6>(Definition, new FluentActionUsingFormValueDefinition
            {
                Type = typeof(TU6),
                Key = key
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6> UsingFormValue<TU6>(string key, TU6 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6>(Definition, new FluentActionUsingFormValueDefinition
            {
                Type = typeof(TU6),
                Key = key,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6> UsingModelBinder<TU6>(Type modelBinderType, string parameterName = null)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6>(Definition, new FluentActionUsingModelBinderDefinition
            {
                Type = typeof(TU6),
                ModelBinderType = modelBinderType,
                ParameterName = parameterName
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6> UsingModelBinder<TU6>(Type modelBinderType, string parameterName, TU6 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6>(Definition, new FluentActionUsingModelBinderDefinition
            {
                Type = typeof(TU6),
                ModelBinderType = modelBinderType,
                HasDefaultValue = true,
                DefaultValue = defaultValue,
                ParameterName = parameterName
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, ModelStateDictionary> UsingModelState()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, ModelStateDictionary>(Definition, new FluentActionUsingModelStateDefinition
            {
                Type = typeof(ModelStateDictionary)
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, HttpContext> UsingHttpContext()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, HttpContext>(Definition, new FluentActionUsingHttpContextDefinition
            {
                Type = typeof(HttpContext)
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, dynamic> UsingViewBag()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, dynamic>(Definition, new FluentActionUsingViewBagDefinition
            {
                Type = typeof(object)
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, ViewDataDictionary> UsingViewData()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, ViewDataDictionary>(Definition, new FluentActionUsingViewDataDefinition
            {
                Type = typeof(ViewDataDictionary)
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, ITempDataDictionary> UsingTempData()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, ITempDataDictionary>(Definition, new FluentActionUsingTempDataDefinition
            {
                Type = typeof(ITempDataDictionary)
            });
        }

        public FluentAction<TP, object> Do(Action<TU1, TU2, TU3, TU4, TU5> handlerAction)
        {
            Definition.ExistingOrNewHandlerDraft.Type = FluentActionHandlerType.Action;
            Definition.ExistingOrNewHandlerDraft.Delegate = handlerAction;
            Definition.CommitHandlerDraft();
            return new FluentAction<TP, object>(Definition);
        }

        public FluentAction<TP, object> DoAsync(Func<TU1, TU2, TU3, TU4, TU5, Task> asyncHandlerAction)
        {
            Definition.ExistingOrNewHandlerDraft.Type = FluentActionHandlerType.Action;
            Definition.ExistingOrNewHandlerDraft.Delegate = asyncHandlerAction;
            Definition.ExistingOrNewHandlerDraft.Async = true;
            Definition.CommitHandlerDraft();
            return new FluentAction<TP, object>(Definition);
        }

        public virtual FluentActionWithMvcController<TU1, TU2, TU3, TU4, TU5, TC> ToMvcController<TC>() where TC : Controller
        {
            return new FluentActionWithMvcController<TU1, TU2, TU3, TU4, TU5, TC>(Definition, new FluentActionUsingMvcControllerDefinition
            {
                Type = typeof(TC)
            });
        }

        public FluentAction<TP, TR2> To<TR2>(Func<TU1, TU2, TU3, TU4, TU5, TR2> handlerFunc)
        {
            return new FluentAction<TP, TR2>(Definition, handlerFunc);
        }

        public FluentAction<TP, TR2> To<TR2>(Func<TU1, TU2, TU3, TU4, TU5, Task<TR2>> asyncHandlerFunc)
        {
            return new FluentAction<TP, TR2>(Definition, asyncHandlerFunc, async: true);
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
    }

    public class FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6> : FluentActionBase
    {
        public FluentAction(FluentActionDefinition fluentActionDefinition, FluentActionUsingDefinition usingDefinition) : base(fluentActionDefinition)
        {
            Definition.ExistingOrNewHandlerDraft.Usings.Add(usingDefinition);
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7> Using<TU7>(FluentActionUsingDefinition usingDefinition)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7>(Definition, usingDefinition);
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TP> UsingParent()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TP>(Definition, new FluentActionUsingParentDefinition
            {
                Type = typeof(TR)
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TP2> UsingParent<TP2>()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TP2>(Definition, new FluentActionUsingParentDefinition
            {
                Type = typeof(TP2)
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TR> UsingResult()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TR>(Definition, new FluentActionUsingResultDefinition
            {
                Type = typeof(TR)
            });
        }

        public FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7> UsingService<TU7>()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7>(Definition, new FluentActionUsingServiceDefinition
            {
                Type = typeof(TU7)
            });
        }

        public FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7> UsingService<TU7>(TU7 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7>(Definition, new FluentActionUsingServiceDefinition
            {
                Type = typeof(TU7),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7> UsingRouteParameter<TU7>(string name)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7>(Definition, new FluentActionUsingRouteParameterDefinition
            {
                Type = typeof(TU7),
                Name = name
            });
        }

        public FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7> UsingRouteParameter<TU7>(string name, TU7 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7>(Definition, new FluentActionUsingRouteParameterDefinition
            {
                Type = typeof(TU7),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7> UsingQueryStringParameter<TU7>(string name)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7>(Definition, new FluentActionUsingQueryStringParameterDefinition
            {
                Type = typeof(TU7),
                Name = name
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7> UsingQueryStringParameter<TU7>(string name, TU7 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7>(Definition, new FluentActionUsingQueryStringParameterDefinition
            {
                Type = typeof(TU7),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7> UsingHeader<TU7>(string name)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7>(Definition, new FluentActionUsingHeaderParameterDefinition
            {
                Type = typeof(TU7),
                Name = name
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7> UsingHeader<TU7>(string name, TU7 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7>(Definition, new FluentActionUsingHeaderParameterDefinition
            {
                Type = typeof(TU7),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7> UsingBody<TU7>()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7>(Definition, new FluentActionUsingBodyDefinition
            {
                Type = typeof(TU7)
            });
        }

        public FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7> UsingBody<TU7>(TU7 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7>(Definition, new FluentActionUsingBodyDefinition
            {
                Type = typeof(TU7),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7> UsingForm<TU7>()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7>(Definition, new FluentActionUsingFormDefinition
            {
                Type = typeof(TU7)
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7> UsingForm<TU7>(TU7 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7>(Definition, new FluentActionUsingFormDefinition
            {
                Type = typeof(TU7),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, IFormFile> UsingFormFile(string name)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, IFormFile>(Definition, new FluentActionUsingFormFileDefinition
            {
                Type = typeof(IFormFile),
                Name = name
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, IEnumerable<IFormFile>> UsingFormFiles(string name)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, IEnumerable<IFormFile>>(Definition, new FluentActionUsingFormFilesDefinition
            {
                Type = typeof(IEnumerable<IFormFile>),
                Name = name
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7> UsingFormValue<TU7>(string key)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7>(Definition, new FluentActionUsingFormValueDefinition
            {
                Type = typeof(TU7),
                Key = key
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7> UsingFormValue<TU7>(string key, TU7 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7>(Definition, new FluentActionUsingFormValueDefinition
            {
                Type = typeof(TU7),
                Key = key,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7> UsingModelBinder<TU7>(Type modelBinderType, string parameterName = null)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7>(Definition, new FluentActionUsingModelBinderDefinition
            {
                Type = typeof(TU7),
                ModelBinderType = modelBinderType,
                ParameterName = parameterName
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7> UsingModelBinder<TU7>(Type modelBinderType, string parameterName, TU7 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7>(Definition, new FluentActionUsingModelBinderDefinition
            {
                Type = typeof(TU7),
                ModelBinderType = modelBinderType,
                HasDefaultValue = true,
                DefaultValue = defaultValue,
                ParameterName = parameterName
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, ModelStateDictionary> UsingModelState()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, ModelStateDictionary>(Definition, new FluentActionUsingModelStateDefinition
            {
                Type = typeof(ModelStateDictionary)
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, HttpContext> UsingHttpContext()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, HttpContext>(Definition, new FluentActionUsingHttpContextDefinition
            {
                Type = typeof(HttpContext)
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, dynamic> UsingViewBag()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, dynamic>(Definition, new FluentActionUsingViewBagDefinition
            {
                Type = typeof(object)
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, ViewDataDictionary> UsingViewData()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, ViewDataDictionary>(Definition, new FluentActionUsingViewDataDefinition
            {
                Type = typeof(ViewDataDictionary)
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, ITempDataDictionary> UsingTempData()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, ITempDataDictionary>(Definition, new FluentActionUsingTempDataDefinition
            {
                Type = typeof(ITempDataDictionary)
            });
        }

        public FluentAction<TP, object> Do(Action<TU1, TU2, TU3, TU4, TU5, TU6> handlerAction)
        {
            Definition.ExistingOrNewHandlerDraft.Type = FluentActionHandlerType.Action;
            Definition.ExistingOrNewHandlerDraft.Delegate = handlerAction;
            Definition.CommitHandlerDraft();
            return new FluentAction<TP, object>(Definition);
        }

        public FluentAction<TP, object> DoAsync(Func<TU1, TU2, TU3, TU4, TU5, TU6, Task> asyncHandlerAction)
        {
            Definition.ExistingOrNewHandlerDraft.Type = FluentActionHandlerType.Action;
            Definition.ExistingOrNewHandlerDraft.Delegate = asyncHandlerAction;
            Definition.ExistingOrNewHandlerDraft.Async = true;
            Definition.CommitHandlerDraft();
            return new FluentAction<TP, object>(Definition);
        }

        public virtual FluentActionWithMvcController<TU1, TU2, TU3, TU4, TU5, TU6, TC> ToMvcController<TC>() where TC : Controller
        {
            return new FluentActionWithMvcController<TU1, TU2, TU3, TU4, TU5, TU6, TC>(Definition, new FluentActionUsingMvcControllerDefinition
            {
                Type = typeof(TC)
            });
        }

        public FluentAction<TP, TR2> To<TR2>(Func<TU1, TU2, TU3, TU4, TU5, TU6, TR2> handlerFunc)
        {
            return new FluentAction<TP, TR2>(Definition, handlerFunc);
        }

        public FluentAction<TP, TR2> To<TR2>(Func<TU1, TU2, TU3, TU4, TU5, TU6, Task<TR2>> asyncHandlerFunc)
        {
            return new FluentAction<TP, TR2>(Definition, asyncHandlerFunc, async: true);
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
    }

    public class FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7> : FluentActionBase
    {
        public FluentAction(FluentActionDefinition fluentActionDefinition, FluentActionUsingDefinition usingDefinition) : base(fluentActionDefinition)
        {
            Definition.ExistingOrNewHandlerDraft.Usings.Add(usingDefinition);
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> Using<TU8>(FluentActionUsingDefinition usingDefinition)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(Definition, usingDefinition);
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TP> UsingParent()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TP>(Definition, new FluentActionUsingParentDefinition
            {
                Type = typeof(TR)
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TP2> UsingParent<TP2>()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TP2>(Definition, new FluentActionUsingParentDefinition
            {
                Type = typeof(TP2)
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TR> UsingResult()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TR>(Definition, new FluentActionUsingResultDefinition
            {
                Type = typeof(TR)
            });
        }

        public FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> UsingService<TU8>()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(Definition, new FluentActionUsingServiceDefinition
            {
                Type = typeof(TU8)
            });
        }

        public FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> UsingService<TU8>(TU8 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(Definition, new FluentActionUsingServiceDefinition
            {
                Type = typeof(TU8),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> UsingRouteParameter<TU8>(string name)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(Definition, new FluentActionUsingRouteParameterDefinition
            {
                Type = typeof(TU8),
                Name = name
            });
        }

        public FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> UsingRouteParameter<TU8>(string name, TU8 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(Definition, new FluentActionUsingRouteParameterDefinition
            {
                Type = typeof(TU8),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> UsingQueryStringParameter<TU8>(string name)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(Definition, new FluentActionUsingQueryStringParameterDefinition
            {
                Type = typeof(TU8),
                Name = name
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> UsingQueryStringParameter<TU8>(string name, TU8 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(Definition, new FluentActionUsingQueryStringParameterDefinition
            {
                Type = typeof(TU8),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> UsingHeader<TU8>(string name)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(Definition, new FluentActionUsingHeaderParameterDefinition
            {
                Type = typeof(TU8),
                Name = name
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> UsingHeader<TU8>(string name, TU8 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(Definition, new FluentActionUsingHeaderParameterDefinition
            {
                Type = typeof(TU8),
                Name = name,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> UsingBody<TU8>()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(Definition, new FluentActionUsingBodyDefinition
            {
                Type = typeof(TU8)
            });
        }

        public FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> UsingBody<TU8>(TU8 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(Definition, new FluentActionUsingBodyDefinition
            {
                Type = typeof(TU8),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> UsingForm<TU8>()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(Definition, new FluentActionUsingFormDefinition
            {
                Type = typeof(TU8)
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> UsingForm<TU8>(TU8 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(Definition, new FluentActionUsingFormDefinition
            {
                Type = typeof(TU8),
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, IFormFile> UsingFormFile(string name)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, IFormFile>(Definition, new FluentActionUsingFormFileDefinition
            {
                Type = typeof(IFormFile),
                Name = name
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, IEnumerable<IFormFile>> UsingFormFiles(string name)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, IEnumerable<IFormFile>>(Definition, new FluentActionUsingFormFilesDefinition
            {
                Type = typeof(IEnumerable<IFormFile>),
                Name = name
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> UsingFormValue<TU8>(string key)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(Definition, new FluentActionUsingFormValueDefinition
            {
                Type = typeof(TU8),
                Key = key
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> UsingFormValue<TU8>(string key, TU8 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(Definition, new FluentActionUsingFormValueDefinition
            {
                Type = typeof(TU8),
                Key = key,
                HasDefaultValue = true,
                DefaultValue = defaultValue
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> UsingModelBinder<TU8>(Type modelBinderType, string parameterName = null)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(Definition, new FluentActionUsingModelBinderDefinition
            {
                Type = typeof(TU8),
                ModelBinderType = modelBinderType,
                ParameterName = parameterName
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> UsingModelBinder<TU8>(Type modelBinderType, string parameterName, TU8 defaultValue)
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8>(Definition, new FluentActionUsingModelBinderDefinition
            {
                Type = typeof(TU8),
                ModelBinderType = modelBinderType,
                HasDefaultValue = true,
                DefaultValue = defaultValue,
                ParameterName = parameterName
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, ModelStateDictionary> UsingModelState()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, ModelStateDictionary>(Definition, new FluentActionUsingModelStateDefinition
            {
                Type = typeof(ModelStateDictionary)
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, HttpContext> UsingHttpContext()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, HttpContext>(Definition, new FluentActionUsingHttpContextDefinition
            {
                Type = typeof(HttpContext)
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, dynamic> UsingViewBag()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, dynamic>(Definition, new FluentActionUsingViewBagDefinition
            {
                Type = typeof(object)
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, ViewDataDictionary> UsingViewData()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, ViewDataDictionary>(Definition, new FluentActionUsingViewDataDefinition
            {
                Type = typeof(ViewDataDictionary)
            });
        }

        public virtual FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, ITempDataDictionary> UsingTempData()
        {
            return new FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, ITempDataDictionary>(Definition, new FluentActionUsingTempDataDefinition
            {
                Type = typeof(ITempDataDictionary)
            });
        }

        public FluentAction<TP, object> Do(Action<TU1, TU2, TU3, TU4, TU5, TU6, TU7> handlerAction)
        {
            Definition.ExistingOrNewHandlerDraft.Type = FluentActionHandlerType.Action;
            Definition.ExistingOrNewHandlerDraft.Delegate = handlerAction;
            Definition.CommitHandlerDraft();
            return new FluentAction<TP, object>(Definition);
        }

        public FluentAction<TP, object> DoAsync(Func<TU1, TU2, TU3, TU4, TU5, TU6, TU7, Task> asyncHandlerAction)
        {
            Definition.ExistingOrNewHandlerDraft.Type = FluentActionHandlerType.Action;
            Definition.ExistingOrNewHandlerDraft.Delegate = asyncHandlerAction;
            Definition.ExistingOrNewHandlerDraft.Async = true;
            Definition.CommitHandlerDraft();
            return new FluentAction<TP, object>(Definition);
        }

        public virtual FluentActionWithMvcController<TU1, TU2, TU3, TU4, TU5, TU6, TU7, TC> ToMvcController<TC>() where TC : Controller
        {
            return new FluentActionWithMvcController<TU1, TU2, TU3, TU4, TU5, TU6, TU7, TC>(Definition, new FluentActionUsingMvcControllerDefinition
            {
                Type = typeof(TC)
            });
        }

        public FluentAction<TP, TR2> To<TR2>(Func<TU1, TU2, TU3, TU4, TU5, TU6, TU7, TR2> handlerFunc)
        {
            return new FluentAction<TP, TR2>(Definition, handlerFunc);
        }

        public FluentAction<TP, TR2> To<TR2>(Func<TU1, TU2, TU3, TU4, TU5, TU6, TU7, Task<TR2>> asyncHandlerFunc)
        {
            return new FluentAction<TP, TR2>(Definition, asyncHandlerFunc, async: true);
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
    }

    public class FluentAction<TP, TR, TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> : FluentActionBase
    {
        public FluentAction(FluentActionDefinition fluentActionDefinition, FluentActionUsingDefinition usingDefinition) : base(fluentActionDefinition)
        {
            Definition.ExistingOrNewHandlerDraft.Usings.Add(usingDefinition);
        }

        public FluentAction<TP, object> Do(Action<TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8> handlerAction)
        {
            Definition.ExistingOrNewHandlerDraft.Type = FluentActionHandlerType.Action;
            Definition.ExistingOrNewHandlerDraft.Delegate = handlerAction;
            Definition.CommitHandlerDraft();
            return new FluentAction<TP, object>(Definition);
        }

        public FluentAction<TP, object> DoAsync(Func<TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8, Task> asyncHandlerAction)
        {
            Definition.ExistingOrNewHandlerDraft.Type = FluentActionHandlerType.Action;
            Definition.ExistingOrNewHandlerDraft.Delegate = asyncHandlerAction;
            Definition.ExistingOrNewHandlerDraft.Async = true;
            Definition.CommitHandlerDraft();
            return new FluentAction<TP, object>(Definition);
        }

        public virtual FluentActionWithMvcController<TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8, TC> ToMvcController<TC>() where TC : Controller
        {
            return new FluentActionWithMvcController<TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8, TC>(Definition, new FluentActionUsingMvcControllerDefinition
            {
                Type = typeof(TC)
            });
        }

        public FluentAction<TP, TR2> To<TR2>(Func<TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8, TR2> handlerFunc)
        {
            return new FluentAction<TP, TR2>(Definition, handlerFunc);
        }

        public FluentAction<TP, TR2> To<TR2>(Func<TU1, TU2, TU3, TU4, TU5, TU6, TU7, TU8, Task<TR2>> asyncHandlerFunc)
        {
            return new FluentAction<TP, TR2>(Definition, asyncHandlerFunc, async: true);
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
    }
}
