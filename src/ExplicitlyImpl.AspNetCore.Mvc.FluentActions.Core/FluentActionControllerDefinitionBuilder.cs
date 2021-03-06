﻿// Licensed under the MIT License. See LICENSE file in the root of the solution for license information.

using ExplicitlyImpl.AspNetCore.Mvc.FluentActions.Core.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentActions
{
    public class FluentActionControllerDefinitionBuilder
    {
        private const string ActionName = "HandlerAction";

        public FluentActionControllerDefinition Build(FluentActionBase fluentAction, ILogger logger = null)
        {
            if (fluentAction == null)
            {
                throw new ArgumentNullException(nameof(fluentAction));
            }

            var validationResult = ValidateFluentActionForBuilding(fluentAction);

            if (!validationResult.Valid)
            {
                throw new FluentActionValidationException($"Could not validate fluent action {fluentAction}: {validationResult}");
            }

            if (fluentAction.Definition.IsMapRoute)
            {
                var handler = fluentAction.Definition.Handlers.First();

                if (handler.Expression == null)
                {
                    throw new ArgumentException(
                        $"Missing action expression for {fluentAction}.");
                }

                if (!(handler.Expression.Body is MethodCallExpression))
                {
                    throw new ArgumentException(
                        $"Expression for {fluentAction} must be a single method call expression.");
                }

                var method = ((MethodCallExpression)handler.Expression.Body).Method;
                var controllerTypeInfo = method.DeclaringType.GetTypeInfo();

                if (!typeof(Controller).IsAssignableFrom(controllerTypeInfo.UnderlyingSystemType))
                {
                    throw new ArgumentException(
                        $"Method call for {fluentAction} must come from a controller.");
                }

                var guid = Guid.NewGuid().ToString().Without("-");

                return new FluentActionControllerDefinition()
                {
                    Id = controllerTypeInfo.Name + "_" + method.Name + "_" + guid,
                    Name = controllerTypeInfo.Name,
                    ActionName = method.Name,
                    FluentAction = fluentAction,
                    TypeInfo = controllerTypeInfo
                };
            } else
            {
                if (
                    (
                        fluentAction.Definition.GroupName != null || 
                        fluentAction.Definition.IgnoreApi == true
                    ) &&
                    fluentAction.Definition.CustomAttributes.All(ca => ca.Type != typeof(ApiExplorerSettingsAttribute))
                )
                {
                    var nonNullableProperties = new Dictionary<string, object>()
                        {
                            { "GroupName", fluentAction.Definition.GroupName },
                            { "IgnoreApi", fluentAction.Definition.IgnoreApi },
                        }
                        .Where(pair => pair.Value != null)
                        .ToList();

                    var apiExplorerSettingsAttributeType = typeof(ApiExplorerSettingsAttribute);
                    fluentAction.Definition.CustomAttributes.Add(new FluentActionCustomAttribute
                    {
                        Type = apiExplorerSettingsAttributeType,
                        Constructor = apiExplorerSettingsAttributeType.GetConstructor(new Type[0]),
                        ConstructorArgs = new object[0],
                        NamedProperties = nonNullableProperties.Select(p => apiExplorerSettingsAttributeType.GetProperty(p.Key)).ToArray(),
                        PropertyValues = nonNullableProperties.Select(p => p.Value).ToArray(),
                        NamedFields = new FieldInfo[0],
                        FieldValues = new object[0],
                    });
                }

                try
                {
                    var controllerTypeInfo = DefineControllerType(fluentAction.Definition, logger);

                    return new FluentActionControllerDefinition()
                    {
                        Id = controllerTypeInfo.Name,
                        Name = controllerTypeInfo.Name,
                        ActionName = ActionName,
                        FluentAction = fluentAction,
                        TypeInfo = controllerTypeInfo
                    };
                } catch (Exception buildException)
                {
                    throw new Exception($"Could not build controller type for {fluentAction}: {buildException.Message}", buildException);
                }
            }
        }

        private FluentActionValidationResult ValidateFluentActionForBuilding(FluentActionBase fluentAction)
        {
            if (fluentAction == null)
            {
                throw new ArgumentNullException(nameof(fluentAction));
            }

            var validationResult = new FluentActionValidationResult
            {
                Valid = true,
                ValidationErrorMessages = new List<string>()
            };

            if (fluentAction.Definition == null)
            {
                validationResult.AddValidationError($"{nameof(fluentAction.Definition)} is null.");
                return validationResult;
            }

            if (fluentAction.Definition.Handlers == null)
            {
                validationResult.AddValidationError($"{nameof(fluentAction.Definition.Handlers)} is null.");
                return validationResult;
            }

            var handlers = fluentAction.Definition.Handlers;

            if (!handlers.Any())
            {
                validationResult.AddValidationError("At least one handler is required.");
            }

            foreach (var handlerWithNoReturnType in handlers.Where(handler => 
                handler.Type != FluentActionHandlerType.Action && 
                handler.ReturnType == null))
            {
                validationResult.AddValidationError("Missing return type for handler.");
            }

            if (handlers.Any() && handlers.Last().Type == FluentActionHandlerType.Action)
            {
                validationResult.AddValidationError("Cannot end a fluent action with a Do statement.");
            }

            return validationResult;
        }

        public class FluentActionValidationResult
        {
            public FluentActionValidationResult()
            {
                ValidationErrorMessages = new List<string>();
            }

            public bool Valid { get; set; }

            public IList<string> ValidationErrorMessages { get; set; }

            public void AddValidationError(string errorMessage)
            {
                Valid = false;
                ValidationErrorMessages.Add(errorMessage);
            }

            public override string ToString()
            {
                return Valid ? "This fluent action is valid" : string.Join(Environment.NewLine, ValidationErrorMessages);
            }
        }

        private static TypeInfo DefineControllerType(
            FluentActionDefinition fluentActionDefinition,
            ILogger logger = null)
        {
            if (fluentActionDefinition == null)
            {
                throw new ArgumentNullException(nameof(fluentActionDefinition));
            }

            var guid = Guid.NewGuid().ToString().Replace("-", "");
            var typeName = $"FluentAction{guid}Controller";

            var controllerTypeBuilder = ControllerTypeBuilder.Create(
                "FluentActionAssembly", 
                "FluentActionModule", 
                typeName, 
                fluentActionDefinition.ParentType);

            foreach (var customAttributeOnClass in fluentActionDefinition.CustomAttributesOnClass)
            {
                controllerTypeBuilder.SetCustomAttribute(
                    customAttributeOnClass.Constructor,
                    customAttributeOnClass.ConstructorArgs,
                    customAttributeOnClass.NamedProperties,
                    customAttributeOnClass.PropertyValues,
                    customAttributeOnClass.NamedFields,
                    customAttributeOnClass.FieldValues
                );
            }

            ControllerMethodBuilder controllerMethodBuilder;
            if (AsyncStateMachineBuilderIsNeeded(fluentActionDefinition))
            {
                controllerMethodBuilder = new ControllerMethodBuilderForFluentActionAsync(fluentActionDefinition, logger);
            } 
            else
            {
                controllerMethodBuilder = new ControllerMethodBuilderForFluentAction(fluentActionDefinition);
            }

            controllerTypeBuilder.BuildMethod(ActionName, controllerMethodBuilder);

            return controllerTypeBuilder.CreateTypeInfo();
        }

        public static bool AsyncStateMachineBuilderIsNeeded(FluentActionDefinition fluentActionDefinition)
        {
            // An AsyncStateMachineBuilder is not needed when there is only one handler invoking an async func
            return fluentActionDefinition.IsAsync &&
                (fluentActionDefinition.Handlers.Count() > 1 ||
                (fluentActionDefinition.Handlers.Count() == 1 && fluentActionDefinition.Handlers.First().Type != FluentActionHandlerType.Func));
        } 
    }
}

