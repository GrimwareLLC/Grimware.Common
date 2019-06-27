using System;
using System.Collections.Generic;

namespace Grimware
{
    public static class EqualityComparer
    {
        public static IEqualityComparer<T> Create<T>(Func<T, T, bool> equalityComparison, Func<T, int> hashFunction)
        {
            if (equalityComparison == null)
                throw new ArgumentNullException(nameof(equalityComparison));
            if (hashFunction == null)
                throw new ArgumentNullException(nameof(hashFunction));

            return new AbstractEqualityComparer<T>(equalityComparison, hashFunction);
        }

        public static IEqualityComparer<T> Create<T, TKey>(Func<T, TKey> keySelector)
        {
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return Create(
                keySelector,
                (x, y) => ReferenceEquals(x, y) || (x != null && x.Equals(y)),
                k => k.GetHashCode()
                );
        }

        public static IEqualityComparer<T> Create<T, TKey>(
            Func<T, TKey> keySelector,
            Func<TKey, TKey, bool> keyEqualityComparison,
            Func<TKey, int> keyHashFunction
            )
        {
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (keyEqualityComparison == null)
                throw new ArgumentNullException(nameof(keyEqualityComparison));
            if (keyHashFunction == null)
                throw new ArgumentNullException(nameof(keyHashFunction));

            return Create(
                keySelector,
                new AbstractEqualityComparer<TKey>(keyEqualityComparison, keyHashFunction)
                );
        }

        public static IEqualityComparer<T> Create<T, TKey>(
            Func<T, TKey> keySelector,
            IEqualityComparer<TKey> keyEqualityComparer
            )
        {
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (keyEqualityComparer == null)
                throw new ArgumentNullException(nameof(keyEqualityComparer));

            return new AbstractEqualityComparer<T>(
                (x, y) => keyEqualityComparer.Equals(keySelector(x), keySelector(y)),
                t => keyEqualityComparer.GetHashCode(keySelector(t))
                );
        }
    }
}
