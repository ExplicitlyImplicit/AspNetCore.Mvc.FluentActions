using System;
using System.Collections.Generic;

namespace ExplicitlyImpl.FluentActions.Test.Utils
{
    public enum TypeComparisonFeature
    {
        Name
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

    public static class TypeFeatureComparers
    {
        public static Dictionary<TypeComparisonFeature, TypeFeatureComparer> All = new Dictionary<TypeComparisonFeature, TypeFeatureComparer> 
        {
            { TypeComparisonFeature.Name , new TypeNameComparer() }
        };
    }
}
