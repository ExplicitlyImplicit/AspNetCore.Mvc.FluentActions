// Licensed under the MIT License. See LICENSE file in the root of the solution for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplicitlyImpl.FluentActions.Test.Utils
{
    public class TypeComparer
    {
        public readonly IEnumerable<TypeComparisonFeature> FeaturesToCompare;
        public readonly TypeComparerOptions Options;

        public TypeComparer(IEnumerable<TypeComparisonFeature> featuresToCompare, TypeComparerOptions options)
        {
            FeaturesToCompare = featuresToCompare;
            Options = options;
        }

        public TypeComparisonResults Compare<T1, T2>()
        {
            return Compare(typeof(T1), typeof(T2), FeaturesToCompare, Options);
        }

        public static TypeComparisonResults Compare<T1, T2>(IEnumerable<TypeComparisonFeature> featuresToCompare, TypeComparerOptions options)
        {
            return Compare(typeof(T1), typeof(T2), featuresToCompare, options);
        }

        public TypeComparisonResults Compare(Type type1, Type type2)
        {
            return Compare(type1, type2, FeaturesToCompare, Options);
        }

        public static TypeComparisonResults Compare(Type type1, Type type2, IEnumerable<TypeComparisonFeature> featuresToCompare, TypeComparerOptions options)
        {
            var comparedFeaturesResults = new List<TypeFeatureComparisonResult>();

            foreach (var featureToCompare in featuresToCompare)
            {
                var comparedFeatureResults = CompareFeature(type1, type2, featureToCompare, options);

                comparedFeaturesResults.AddRange(comparedFeatureResults);

                if (options.StopAtFirstMismatch && 
                    comparedFeatureResults.Any(comparedFeatureResult => !comparedFeatureResult.CompleteMatch))
                {
                    break;
                }
            }

            return new TypeComparisonResults(
                featuresToCompare,
                comparedFeaturesResults
            );
        }

        public static IEnumerable<TypeFeatureComparisonResult> CompareFeature(Type type1, Type type2, TypeComparisonFeature feature, TypeComparerOptions options)
        {
            return TypeFeatureComparers.All[feature].Compare(type1, type2, options);
        }
    }

    public class TypeComparerOptions
    {
        public bool StopAtFirstMismatch { get; set; }
    }

    public class TypeComparisonResults
    {
        public readonly IEnumerable<TypeComparisonFeature> ComparedFeatures;

        public readonly IEnumerable<TypeFeatureComparisonResult> ComparedFeaturesResults;

        public readonly bool CompleteMatch;

        public TypeComparisonResults(IEnumerable<TypeComparisonFeature> comparedFeatures, IEnumerable<TypeFeatureComparisonResult> comparedFeaturesResults)
        {
            ComparedFeatures = comparedFeatures;
            ComparedFeaturesResults = comparedFeaturesResults;
            CompleteMatch = comparedFeaturesResults.All(comparison => comparison.CompleteMatch);
        }

        public IEnumerable<TypeComparisonFeature> MismatchingFeatures => ComparedFeaturesResults
                .Where(comparedFeatureResults => !comparedFeatureResults.CompleteMatch)
                .Select(comparedFeatureResults => comparedFeatureResults.ComparedFeature);

        public IEnumerable<TypeFeatureComparisonResult> MismatchingFeaturesResults => ComparedFeaturesResults
                .Where(comparedFeatureResults => !comparedFeatureResults.CompleteMatch);

        public IEnumerable<TypeComparisonFeature> MatchingFeatures => ComparedFeaturesResults
                .Where(comparedFeatureResults => comparedFeatureResults.CompleteMatch)
                .Select(comparedFeatureResults => comparedFeatureResults.ComparedFeature);

        public IEnumerable<TypeFeatureComparisonResult> MatchingFeaturesResults => ComparedFeaturesResults
                .Where(comparedFeatureResults => comparedFeatureResults.CompleteMatch);
    }

    public class TypeFeatureComparisonResult
    {
        public readonly TypeComparisonFeature ComparedFeature;

        public readonly bool CompleteMatch;

        public readonly string Message;

        public TypeFeatureComparisonResult(TypeComparisonFeature comparedFeature, bool completeMatch, string message)
        {
            ComparedFeature = comparedFeature;
            CompleteMatch = completeMatch;
            Message = message;
        }
    }
}
