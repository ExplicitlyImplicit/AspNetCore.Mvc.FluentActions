using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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

            //var attributesForActionMethod1 = actionMethodForType1.GetCustomAttributes();
            //var attributesForActionMethod2 = actionMethodForType2.GetCustomAttributes();

            //if (attributesForActionMethod1.Count() != attributesForActionMethod2.Count())
            //{
            //    comparisonResults.Add(new TypeFeatureComparisonResult(
            //        TypeComparisonFeature.HandlerActionMethod,
            //        false,
            //        $"Custom attribute count does not match between action methods ({attributesForActionMethod1.Count()} vs {attributesForActionMethod2.Count()})."));
            //} else
            //{
            //    // TODO
            //}

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
