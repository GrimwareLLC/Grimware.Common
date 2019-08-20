using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Grimware.Extensions
{
    public static partial class EnumerableExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<int> AllIndexesWhere<T>(this IEnumerable<T> source, Predicate<T> predicate)
        {
            return source?
                .Select((t, i) => new {Index = i, IsMatch = predicate(t)})
                .Where(a => a.IsMatch)
                .Select(a => a.Index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> source, T item)
        {
            return source == null ? Enumerable.Repeat(item, 1) : source.Concat(Enumerable.Repeat(item, 1));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TResult> Convert<T, TResult>(this IEnumerable<T> source, Converter<T, TResult> converter)
        {
            if (converter == null) throw new ArgumentNullException(nameof(converter));
            return source?.Select(t => converter(t));
        }

        /// <summary>
        ///     Returns distinct elements from a sequence by using the specified <paramref name="equalityComparison" />
        ///     and <paramref name="hashGenerator" /> to compare items.
        /// </summary>
        /// <typeparam name="T">
        ///     The type of the elements of <paramref name="source" />.
        /// </typeparam>
        /// <param name="source">The sequence from which to remove duplicate elements.</param>
        /// <param name="equalityComparison">
        ///     A <see cref="Func&lt;T, T, Boolean&gt;" /> to evaluate the equality of items from the sequence.
        /// </param>
        /// <param name="hashGenerator">
        ///     A <see cref="Func&lt;T, Int32&gt;" /> to generate hash codes for an item.
        /// </param>
        /// <returns>
        ///     An <see cref="IEnumerable&lt;T&gt;" /> that contains distinct elements from the <paramref name="source" />
        ///     sequence.
        /// </returns>
        public static IEnumerable<T> Distinct<T>(
            this IEnumerable<T> source,
            Func<T, T, bool> equalityComparison,
            Func<T, int> hashGenerator
        )
        {
            if (equalityComparison == null)
                throw new ArgumentNullException(nameof(equalityComparison));

            if (hashGenerator == null)
                throw new ArgumentNullException(nameof(hashGenerator));

            return source?.Distinct(EqualityComparer.Create(equalityComparison, hashGenerator));
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
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source?.Distinct(EqualityComparer.Create(keySelector));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> Except<T>(this IEnumerable<T> source, T item)
        {
            return source?.Except(Enumerable.Repeat(item, 1));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int? FirstIndexWhere<T>(this IEnumerable<T> source, Predicate<T> predicate)
        {
            return source?.AllIndexesWhere(predicate)?.Convert(i => (int?) i).FirstOrDefault();
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null || action == null)
                return;

            foreach (var t in source)
                action(t);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> Last<T>(this IEnumerable<T> source, int count)
        {
            return source?.Reverse().Take(count).Reverse();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> Last<T>(this IEnumerable<T> source, Predicate<T> predicate, int count)
        {
            return source?.Where(t => predicate(t)).Reverse().Take(count).Reverse();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool None<T>(this IEnumerable<T> source)
        {
            return !(source?.Any() ?? false);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool None<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            return !(source?.Any(predicate) ?? false);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> OrderRandom<T>(this IEnumerable<T> source)
        {
            if (source == null) return Enumerable.Empty<T>();

            return
                source
                    .Select(t => new {T = t, Guid = Guid.NewGuid()})
                    .OrderBy(a => a.Guid)
                    .Select(a => a.T);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T TakeRandom<T>(this IEnumerable<T> source)
        {
            return source != null ? TakeRandom(source, 1).FirstOrDefault() : default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> TakeRandom<T>(this IEnumerable<T> source, int count)
        {
            if (source == null || count <= 0) return Enumerable.Empty<T>();

            return OrderRandom(source).Take(count);
        }
    }
}