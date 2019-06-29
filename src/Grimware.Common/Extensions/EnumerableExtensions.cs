using System;
using System.Collections.Generic;
using System.Linq;

namespace Grimware.Extensions
{
    public static partial class EnumerableExtensions
    {
        public static IEnumerable<T> Action<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null)
                yield break;

            foreach (var t in source)
            {
                action(t);
                yield return t;
            }
        }

        public static IEnumerable<int> AllIndexesWhere<T>(this IEnumerable<T> source, Predicate<T> predicate)
        {
            return
                source
                    .Select((t, i) => new { Index = i, IsMatch = predicate(t) })
                    .Where(a => a.IsMatch)
                    .Select(a => a.Index);
        }

        public static IEnumerable<T> Concat<T>(this IEnumerable<T> source, T item)
        {
            return source == null ? Enumerable.Repeat(item, 1) : source.Concat(Enumerable.Repeat(item, 1));
        }

        public static IEnumerable<TResult> Convert<T, TResult>(
            this IEnumerable<T> input,
            Converter<T, TResult> converter)
        {
            return input.Select(t => converter(t));
        }

        /// <summary>
        ///     Returns distinct elements from a sequence by using the specified <paramref name="equalityComparison" />
        ///     and <paramref name="hashCodeGenerator" /> to compare items.
        /// </summary>
        /// <typeparam name="T">
        ///     The type of the elements of <paramref name="source" />.
        /// </typeparam>
        /// <param name="source">The sequence from which to remove duplicate elements.</param>
        /// <param name="equalityComparison">
        ///     A <see cref="Func&lt;T, T, Boolean&gt;" /> to evaluate the equality of items from the sequence.
        /// </param>
        /// <param name="hashCodeGenerator">
        ///     A <see cref="Func&lt;T, Int32&gt;" /> to generate hash codes for an item.
        /// </param>
        /// <returns>
        ///     An <see cref="IEnumerable&lt;T&gt;" /> that contains distinct elements from the <paramref name="source" /> sequence.
        /// </returns>
        public static IEnumerable<T> Distinct<T>(
            this IEnumerable<T> source,
            Func<T, T, bool> equalityComparison,
            Func<T, int> hashCodeGenerator
            )
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (equalityComparison == null)
                throw new ArgumentNullException(nameof(equalityComparison));
            if (hashCodeGenerator == null)
                throw new ArgumentNullException(nameof(hashCodeGenerator));

            return source.Distinct(EqualityComparer.Create(equalityComparison, hashCodeGenerator));
        }

        /// <summary>
        ///     Returns distinct elements from a sequence by finding distinct keys using the specified
        ///     <paramref name="keySelector" />.
        /// </summary>
        /// <typeparam name="T">
        ///     The type of the elements of <paramref name="source" />.
        /// </typeparam>
        /// <typeparam name="TKey">
        ///     The type of keys created by <paramref name="keySelector" />.
        /// </typeparam>
        /// <param name="source">The sequence from which to remove duplicate elements.</param>
        /// <param name="keySelector">
        ///     A <see cref="Func&lt;T, K&gt;" /> to create a key from an item.
        /// </param>
        /// <returns>
        ///     An <see cref="IEnumerable&lt;T&gt;" /> that contains distinct elements from the <paramref name="source" /> sequence
        ///     based on key selection.
        /// </returns>
        public static IEnumerable<T> Distinct<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.Distinct(EqualityComparer.Create(keySelector));
        }

        public static IEnumerable<T> Except<T>(this IEnumerable<T> source, T item)
        {
            return source.Except(Enumerable.Repeat(item, 1));
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null)
                return;
            if (action == null)
                return;

            foreach (var t in source)
                action(t);
        }

        public static int? FirstIndexWhere<T>(this IEnumerable<T> source, Predicate<T> predicate)
        {
            return
                source
                    .Select((t, i) => new { Index = i, IsMatch = predicate(t) })
                    .FirstOrDefault(a => a.IsMatch)
                    ?.Index;
        }

        public static IEnumerable<T> Last<T>(this IEnumerable<T> source, int count)
        {
            return source.Reverse().Take(count).Reverse();
        }

        public static bool None<T>(this IEnumerable<T> source)
        {
            return !source.Any();
        }

        public static bool None<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            return !source.Any(predicate);
        }
    }
}
