using System;
using System.Collections.Generic;
using System.Linq;

namespace Grimware.Extensions
{
    partial class EnumerableExtensions
    {
        public static IDiffResult<T> Diff<T, TKey, TData>(
            this IEnumerable<T> previous,
            IEnumerable<T> current,
            Func<T, TKey> keySelector,
            Func<T, TData> dataSelector
        )
        {
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            if (dataSelector == null)
                throw new ArgumentNullException(nameof(dataSelector));

            var diffContext = new DiffContext<T, TKey>(previous, current, keySelector);

            var modifiedKeys = diffContext.SelectModifiedKeys(dataSelector).ToArray();
            var unmodifiedKeys = diffContext.SelectIntersectingKeys().Except(modifiedKeys).ToArray();

            return new DiffResult<T>
            {
                Removed = SelectValues(diffContext.Previous, diffContext.SelectRemovedKeys()),
                Added = SelectValues(diffContext.Current, diffContext.SelectAddedKeys()),
                Modified = SelectValues(diffContext.Current, modifiedKeys),
                Unmodified = SelectValues(diffContext.Current, unmodifiedKeys)
            };
        }

        public static IEnumerable<T> SelectAdded<T, TKey>(
            this IEnumerable<T> previous,
            IEnumerable<T> current,
            Func<T, TKey> keySelector
        )
        {
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            var diffContext = new DiffContext<T, TKey>(previous, current, keySelector);

            return SelectValues(diffContext.Current, diffContext.SelectAddedKeys());
        }

        public static IEnumerable<T> SelectModified<T, TKey, TData>(
            this IEnumerable<T> previous,
            IEnumerable<T> current,
            Func<T, TKey> keySelector,
            Func<T, TData> dataSelector
        )
        {
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            if (dataSelector == null)
                throw new ArgumentNullException(nameof(dataSelector));

            var diffContext = new DiffContext<T, TKey>(previous, current, keySelector);

            return SelectValues(diffContext.Current, diffContext.SelectModifiedKeys(dataSelector));
        }

        public static IEnumerable<T> SelectRemoved<T, TKey>(
            this IEnumerable<T> previous,
            IEnumerable<T> current,
            Func<T, TKey> keySelector
        )
        {
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            var diffContext = new DiffContext<T, TKey>(previous, current, keySelector);

            return SelectValues(diffContext.Previous, diffContext.SelectRemovedKeys());
        }

        private static IEnumerable<T> SelectValues<T, TKey>(Dictionary<TKey, T> dictionary, IEnumerable<TKey> keys)
        {
            return
                from kv in dictionary
                where keys.Contains(kv.Key)
                select kv.Value;
        }

        private class DiffContext<T, TKey>
        {
            public DiffContext(IEnumerable<T> previous, IEnumerable<T> current, Func<T, TKey> keySelector)
            {
                Previous = (previous ?? Array.Empty<T>()).ToDictionary(keySelector);
                Current = (current ?? Array.Empty<T>()).ToDictionary(keySelector);
            }

            public Dictionary<TKey, T> Current { get; }

            public Dictionary<TKey, T> Previous { get; }

            public IEnumerable<TKey> SelectAddedKeys()
            {
                return Current.Keys.Except(Previous.Keys);
            }

            public IEnumerable<TKey> SelectIntersectingKeys()
            {
                return Current.Keys.Intersect(Previous.Keys);
            }

            public IEnumerable<TKey> SelectModifiedKeys<TData>(Func<T, TData> dataSelector)
            {
                var intersectingValues =
                    from k in SelectIntersectingKeys()
                    select new
                    {
                        Key = k,
                        CurrentData = dataSelector(Current[k]),
                        PreviousData = dataSelector(Previous[k])
                    };

                return
                    from a in intersectingValues
                    where a.CurrentData != null
                        && !a.CurrentData.Equals(a.PreviousData)
                    select a.Key;
            }

            public IEnumerable<TKey> SelectRemovedKeys()
            {
                return Previous.Keys.Except(Current.Keys);
            }
        }

        private struct DiffResult<T>
            : IDiffResult<T>
        {
            public IEnumerable<T> Added { get; set; }

            public IEnumerable<T> Modified { get; set; }

            public IEnumerable<T> Removed { get; set; }

            public IEnumerable<T> Unmodified { get; set; }
        }
    }
}
