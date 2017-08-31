// Licensed under the MIT License. See LICENSE file in the root of the solution for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace ExplicitlyImpl.FluentActions.Test.Utils
{
    public enum TypeComparisonFeature
    {
        Name,
        ParentType,
        HandlerActionMethod
    }

    public interface TypeFeatureComparer
    {
        IEnumerable<TypeFeatureComparisonResult> Compare(Type Type1, Type Type2, TypeComparerOptions options);
    }

    public class TypeNameComparer : TypeFeatureComparer
    {
        public IEnumerable<TypeFeatureComparisonResult> Compare(Type type1, Type type2, TypeComparerOptions options)
        {
            var namesMatch = type1.Name == type2.Name;

            return new[]
            {
                new TypeFeatureComparisonResult(
                    TypeComparisonFeature.Name,
                    namesMatch,
                    namesMatch ? "Names match." : "Names do not match.")
            };
        }
    }

    public class ParentTypeComparer : TypeFeatureComparer
    {
        public IEnumerable<TypeFeatureComparisonResult> Compare(Type type1, Type type2, TypeComparerOptions options)
        {
            var comparisonResults = new List<TypeFeatureComparisonResult>();

            var parentTypesMatch = type1.GetTypeInfo().BaseType == type2.GetTypeInfo().BaseType;

            comparisonResults.Add(new TypeFeatureComparisonResult(
                TypeComparisonFeature.ParentType,
                parentTypesMatch,
                parentTypesMatch ? "Parent types match." : "Parent types do not match."
            ));

            var attributesForType1 = type1.GetTypeInfo().GetCustomAttributes()
                .ToArray();
            var attributesForType2 = type2.GetTypeInfo().GetCustomAttributes()
                .ToArray();

            for (var attributeIndex = 0; attributeIndex < attributesForType1.Length; attributeIndex++)
            {
                if (attributesForType1[attributeIndex].GetType() != attributesForType2[attributeIndex].GetType())
                {
                    comparisonResults.Add(new TypeFeatureComparisonResult(
                        TypeComparisonFeature.HandlerActionMethod,
                        false,
                        $"Attribute types do not match between action methods (attribute index: {attributeIndex})."));
                }
            }

            return comparisonResults;
        }
    }

    public class HandlerActionMethodComparer : TypeFeatureComparer
    {
        public IEnumerable<TypeFeatureComparisonResult> Compare(Type type1, Type type2, TypeComparerOptions options)
        {
            var comparisonResults = new List<TypeFeatureComparisonResult>();

            var typeInfo1 = type1.GetTypeInfo();
            var typeInfo2 = type2.GetTypeInfo();

            var actionMethodForType1 = type1.GetTypeInfo().GetDeclaredMethods("HandlerAction").First();
            if (actionMethodForType1 == null)
            {
                comparisonResults.Add(new TypeFeatureComparisonResult(
                    TypeComparisonFeature.HandlerActionMethod, 
                    false, 
                    $"Type {type1.Name} is missing a handler action method."));
            }

            var actionMethodForType2 = type2.GetTypeInfo().GetDeclaredMethods("HandlerAction").First();
            if (actionMethodForType2 == null)
            {
                comparisonResults.Add(new TypeFeatureComparisonResult(
                    TypeComparisonFeature.HandlerActionMethod,
                    false,
                    $"Type {type2.Name} is missing a handler action method."));
            }

            if (actionMethodForType1 == null || actionMethodForType2 == null)
            {
                return comparisonResults;
            }

            var parametersForType1 = actionMethodForType1.GetParameters();
            var parametersForType2 = actionMethodForType2.GetParameters();

            if (parametersForType1.Length != parametersForType2.Length)
            {
                comparisonResults.Add(new TypeFeatureComparisonResult(
                    TypeComparisonFeature.HandlerActionMethod,
                    false,
                    $"Parameter count does not match between action methods ({parametersForType1.Length} vs {parametersForType2.Length})."));
            } 
            else
            {
                for(var parameterIndex = 0; parameterIndex < parametersForType1.Length; parameterIndex++)
                {
                    if (parametersForType1[parameterIndex].ParameterType != parametersForType2[parameterIndex].ParameterType)
                    {
                        comparisonResults.Add(new TypeFeatureComparisonResult(
                            TypeComparisonFeature.HandlerActionMethod,
                            false,
                            $"Parameter types do not match between action methods (parameter index: {parameterIndex})."));
                    } 
                    else
                    {
                        var parameterAttributes1 = parametersForType1[parameterIndex].CustomAttributes
                            .Where(attribute => attribute.AttributeType != typeof(OptionalAttribute))
                            .ToArray();
                        var parameterAttributes2 = parametersForType2[parameterIndex].CustomAttributes
                            .Where(attribute => attribute.AttributeType != typeof(OptionalAttribute))
                            .ToArray();

                        if (parameterAttributes1.Length != parameterAttributes2.Length)
                        {
                            comparisonResults.Add(new TypeFeatureComparisonResult(
                                TypeComparisonFeature.HandlerActionMethod,
                                false,
                                $"Parameter attribute count does not match between action methods for parameter at index {parameterIndex} ({parameterAttributes1.Length} vs {parameterAttributes2.Length})."));
                        } 
                        else
                        {
                            for (var parameterAttributeIndex = 0; parameterAttributeIndex < parameterAttributes1.Length; parameterAttributeIndex++)
                            {
                                if (parameterAttributes1[parameterAttributeIndex].AttributeType != parameterAttributes2[parameterAttributeIndex].AttributeType)
                                {
                                    comparisonResults.Add(new TypeFeatureComparisonResult(
                                        TypeComparisonFeature.HandlerActionMethod,
                                        false,
                                        $"Parameter attribute types do not match between action methods for parameter at index {parameterIndex} ({parameterAttributes1[parameterAttributeIndex].AttributeType} vs {parameterAttributes2[parameterAttributeIndex].AttributeType})."));
                                } 
                                else if (parameterAttributes1[parameterAttributeIndex].AttributeType == typeof(ModelBinderAttribute))
                                {
                                    var parameterAttribute1 = parameterAttributes1[parameterAttributeIndex];
                                    var parameterAttribute2 = parameterAttributes2[parameterAttributeIndex];

                                    if (parameterAttribute1.NamedArguments.Count != parameterAttribute2.NamedArguments.Count)
                                    {
                                        comparisonResults.Add(new TypeFeatureComparisonResult(
                                            TypeComparisonFeature.HandlerActionMethod,
                                            false,
                                            $"Named argument count for ModelBinderAttribute does not match between action methods for parameter at index {parameterIndex} ({parameterAttribute1.NamedArguments.Count} vs {parameterAttribute2.NamedArguments.Count})."));
                                    } 
                                    else
                                    {
                                        // TODO
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (actionMethodForType1.ReturnType != actionMethodForType2.ReturnType)
            {
                comparisonResults.Add(new TypeFeatureComparisonResult(
                    TypeComparisonFeature.HandlerActionMethod,
                    false,
                    $"Return type does not match between action methods ({actionMethodForType1.ReturnType.Name} vs {actionMethodForType2.ReturnType.Name})."));
            }

            var attributesForType1 = actionMethodForType1.GetCustomAttributes()
                .ToArray();
            var attributesForType2 = actionMethodForType2.GetCustomAttributes()
                .Where(attribute => !(attribute is AsyncStateMachineAttribute) && !(attribute is DebuggerStepThroughAttribute))
                .ToArray();

            if (attributesForType1.Count() != attributesForType2.Count())
            {
                comparisonResults.Add(new TypeFeatureComparisonResult(
                    TypeComparisonFeature.HandlerActionMethod,
                    false,
                    $"Custom attribute count does not match between action methods ({attributesForType1.Count()} vs {attributesForType2.Count()})."));
            } 
            else
            {
                for(var attributeIndex = 0; attributeIndex < attributesForType1.Length; attributeIndex++)
                {
                    if (attributesForType1[attributeIndex].GetType() != attributesForType2[attributeIndex].GetType())
                    {
                        comparisonResults.Add(new TypeFeatureComparisonResult(
                            TypeComparisonFeature.HandlerActionMethod,
                            false,
                            $"Attribute types do not match between action methods (attribute index: {attributeIndex})."));
                    } 
                    else if (attributesForType1[attributeIndex] is RouteAttribute)
                    {
                        var routeTemplate1 = ((RouteAttribute)attributesForType1[attributeIndex]).Template;
                        var routeTemplate2 = ((RouteAttribute)attributesForType2[attributeIndex]).Template;

                        if (routeTemplate1 != routeTemplate2)
                        {
                            comparisonResults.Add(new TypeFeatureComparisonResult(
                                TypeComparisonFeature.HandlerActionMethod,
                                false,
                                $"Route templates do not match between action methods ({routeTemplate1} vs {routeTemplate2})."));
                        }
                    }
                }
            }

            if (!comparisonResults.Any())
            {
                comparisonResults.Add(new TypeFeatureComparisonResult(
                    TypeComparisonFeature.HandlerActionMethod,
                    true,
                    $"Types have matching handler action methods."));
            }

            return comparisonResults;
        }
    }

    public static class TypeFeatureComparers
    {
        public static Dictionary<TypeComparisonFeature, TypeFeatureComparer> All = new Dictionary<TypeComparisonFeature, TypeFeatureComparer> 
        {
            { TypeComparisonFeature.Name , new TypeNameComparer() },
            { TypeComparisonFeature.ParentType , new ParentTypeComparer() },
            { TypeComparisonFeature.HandlerActionMethod , new HandlerActionMethodComparer() },
        };
    }
}
