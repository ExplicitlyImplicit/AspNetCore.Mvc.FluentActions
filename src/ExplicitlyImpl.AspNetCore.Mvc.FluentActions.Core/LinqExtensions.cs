// Licensed under the MIT License. See LICENSE file in the root of the solution for license information.

using System;
using System.Collections;
using System.Collections.Generic;

namespace ExplicitlyImpl.AspNetCore.Mvc.FluentActions
{
    public static class DivideLinqExtension
    {
        public static IEnumerable<IEnumerable<TSource>> Divide<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, bool putHitElementInNextGroup = false)
        {
            var enumerator = source.GetEnumerator();

            if (!enumerator.MoveNext()) yield break;

            ContiguousGroup<TSource> current = null;
            while (true)
            {
                current = new ContiguousGroup<TSource>(enumerator, predicate, putHitElementInNextGroup);

                yield return current;

                if (current.EndOfCollection)
                {
                    yield break;
                }
            }
        }

        class ContiguousGroup<TSource> : IEnumerable<TSource>
        {
            private IEnumerator<TSource> Enumerator;

            private Func<TSource, bool> Predicate;

            private bool PutHitElementInNextGroup;

            internal bool EndOfCollection;

            public ContiguousGroup(IEnumerator<TSource> enumerator, Func<TSource, bool> predicate, bool putHitElementInNextGroup)
            {
                Enumerator = enumerator;
                Predicate = predicate;
                PutHitElementInNextGroup = putHitElementInNextGroup;
            }

            public IEnumerator<TSource> GetEnumerator()
            {
                var isFirstElement = true;

                while (true)
                {
                    var current = Enumerator.Current;
                    var hit = Predicate(current);

                    if (isFirstElement || !hit || (hit && !PutHitElementInNextGroup))
                    {
                        yield return Enumerator.Current;

                        if (!Enumerator.MoveNext())
                        {
                            EndOfCollection = true;
                            yield break;
                        } else if (hit && !PutHitElementInNextGroup)
                        {
                            yield break;
                        }
                    } else
                    {
                        yield break;
                    }

                    isFirstElement = false;
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
