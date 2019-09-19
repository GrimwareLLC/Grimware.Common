using System;
using System.Collections.Generic;

namespace Grimware
{
    /// <summary>
    ///     Provides a wrapper allowing use of methods requiring IEqualityComparer&lt;T&gt;
    ///     by providing delegates to perform hash code generation and equality comparison.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <typeparam name="T">The type of objects to compare.</typeparam>
    public class AbstractEqualityComparer<T>
        : EqualityComparer<T>
    {
        private readonly Func<T, T, bool> _equalityComparison;
        private readonly Func<T, int> _hashFunction;

        public AbstractEqualityComparer(Func<T, T, bool> equalityComparison, Func<T, int> hashFunction)
        {
            _equalityComparison = equalityComparison ?? throw new ArgumentNullException(nameof(equalityComparison));
            _hashFunction = hashFunction ?? throw new ArgumentNullException(nameof(hashFunction));
        }

        public override bool Equals(T x, T y)
        {
            return _equalityComparison(x, y);
        }

        public override int GetHashCode(T obj)
        {
            return _hashFunction(obj);
        }
    }
}
