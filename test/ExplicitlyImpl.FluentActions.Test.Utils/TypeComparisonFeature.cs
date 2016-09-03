// Licensed under the MIT License. See LICENSE file in the root of the solution for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;

namespace ExplicitlyImpl.FluentActions.Test.Utils
{
    public enum TypeComparisonFeature
    {
        Name,
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
            } else
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
                }
            }

            if (actionMethodForType1.ReturnType != actionMethodForType2.ReturnType)
            {
                comparisonResults.Add(new TypeFeatureComparisonResult(
                    TypeComparisonFeature.HandlerActionMethod,
                    false,
                    $"Return type does not match between action methods ({actionMethodForType1.ReturnType.Name} vs {actionMethodForType2.ReturnType.Name})."));
            }

            var attributesForActionMethod1 = actionMethodForType1.GetCustomAttributes()
                .ToArray();
            var attributesForActionMethod2 = actionMethodForType2.GetCustomAttributes()
                .Where(attribute => !(attribute is AsyncStateMachineAttribute) && !(attribute is DebuggerStepThroughAttribute))
                .ToArray();

            if (attributesForActionMethod1.Count() != attributesForActionMethod2.Count())
            {
                comparisonResults.Add(new TypeFeatureComparisonResult(
                    TypeComparisonFeature.HandlerActionMethod,
                    false,
                    $"Custom attribute count does not match between action methods ({attributesForActionMethod1.Count()} vs {attributesForActionMethod2.Count()})."));
            } else
            {
                for(var attributeIndex = 0; attributeIndex < attributesForActionMethod1.Length; attributeIndex++)
                {
                    if (attributesForActionMethod1[attributeIndex].GetType() != attributesForActionMethod2[attributeIndex].GetType())
                    {
                        comparisonResults.Add(new TypeFeatureComparisonResult(
                            TypeComparisonFeature.HandlerActionMethod,
                            false,
                            $"Attribute types do not match between action methods (attribute index: {attributeIndex})."));
                    } 
                    else if (attributesForActionMethod1[attributeIndex] is RouteAttribute)
                    {
                        var routeTemplate1 = ((RouteAttribute)attributesForActionMethod1[attributeIndex]).Template;
                        var routeTemplate2 = ((RouteAttribute)attributesForActionMethod2[attributeIndex]).Template;

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
            { TypeComparisonFeature.HandlerActionMethod , new HandlerActionMethodComparer() }
        };
    }
}
