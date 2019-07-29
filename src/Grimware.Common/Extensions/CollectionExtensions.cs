using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Grimware.Extensions
{
    public static class CollectionExtensions
    {
        public static void AddIf<T>(this ICollection<T> collection, T item, Predicate<T> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            collection.AddIf(item, predicate(item));
        }

        public static void AddIf<T>(this ICollection<T> collection, T item, bool condition)
        {
            if (collection != null && condition)
                collection.Add(item);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddIfNotExists<T>(this ICollection<T> collection, T item) => collection?.AddIf(item, !collection.Contains(item));

        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            if (collection == null || items == null) return;

            foreach (var item in items)
                collection.Add(item);
        }

        public static void AddRangeIfNotExists<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            if (collection == null || items == null) return;

            foreach (var item in items.Where(t => !collection.Contains(t)))
                collection.Add(item);
        }

        public static void RemoveIf<T>(this ICollection<T> collection, T item, Predicate<T> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            collection.RemoveIf(item, predicate(item));
        }

        public static void RemoveIf<T>(this ICollection<T> collection, T item, bool condition)
        {
            if (collection != null && condition)
                collection.Remove(item);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RemoveIfExists<T>(this ICollection<T> collection, T item) => collection?.RemoveIf(item, collection.Contains(item));

        public static void RemoveRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            if (collection == null || items == null) return;

            foreach (var item in items)
                collection.Remove(item);
        }

        public static void RemoveRangeIfExists<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            if (collection == null || items == null) return;

            foreach (var item in items.Where(collection.Contains))
                collection.Remove(item);
        }
    }
}